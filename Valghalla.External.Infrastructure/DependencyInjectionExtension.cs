using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Valghalla.Database;
using Valghalla.External.Application.Modules.App.Interfaces;
using Valghalla.External.Application.Modules.MyProfile.Interfaces;
using Valghalla.External.Application.Modules.Registration.Interfaces;
using Valghalla.External.Application.Modules.Shared.SpecialDiet.Interfaces;
using Valghalla.External.Application.Modules.Shared.User;
using Valghalla.External.Application.Modules.Tasks.Interfaces;
using Valghalla.External.Application.Modules.Team.Interfaces;
using Valghalla.External.Application.Modules.Unprotected.Interfaces;
using Valghalla.External.Application.Modules.Web.Interfaces;
using Valghalla.External.Application.Modules.WorkLocation.Interfaces;
using Valghalla.External.Infrastructure.Modules.App;
using Valghalla.External.Infrastructure.Modules.MyProfile;
using Valghalla.External.Infrastructure.Modules.Registration;
using Valghalla.External.Infrastructure.Modules.Shared.SpecialDiet;
using Valghalla.External.Infrastructure.Modules.Shared.User;
using Valghalla.External.Infrastructure.Modules.Tasks;
using Valghalla.External.Infrastructure.Modules.Team;
using Valghalla.External.Infrastructure.Modules.Unprotected;
using Valghalla.External.Infrastructure.Modules.Web;
using Valghalla.External.Infrastructure.Modules.WorkLocation;
using Valghalla.Infrastructure;

namespace Valghalla.External.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSharedInfrastructure();
            services.AddAutoMapper(typeof(AssemblyReference).GetTypeInfo().Assembly);
            services.AddQueryEngine(typeof(AssemblyReference), typeof(Application.AssemblyReference));

            #region App
            services.AddScoped<IAppQueryRepository, AppQueryRepository>();
            #endregion

            #region Tasks
            services.AddScoped<ITaskQueryRepository, TaskQueryRepository>();
            services.AddScoped<ITaskCommandRepository, TaskCommandRepository>();
            #endregion

            #region Team
            services.AddScoped<ITeamQueryRepository, TeamQueryRepository>();
            services.AddScoped<ITeamCommandRepository, TeamCommandRepository>();
            #endregion

            #region MyProfile
            services.AddScoped<IMyProfileQueryRepository, MyProfileQueryRepository>();
            services.AddScoped<IMyProfileCommandRepository, MyProfileCommandRepository>();
            #endregion

            #region Registration
            services.AddScoped<IRegistrationQueryRepository, RegistrationQueryRepository>();
            services.AddScoped<IRegistrationCommandRepository, RegistrationCommandRepository>();
            #endregion

            #region Web
            services.AddScoped<IWebPageQueryRepository, WebPageQueryRepository>();
            #endregion

            #region Work Location
            services.AddScoped<IWorkLocationQueryRepository, WorkLocationQueryRepository>();
            #endregion

            #region Shared
            services.AddScoped<IUserSharedQueryRepository, UserSharedQueryRepository>();
            services.AddScoped<ISpecialDietSharedQueryRepository, SpecialDietSharedQueryRepository>();
            #endregion

            #region Unprotected
            services.AddScoped<IUnprotectedTasksQueryRepository, UnprotectedTasksQueryRepository>();
            services.AddScoped<IUnprotectedTeamQueryRepository, UnprotectedTeamQueryRepository>();
            #endregion

            return services;
        }
    }
}
