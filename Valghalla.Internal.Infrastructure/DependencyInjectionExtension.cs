#region Modules
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using Valghalla.Database;
using Valghalla.Infrastructure;
using Valghalla.Internal.Application.Modules.Administration.Area.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Link.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Team.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.User.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Interfaces;
using Valghalla.Internal.Application.Modules.Analyze.Interfaces;
using Valghalla.Internal.Application.Modules.App.Interfaces;
using Valghalla.Internal.Application.Modules.Communication.CommunicationLog.Interfaces;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Area.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Communication.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Election.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.ElectionType.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.SpecialDiet.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.TaskType.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Team.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.User;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Infrastructure.Modules.Administration.Area;
using Valghalla.Internal.Infrastructure.Modules.Administration.Communication;
using Valghalla.Internal.Infrastructure.Modules.Administration.Election;
using Valghalla.Internal.Infrastructure.Modules.Administration.ElectionType;
using Valghalla.Internal.Infrastructure.Modules.Administration.Link;
using Valghalla.Internal.Infrastructure.Modules.Administration.SpecialDiet;
using Valghalla.Internal.Infrastructure.Modules.Administration.TaskType;
using Valghalla.Internal.Infrastructure.Modules.Administration.Team;
using Valghalla.Internal.Infrastructure.Modules.Administration.User;
using Valghalla.Internal.Infrastructure.Modules.Administration.Web;
using Valghalla.Internal.Infrastructure.Modules.Administration.WorkLocation;
using Valghalla.Internal.Infrastructure.Modules.Administration.WorkLocationTemplate;
using Valghalla.Internal.Infrastructure.Modules.Analyze;
using Valghalla.Internal.Infrastructure.Modules.App;
using Valghalla.Internal.Infrastructure.Modules.Communication.CommunicationLog;
using Valghalla.Internal.Infrastructure.Modules.Participant;
using Valghalla.Internal.Infrastructure.Modules.Shared.Area;
using Valghalla.Internal.Infrastructure.Modules.Shared.Communication;
using Valghalla.Internal.Infrastructure.Modules.Shared.Election;
using Valghalla.Internal.Infrastructure.Modules.Shared.ElectionType;
using Valghalla.Internal.Infrastructure.Modules.Shared.Participant;
using Valghalla.Internal.Infrastructure.Modules.Shared.SpecialDiet;
using Valghalla.Internal.Infrastructure.Modules.Shared.TaskType;
using Valghalla.Internal.Infrastructure.Modules.Shared.Team;
using Valghalla.Internal.Infrastructure.Modules.Shared.User;
using Valghalla.Internal.Infrastructure.Modules.Shared.WorkLocation;
using Valghalla.Internal.Infrastructure.Modules.Tasks;
#endregion

