using Valghalla.Application.Secret;
using Valghalla.Application.Tenant;
using Valghalla.External.API.Auth;

namespace Valghalla.External.API.Middlewares
{
    internal class TenantContextHandlingMiddleware : IMiddleware
    {
        private readonly TenantContextInternalProvider internalProvider;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ISecretService secretService;

        public TenantContextHandlingMiddleware(
            TenantContextInternalProvider internalProvider,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            ISecretService secretService)
        {
            this.internalProvider = internalProvider;
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
            this.secretService = secretService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            TenantContext tenantContext;

            if (webHostEnvironment.IsDevelopment())
            {
                var value = await GetTenantContextAsync("localhost");

                tenantContext = new TenantContext()
                {
                    Name = value.Name,
                    InternalDomain = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}",
                    ExternalDomain = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}",
                    ConnectionString = value.ConnectionString,
                    AngularDevServer = configuration.GetValue<string>("AngularDevServer")
                };
            }
            else
            {
                var domain = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}";
                tenantContext = await GetTenantContextAsync(domain);
            }

            if (string.IsNullOrEmpty(tenantContext.ConnectionString)
                && !context.Request.Path.ToString().ToLower().Contains("_/not-found")
                && !context.Request.Path.ToString().ToLower().Contains("api/")
                && string.IsNullOrEmpty(Path.GetExtension(context.Request.Path)))
            {
                context.Response.Redirect("/_/not-found");
                return;
            }
            else
            {
                internalProvider.SetTenantContext(tenantContext);
            }

            await next(context);
        }

        private async Task<TenantContext> GetTenantContextAsync(string domain)
        {
            var tenants = await secretService.GetTenantConfigurationsAsync(default);

            if (!tenants.Any())
            {
                throw new Exception("Could not get database tenant information");
            }

            var datebaseTenantInfo = tenants.SingleOrDefault(i => i.ExternalDomain.Equals(domain, StringComparison.OrdinalIgnoreCase));

            return datebaseTenantInfo ?? throw new Exception("Could not find database tenant information matching internal domain: " + domain);
        }
    }
}
