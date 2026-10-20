namespace DomainDrivenVerticalSlices.Template.ArchitectureTests;

using System.Reflection;
using System.Reflection.Emit;

internal static class DependencyInspector
{
    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
    private static readonly Dictionary<short, OpCode> Codes = typeof(OpCodes).GetFields()
        .Where(field => field.FieldType == typeof(OpCode))
        .Select(field => (OpCode)field.GetValue(null)!)
        .ToDictionary(code => code.Value);

    public static IEnumerable<Type> GetReferencedTypes(Type type) =>
        GetRawReferences(type).SelectMany(Expand).Distinct();

    private static IEnumerable<Type> Expand(Type type)
    {
        yield return type;
        if (type.HasElementType)
        {
            foreach (Type element in Expand(type.GetElementType()!)) { yield return element; }
        }

        foreach (Type argument in type.GetGenericArguments())
        {
            foreach (Type nested in Expand(argument)) { yield return nested; }
        }
    }

    private static IEnumerable<Type> GetRawReferences(Type type)
    {
        if (type.BaseType is not null) { yield return type.BaseType; }
        foreach (Type item in type.GetInterfaces()) { yield return item; }
        foreach (FieldInfo field in type.GetFields(Flags)) { yield return field.FieldType; }
        foreach (PropertyInfo property in type.GetProperties(Flags)) { yield return property.PropertyType; }
        foreach (MethodBase method in type.GetMethods(Flags).Cast<MethodBase>().Concat(type.GetConstructors(Flags)))
        {
            if (method is MethodInfo info) { yield return info.ReturnType; }
            foreach (ParameterInfo parameter in method.GetParameters()) { yield return parameter.ParameterType; }
            MethodBody? body = method.GetMethodBody();
            if (body is null) { continue; }
            foreach (LocalVariableInfo local in body.LocalVariables) { yield return local.LocalType; }
            foreach (ExceptionHandlingClause clause in body.ExceptionHandlingClauses)
            {
                if (clause.Flags == ExceptionHandlingClauseOptions.Clause && clause.CatchType is not null) { yield return clause.CatchType; }
            }

            byte[] bytes = body.GetILAsByteArray()!;
            int position = 0;
            while (position < bytes.Length)
            {
                short value = bytes[position++];
                if (value == 0xfe) { value = unchecked((short)(0xfe00 | bytes[position++])); }
                OpCode code = Codes[value];
                if (code.OperandType is OperandType.InlineType or OperandType.InlineMethod or OperandType.InlineField or OperandType.InlineTok)
                {
                    int token = BitConverter.ToInt32(bytes, position);
                    MemberInfo? member = method.Module.ResolveMember(token, type.GetGenericArguments(), method.IsGenericMethod ? method.GetGenericArguments() : null);
                    if (member is Type referenced) { yield return referenced; }
                    if (member?.DeclaringType is Type owner) { yield return owner; }
                    if (member is FieldInfo field) { yield return field.FieldType; }
                    if (member is MethodInfo called)
                    {
                        yield return called.ReturnType;
                        foreach (Type argument in called.GetGenericArguments()) { yield return argument; }
                    }

                    if (member is MethodBase calledMethod)
                    {
                        foreach (ParameterInfo parameter in calledMethod.GetParameters()) { yield return parameter.ParameterType; }
                    }
                }

                position += code.OperandType switch
                {
                    OperandType.InlineNone => 0,
                    OperandType.ShortInlineBrTarget or OperandType.ShortInlineI or OperandType.ShortInlineVar => 1,
                    OperandType.InlineVar => 2,
                    OperandType.InlineI8 or OperandType.InlineR => 8,
                    OperandType.InlineSwitch => 4 + (4 * BitConverter.ToInt32(bytes, position)),
                    _ => 4,
                };
            }
        }
    }
}
