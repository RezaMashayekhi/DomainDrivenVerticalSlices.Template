// Deliberately invalid types are confined to the test assembly.
namespace BoundaryFixtures.Modules.First.Features
{
    internal sealed class BodyOnlyDependency
    {
        public Type Execute() => typeof(Second.Domain.Secret);
    }

    internal sealed class GenericDependency
    {
        public Task<List<Second.Domain.Secret>> Execute() => throw new NotSupportedException();
    }
}

namespace BoundaryFixtures.Modules.Second.Domain
{
    internal sealed class Secret;
}

namespace BoundaryFixtures.Modules.Second.Contracts
{
    internal sealed class PublicContract;
}
