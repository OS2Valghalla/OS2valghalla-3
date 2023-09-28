using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Valghalla.Infrastructure;
using Valghalla.Worker.Infrastructure.Modules.AuditLog.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Communication.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Election.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Job.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Participant.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Tasks.Repositories;

namespace Valghalla.Worker.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSharedInfrastructure();
            services.AddAutoMapper(typeof(AssemblyReference).GetTypeInfo().Assembly);

            #region
            services.AddScoped<IJobQueryRepository, JobQueryRepository>();
            services.AddScoped<IJobCommandRepository, JobCommandRepository>();
            #endregion

            #region Election
            services.AddScoped<IElectionQueryRepository, ElectionQueryRepository>();
            services.AddScoped<IElectionCommandRepository, ElectionCommandRepository>();
            #endregion

            #region Participant
            services.AddScoped<IParticipantQueryRepository, ParticipantQueryRepository>();
            services.AddScoped<IParticipantCommandRepository, ParticipantCommandRepository>();
            #endregion

            #region Tasks
            services.AddScoped<ITaskTypeQueryRepository, TaskTypeQueryRepository>();
            services.AddScoped<ITaskAssignmentQueryRepository, TaskAssignmentQueryRepository>();
            services.AddScoped<ITaskAssignmentCommandRepository, TaskAssignmentCommandRepository>();
            #endregion

            #region Communication
            services.AddScoped<ICommunicationCommandRepository, CommunicationCommandRepository>();
            services.AddScoped<ICommunicationLogCommandRepository, CommunicationLogCommandRepository>();
            #endregion

            #region Audit Log
            services.AddScoped<IAuditLogCommandRepository, AuditLogCommandRepository>();
            #endregion

            return services;
        }
    }
}
