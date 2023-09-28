using Microsoft.EntityFrameworkCore;
using Valghalla.Application;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Communication.Commands;
using Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Communication
{
    internal class CommunicationCommandRepository: ICommunicationCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<ElectionEntity> elections;
        private readonly DbSet<CommunicationTemplateEntity> communicationTemplates;
        private readonly DbSet<CommunicationTemplateFileEntity> communicationTemplateFiles;

        public CommunicationCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            elections = dataContext.Set<ElectionEntity>();
            communicationTemplates = dataContext.Set<CommunicationTemplateEntity>();
            communicationTemplateFiles = dataContext.Set<CommunicationTemplateFileEntity>();
        }

        public async Task<Guid> CreateCommunicationTemplateAsync(CreateCommunicationTemplateCommand command, CancellationToken cancellationToken)
        {
            var entity = new CommunicationTemplateEntity()
            {
                Id = Guid.NewGuid(),
                Title = command.Title,
                Subject = command.Subject,
                Content = command.Content,
                TemplateType = command.TemplateType
            };

            var templateFileEntities = command.FileReferenceIds.Select(fileRefId => new CommunicationTemplateFileEntity()
            {
                CommunicationTemplateId = entity.Id,
                FileReferenceId = fileRefId,
            });

            await communicationTemplates.AddAsync(entity, cancellationToken);
            await communicationTemplateFiles.AddRangeAsync(templateFileEntities, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task UpdateCommunicationTemplateAsync(UpdateCommunicationTemplateCommand command, CancellationToken cancellationToken)
        {
            var entity = await communicationTemplates
                .Where(i => i.Id == command.Id)
                .SingleAsync(cancellationToken);

            var communicationTemplateFileEntities = await communicationTemplateFiles
                .Where(i => i.CommunicationTemplateId == command.Id)
                .ToArrayAsync(cancellationToken);

            entity.Title = command.Title;
            entity.Subject = command.Subject;
            entity.Content = command.Content;
            entity.TemplateType = command.TemplateType;

            var communicationTemplateFileEntitiesToAdd = command.FileReferenceIds
                .Where(fileRefId => !communicationTemplateFileEntities.Any(i => i.FileReferenceId == fileRefId))
                .Select(fileRefId => new CommunicationTemplateFileEntity()
                {
                    CommunicationTemplateId = entity.Id,
                    FileReferenceId = fileRefId,
                });

            var communicationTemplateFileEntitiesToRemove = communicationTemplateFileEntities
                .Where(i => !command.FileReferenceIds.Any(fileRefId => fileRefId == i.FileReferenceId));

            communicationTemplates.Update(entity);

            if (communicationTemplateFileEntitiesToRemove.Any())
            {
                communicationTemplateFiles.RemoveRange(communicationTemplateFileEntitiesToRemove);
            }

            if (communicationTemplateFileEntitiesToAdd.Any())
            {
                await communicationTemplateFiles.AddRangeAsync(communicationTemplateFileEntitiesToAdd, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCommunicationTemplateAsync(DeleteCommunicationTemplateCommand command, CancellationToken cancellationToken)
        {
            var electionsUsingTemplate = await elections.Where(i => i.ConfirmationOfRegistrationCommunicationTemplateId == command.Id || i.ConfirmationOfCancellationCommunicationTemplateId == command.Id
               || i.InvitationCommunicationTemplateId == command.Id || i.InvitationReminderCommunicationTemplateId == command.Id
               || i.TaskReminderCommunicationTemplateId == command.Id || i.RetractedInvitationCommunicationTemplateId == command.Id).ToArrayAsync(cancellationToken);

            foreach(var electionUsingTemplate in electionsUsingTemplate)
            {
                if (electionUsingTemplate.ConfirmationOfRegistrationCommunicationTemplateId == command.Id)
                {
                    electionUsingTemplate.ConfirmationOfRegistrationCommunicationTemplateId = new Guid(Constants.DefaultCommunicationTemplates.ConfirmationOfRegistrationStringId);
                }
                if (electionUsingTemplate.ConfirmationOfCancellationCommunicationTemplateId == command.Id)
                {
                    electionUsingTemplate.ConfirmationOfCancellationCommunicationTemplateId = new Guid(Constants.DefaultCommunicationTemplates.ConfirmationOfCancellationStringId);
                }
                if (electionUsingTemplate.InvitationCommunicationTemplateId == command.Id)
                {
                    electionUsingTemplate.InvitationCommunicationTemplateId = new Guid(Constants.DefaultCommunicationTemplates.InvitationStringId);
                }
                if (electionUsingTemplate.InvitationReminderCommunicationTemplateId == command.Id)
                {
                    electionUsingTemplate.InvitationReminderCommunicationTemplateId = new Guid(Constants.DefaultCommunicationTemplates.InvitationReminderStringId);
                }
                if (electionUsingTemplate.TaskReminderCommunicationTemplateId == command.Id)
                {
                    electionUsingTemplate.TaskReminderCommunicationTemplateId = new Guid(Constants.DefaultCommunicationTemplates.TaskReminderStringId);
                }
                if (electionUsingTemplate.RetractedInvitationCommunicationTemplateId == command.Id)
                {
                    electionUsingTemplate.RetractedInvitationCommunicationTemplateId = new Guid(Constants.DefaultCommunicationTemplates.RetractedInvitationStringId);
                }

                elections.Update(electionUsingTemplate);
            }

            var entity = await communicationTemplates.SingleAsync(i => i.Id == command.Id, cancellationToken);

            communicationTemplates.Remove(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
