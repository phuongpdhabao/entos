
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using ENTOS.SharedKernel.Interfaces; // Ensure this namespace is included for the 'Scan' method  


namespace ENTOS.SharedKernel.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScrutorApplicationServices(this IServiceCollection services)
        {
            var except = new[]
            {
                typeof(ITransientDependency),
                typeof(IScopedDependency),
                typeof(ISingletonDependency)
            };

            var rootNamespace = AppDomain.CurrentDomain.FriendlyName.Split('.')[0];

            var assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a =>
                    !a.IsDynamic &&
                    a.GetName().Name != null &&
                    a.GetName().Name.StartsWith(rootNamespace))
                .ToArray();

            services.Scan(scan => scan
                .FromAssemblies(assemblies)

                // Transient
                .AddClasses(c => c.AssignableTo<ITransientDependency>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .As(t =>
                    {
                        var interfaces = t.GetInterfaces().Except(except).ToArray();
                        return interfaces.Length > 0 ? interfaces : new[] { t };
                    })
                    .WithTransientLifetime()

                // Scoped
                .AddClasses(c => c.AssignableTo<IScopedDependency>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .As(t =>
                    {
                        var interfaces = t.GetInterfaces().Except(except).ToArray();
                        return interfaces.Length > 0 ? interfaces : new[] { t };
                    })
                    .WithScopedLifetime()

                // Singleton
                .AddClasses(c => c.AssignableTo<ISingletonDependency>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .As(t =>
                    {
                        var interfaces = t.GetInterfaces().Except(except).ToArray();
                        return interfaces.Length > 0 ? interfaces : new[] { t };
                    })
                    .WithSingletonLifetime()
            );

            return services;
        }


    }
}
