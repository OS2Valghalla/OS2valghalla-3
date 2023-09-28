using Microsoft.Extensions.DependencyInjection;
using Valghalla.Application;

namespace Valghalla.External.Application
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(AssemblyReference);

            services.AddSharedServices();
            services.AddCqrs(assembly);
            services.AddConfirmators(assembly);
            services.AddQueryEngineHandler(assembly);

            return services;
        }
    }
}
