namespace DomainDrivenVerticalSlices.Template.Application.Extensions;

using System.Reflection;
using DomainDrivenVerticalSlices.Template.Application.PipelineBehaviour;
using DomainDrivenVerticalSlices.Template.Common.Mediator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = Assembly.GetExecutingAssembly();
        var domainAssembly = typeof(Domain.Events.Entity1CreatedEvent).Assembly;

        // Register custom mediator as scoped (required because handlers depend on scoped services like repositories)
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<ISender>(sp => sp.GetRequiredService<IMediator>());
        services.AddScoped<IPublisher>(sp => sp.GetRequiredService<IMediator>());

        // Register pipeline behaviors (order matters: exception handling first, then performance, logging, and validation last)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        // Register all handlers from Application and Domain assemblies
        RegisterHandlers(services, applicationAssembly);
        RegisterNotificationHandlers(services, applicationAssembly, domainAssembly);

        // Register validators
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, Assembly assembly)
    {
        // Find all IRequestHandler<,> implementations
        var handlerTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType &&
                           (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                            i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
                .Select(i => new { InterfaceType = i, ImplementationType = t }))
            .ToList();

        foreach (var handler in handlerTypes)
        {
            // Get the base IRequestHandler<,> interface for registration
            var requestHandlerInterface = handler.InterfaceType;
            if (requestHandlerInterface.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                requestHandlerInterface.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
            {
                var args = requestHandlerInterface.GetGenericArguments();
                requestHandlerInterface = typeof(IRequestHandler<,>).MakeGenericType(args);
            }

            services.AddTransient(requestHandlerInterface, handler.ImplementationType);
        }
    }

    private static void RegisterNotificationHandlers(IServiceCollection services, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var notificationHandlerTypes = assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false })
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                    .Select(i => new { InterfaceType = i, ImplementationType = t }))
                .ToList();

            foreach (var handler in notificationHandlerTypes)
            {
                services.AddTransient(handler.InterfaceType, handler.ImplementationType);
            }
        }
    }
}
