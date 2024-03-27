using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Valghalla.Application;
using Valghalla.Application.Web;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.App.Interfaces;
using Valghalla.External.Application.Modules.App.Responses;

namespace Valghalla.External.Infrastructure.Modules.App
{
    internal class AppQueryRepository : IAppQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<ParticipantEntity> participants;
        private readonly IQueryable<ElectionEntity> elections;
        private readonly IQueryable<WebPageEntity> webPages;
        private readonly IQueryable<ElectionCommitteeContactInformationEntity> electionCommitteeContactInformation;

        public AppQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            participants = dataContext.Set<ParticipantEntity>().AsNoTracking();
            elections = dataContext.Set<ElectionEntity>().AsNoTracking();
            webPages = dataContext.Set<WebPageEntity>().AsNoTracking();
            electionCommitteeContactInformation = dataContext.Set<ElectionCommitteeContactInformationEntity>().AsNoTracking();
        }

        public async Task<bool> CheckIfElectionIsActivatedAsync(CancellationToken cancellationToken)
        {
            return await elections
                .Where(i => i.Active)
                .AnyAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserTeamResponse>> GetUserTeamsAsync(Guid participantId, CancellationToken cancellationToken)
        {
            var data = await participants
                .Include(i => i.TeamForMembers)
                .Include(i => i.TeamForResponsibles)
                .Where(i => i.Id == participantId)
                .Select(i => new { i.Id, i.TeamForMembers, i.TeamForResponsibles })
                .SingleOrDefaultAsync(cancellationToken);

            if (data == null) return Enumerable.Empty<UserTeamResponse>();

            var teams = new List<UserTeamResponse>();

            teams.AddRange(data.TeamForMembers.Select(i => new UserTeamResponse()
            {
                Id = i.Id,
                Name = i.Name,
            }));

            teams.AddRange(data.TeamForResponsibles.Select(i => new UserTeamResponse()
            {
                Id = i.Id,
                Name = i.Name,
            }));

            teams = teams.DistinctBy(i => i.Id).ToList();

            return teams;
        }

        public async Task<ElectionCommitteeContactInformationPage> GetWebPageAsync(CancellationToken cancellationToken)
        {
            var entity = await electionCommitteeContactInformation
                .Include(i => i.LogoFileReference)
                .Where(i => i.PageName == Constants.WebPages.WebPageName_ElectionCommitteeContactInformation)
                .FirstOrDefaultAsync(cancellationToken);

            return mapper.Map<ElectionCommitteeContactInformationPage>(entity);
        }

        public async Task<bool> CheckIfFAQPageActivatedAsync(CancellationToken cancellationToken)
        {
            var entity = await webPages
                .Where(i => i.PageName == Constants.WebPages.WebPageName_FAQPage)
                .FirstOrDefaultAsync(cancellationToken);

            return entity != null ? JsonSerializer.Deserialize<FAQPage>(entity.PageInfo).IsActivated : false;
        }
    }
}
