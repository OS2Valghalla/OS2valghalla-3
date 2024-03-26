using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Valghalla.Application.AuditLog;
using Valghalla.Application.Auth;
using Valghalla.Application.Communication;
using Valghalla.Application.Configuration.Interfaces;
using Valghalla.Application.Storage;
using Valghalla.Application.TaskValidation;
using Valghalla.Database;
using Valghalla.Infrastructure.AuditLog;
using Valghalla.Infrastructure.Communication;
using Valghalla.Infrastructure.Configuration;
using Valghalla.Infrastructure.FileStorage;
using Valghalla.Infrastructure.TaskValidation;
using Valghalla.Infrastructure.User;

namespace Valghalla.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
        {
            services.AddDatabase();
            services.AddAutoMapper(typeof(AssemblyReference).GetTypeInfo().Assembly);
            services.AddDataProtection();

            services.AddScoped<IAuditLogCommandRepository, AuditLogCommandRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IFileStorageRepository, FileStorageRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<ITaskValidationRepository, TaskValidationRepository>();
            services.AddScoped<ICommunicationLogRepository, CommunicationLogRepository>();
            services.AddScoped<ICommunicationQueryRepository, CommunicationQueryRepository>();

            return services;
        }
    }
}
