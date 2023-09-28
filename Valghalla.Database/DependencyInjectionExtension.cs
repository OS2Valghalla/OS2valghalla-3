using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Attributes;
using Valghalla.Application.Tenant;
using Valghalla.Database.Interceptors.Audit;
using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AssemblyReference).GetTypeInfo().Assembly);

            services.AddScoped<ChangeTrackingInterceptor>();
            services.AddScoped<GeneralAuditInterceptor>();
            services.AddScoped<ParticipantAuditInterceptor>();

            services.AddScoped<DataContextValidator>();

            services.AddDbContext<DataContext>((serviceProvider, options) =>
            {
                var changeTrackingInterceptor = serviceProvider.GetRequiredService<ChangeTrackingInterceptor>();
                var generalAuditInterceptor = serviceProvider.GetRequiredService<GeneralAuditInterceptor>();
                var participantAuditInterceptor = serviceProvider.GetRequiredService<ParticipantAuditInterceptor>();
                var tenantContextProvider = serviceProvider.GetRequiredService<ITenantContextProvider>();

                options
                    .UseNpgsql(tenantContextProvider.CurrentTenant.ConnectionString)
                    .AddInterceptors(
                        changeTrackingInterceptor,
                        generalAuditInterceptor,
                        participantAuditInterceptor);
            });

            return services;
        }

        public static IServiceCollection AddQueryEngine(this IServiceCollection services, Type infrastructureAssembly, Type applicationAssembly)
        {
            var definedTypes = infrastructureAssembly.GetTypeInfo().Assembly.GetTypes();
            var definedApplicationTypes = applicationAssembly.GetTypeInfo().Assembly.GetTypes();
            var queryFormTypes = definedApplicationTypes.Where(t =>
                t.IsAssignableTo(typeof(QueryForm)) &&
                t != typeof(QueryForm) &&
                t != typeof(QueryForm<,>));

            foreach (var queryFormType in queryFormTypes)
            {
                if (queryFormType.IsAbstract) continue;

                var queryFormBaseType = queryFormType.BaseType!;
                var genericArgs = queryFormBaseType.GetGenericArguments();

                while (queryFormBaseType != typeof(object) && !genericArgs.Any())
                {
                    queryFormBaseType = queryFormBaseType.BaseType!;
                    genericArgs = queryFormBaseType.GetGenericArguments();
                }

                if (!genericArgs.Any())
                {
                    continue;
                }

                var queryResultItemType = genericArgs[0];
                var queryFormParamType = genericArgs[1];
                var queryEngineRepositoryInterfaceType = typeof(IQueryEngineRepository<,,>).MakeGenericType(queryFormType, queryResultItemType, queryFormParamType);                
                var queryEngineRepositoryType = definedTypes.Single(i => i.IsAssignableTo(queryEngineRepositoryInterfaceType) && !Attribute.IsDefined(i, typeof(QueryEngineIgnoreAttribute)));

                services.AddScoped(queryEngineRepositoryInterfaceType, queryEngineRepositoryType);
            }

            return services;
        }
    }
}