namespace Valghalla.Internal.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSharedInfrastructure();
            services.AddAutoMapper(typeof(AssemblyReference).GetTypeInfo().Assembly);
            services.AddQueryEngine(typeof(AssemblyReference), typeof(Application.AssemblyReference));

            #region Administration
            services.AddScoped<IElectionCommandRepository, ElectionCommandRepository>();            
            services.AddScoped<IElectionQueryRepository, ElectionQueryRepository>();
            services.AddScoped<ILinkCommandRepository, LinkCommandRepository>();
            services.AddScoped<ILinkQueryRepository, LinkQueryRepository>();
            services.AddScoped<ISpecialDietCommandRepository, SpecialDietCommandRepository>();
            services.AddScoped<ISpecialDietQueryRepository, SpecialDietQueryRepository>();
            services.AddScoped<ITeamCommandRepository, TeamCommandRepository>();
            services.AddScoped<ITeamQueryRepository, TeamQueryRepository>();
            services.AddScoped<ITaskTypeCommandRepository, TaskTypeCommandRepository>();
            services.AddScoped<ITaskTypeQueryRepository, TaskTypeQueryRepository>();
            services.AddScoped<IAreaCommandRepository, AreaCommandRepository>();
            services.AddScoped<IAreaQueryRepository, AreaQueryRepository>();
            services.AddScoped<IUserCommandRepository, UserCommandRepository>();
            services.AddScoped<IUserQueryRepository, UserQueryRepository>();
            services.AddScoped<IElectionTypeCommandRepository, ElectionTypeCommandRepository>();
            services.AddScoped<IElectionTypeQueryRepository, ElectionTypeQueryRepository>();
            services.AddScoped<IWebPageCommandRepository, WebPageCommandRepository>();
            services.AddScoped<IWebPageQueryRepository, WebPageQueryRepository>();
            services.AddScoped<IElectionCommitteeContactInformationCommandRepository, ElectionCommitteeContactInformationCommandRepository>();
            services.AddScoped<IElectionCommitteeContactInformationQueryRepository, ElectionCommitteeContactInformationQueryRepository>();
            services.AddScoped<IWorkLocationCommandRepository, WorkLocationCommandRepository>();
            services.AddScoped<IWorkLocationQueryRepository, WorkLocationQueryRepository>();
            services.AddScoped<IWorkLocationTemplateCommandRepository, WorkLocationTemplateCommandRepository>();
            services.AddScoped<IWorkLocationTemplateQueryRepository, WorkLocationTemplateQueryRepository>();
            services.AddScoped<ICommunicationCommandRepository, CommunicationCommandRepository>();
            services.AddScoped<ICommunicationQueryRepository, CommunicationQueryRepository>();
            #endregion

            #region Analyze
            services.AddScoped<IAnalyzeCommandRepository, AnalyzeCommandRepository>();
            services.AddScoped<IAnalyzeQueryRepository, AnalyzeQueryRepository>();
            #endregion

            #region App
            services.AddScoped<IAppElectionQueryRepository, AppElectionQueryRepository>();
            #endregion

            #region Participant
            services.AddScoped<IParticipantCommandRepository, ParticipantCommandRepository>();
            services.AddScoped<IParticipantQueryRepository, ParticipantQueryRepository>();
            services.AddScoped<IParticipantEventLogCommandRepository, ParticipantEventLogCommandRepository>();
            services.AddScoped<IParticipantGdprQueryRepository, ParticipantGdprQueryRepository>();
            services.AddScoped<IParticipantGdprCommandRepository, ParticipantGdprCommandRepository>();
            #endregion

            #region Tasks
            services.AddScoped<IElectionWorkLocationTasksQueryRepository, ElectionWorkLocationTasksQueryRepository>();
            services.AddScoped<IElectionWorkLocationTasksCommandRepository, ElectionWorkLocationTasksCommandRepository>();
            services.AddScoped<IElectionAreaTasksQueryRepository, ElectionAreaTasksQueryRepository>();
            services.AddScoped<IFilteredTasksQueryRepository, FilteredTasksQueryRepository>();
            #endregion

            #region Communication
            services.AddScoped<ICommunicationLogQueryRepository, CommunicationLogQueryRepository>();
            #endregion

            #region Shared
            services.AddScoped<IUserSharedQueryRepository, UserSharedQueryRepository>();
            services.AddScoped<IElectionSharedQueryRepository, ElectionSharedQueryRepository>();
            services.AddScoped<IElectionTypeSharedQueryRepository, ElectionTypeSharedQueryRepository>();
            services.AddScoped<IWorkLocationSharedQueryRepository, WorkLocationSharedQueryRepository>();
            services.AddScoped<ICommunicationSharedQueryRepository, CommunicationSharedQueryRepository>();
            services.AddScoped<ITaskTypeSharedQueryRepository, TaskTypeSharedQueryRepository>();
            services.AddScoped<ITeamSharedQueryRepository, TeamSharedQueryRepository>();
            services.AddScoped<ISpecialDietSharedQueryRepository, SpecialDietSharedQueryRepository>();
            services.AddScoped<IAreaSharedQueryRepository, AreaSharedQueryRepository>();
            services.AddScoped<IParticipantSharedQueryRepository, ParticipantSharedQueryRepository>();
            #endregion

            return services;
        }
    }
}
