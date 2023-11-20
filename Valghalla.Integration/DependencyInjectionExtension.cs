using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Valghalla.Application.AuditLog;
using Valghalla.Application.Communication;
using Valghalla.Application.Configuration.Interfaces;
using Valghalla.Application.CPR;
using Valghalla.Application.DigitalPost;
using Valghalla.Application.Excel;
using Valghalla.Application.Mail;
using Valghalla.Application.Saml;
using Valghalla.Application.Secret;
using Valghalla.Application.SMS;
using Valghalla.Application.Storage;
using Valghalla.Integration.AuditLog;
using Valghalla.Integration.Auth;
using Valghalla.Integration.Communication;
using Valghalla.Integration.Configuration;
using Valghalla.Integration.CPR;
using Valghalla.Integration.DigitalPost;
using Valghalla.Integration.Excel;
using Valghalla.Integration.Mail;
using Valghalla.Integration.Saml;
using Valghalla.Integration.Secret;
using Valghalla.Integration.SMS;
using Valghalla.Integration.Storage;

namespace Valghalla.Integration
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddSaml2Auth(this IServiceCollection services)
        {
            services.AddScoped<ISaml2AuthService, Saml2AuthService>();
            services.AddSingleton<IAuthorizationPolicyProvider, UserAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, UserAuthorizationHandler>();

            services.AddAuthentication("saml2").AddCookie("saml2", delegate (CookieAuthenticationOptions options)
            {
                options.LoginPath = new PathString("/api/auth/login");
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.None;

                options.Events.OnRedirectToLogin = async (c) =>
                {
                    // we dont do redirect in here since it will have CORS issue
                    // instead return 401 code with custom error message and let client side refresh cookie instead
                    c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    c.Response.Headers.Location = c.RedirectUri;

                    await c.Response.WriteAsync("__COOKIE_EXPIRED__", default);
                };
            });

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddIntegration(this IServiceCollection services)
        {
            services.AddSingleton<ISecretService, SecretService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddHttpClient<ITextMessageService, TextMessageService>();
            services.AddScoped<IMailMessageService, MailMessageService>();
            services.AddScoped<DigitalPostMessageHelper>();

            services
                .AddHttpClient<IDigitalPostService, DigitalPostService>()
                .ConfigurePrimaryHttpMessageHandler(sp => DigitalPostHttpClientHandler.Initialize(sp));

            services.AddHttpClient<IDigitalPostService, DigitalPostService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<ICPRService, CPRService>();
            services.AddScoped<ICommunicationHelper, CommunicationHelper>();
            services.AddScoped<ICommunicationService, CommunicationService>();

            services.AddTenantConfigurations();

            return services;
        }

        private static IServiceCollection AddTenantConfigurations(this IServiceCollection services)
        {
            services.AddScoped<ConfigurationContainer>();

            var definedTypes = typeof(Application.AssemblyReference).GetTypeInfo().Assembly.GetTypes();
            var configurationTypes = definedTypes.Where(t => t.IsAssignableTo(typeof(IConfiguration)) && !t.IsAbstract);

            foreach (var configurationType in configurationTypes)
            {
                services.AddScoped(configurationType, serviceProvider =>
                {
                    var container = serviceProvider.GetRequiredService<ConfigurationContainer>();
                    return container.GetConfiguration(configurationType);
                });
            }

            return services;
        }
    }
}
