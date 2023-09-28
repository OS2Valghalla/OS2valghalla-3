using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Behaviors;
using Valghalla.Application.Confirmations;
using Valghalla.Application.QueryEngine;
using Valghalla.Application.TaskValidation;

namespace Valghalla.Application
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            services.AddScoped<ITaskValidationService, TaskValidationService>();

            return services;
        }

        public static IServiceCollection AddCqrs(this IServiceCollection services, Type assembly)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly.GetTypeInfo().Assembly));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CqrsBehavior<,>));

            ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("en-US");
            services.AddValidatorsFromAssembly(assembly.Assembly);

            return services;
        }

        public static IServiceCollection AddConfirmators(this IServiceCollection services, Type assembly)
        {
            var definedTypes = assembly.GetTypeInfo().Assembly.GetTypes();
            var confirmatorTypes = definedTypes.Where(t =>
                t.IsAssignableTo(typeof(Confimrator)) &&
                !t.IsAbstract);

            foreach (var confirmatorType in confirmatorTypes)
            {
                var baseType = confirmatorType.BaseType!;
                var genericArgs = baseType.GetGenericArguments();

                while (baseType != typeof(object) && !genericArgs.Any())
                {
                    baseType = baseType.BaseType!;
                    genericArgs = baseType.GetGenericArguments();
                }

                if (!genericArgs.Any())
                {
                    continue;
                }

                var genericType = genericArgs.First();
                var interfaceType = typeof(IConfirmator<>).MakeGenericType(genericType);
                services.AddScoped(interfaceType, confirmatorType);
            }

            return services;
        }

        public static IServiceCollection AddQueryEngineHandler(this IServiceCollection services, Type assembly)
        {
            var definedTypes = assembly.GetTypeInfo().Assembly.GetTypes();
            var queryFormTypes = definedTypes.Where(t =>
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
                var queryFormParamsType = genericArgs[1];
                var queryResultType = typeof(QueryResult<>).MakeGenericType(queryResultItemType);

                var queryResultResponseType = typeof(Response<>).MakeGenericType(queryResultType);
                var queryEngineHandlerInterfaceType = typeof(IRequestHandler<,>).MakeGenericType(queryFormType, queryResultResponseType);
                var queryEngineHandlerImplType = typeof(QueryEngineHandler<,,>).MakeGenericType(queryFormType, queryResultItemType, queryFormParamsType);

                var queryFormInfoResponseType = typeof(Response<QueryFormInfo>);
                var queryEngineFormHandlerInterfaceType = typeof(IRequestHandler<,>).MakeGenericType(queryFormParamsType, queryFormInfoResponseType);
                var queryEngineFormHandlerImplType = typeof(QueryEngineFormHandler<,,>).MakeGenericType(queryFormType, queryResultItemType, queryFormParamsType);

                services.AddScoped(queryEngineHandlerInterfaceType, queryEngineHandlerImplType);
                services.AddScoped(queryEngineFormHandlerInterfaceType, queryEngineFormHandlerImplType);
            }

            return services;
        }
    }
}
