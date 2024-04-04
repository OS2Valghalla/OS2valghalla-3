using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Valghalla.Application.Auth;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Infrastructure.User
{
    internal class UserTokenRepository : IUserTokenRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<UserTokenEntity> userTokens;
        private readonly IDataProtector dataProtector;

        public UserTokenRepository(DataContext dataContext, IDataProtectionProvider dataProtectionProvider)
        {
            this.dataContext = dataContext;
            userTokens = dataContext.Set<UserTokenEntity>();
            dataProtector = dataProtectionProvider.CreateProtector(nameof(UserTokenRepository));
        }

        public async Task AddUserTokenAsync(UserToken token, CancellationToken cancellationToken)
        {
            var entity = new UserTokenEntity()
            {
                Id = token.Key.Identifier,
                Code = token.Key.Code,
                Value = Encrypt(token.Value),
                CreatedAt = token.CreatedAt,
                ExpiredAt = token.ExpiredAt,
            };

            await userTokens.AddAsync(entity, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserToken>> GetUserTokensAsync(Guid identifier, CancellationToken cancellationToken)
        {
            var entities = await userTokens
                .AsNoTracking()
                .Where(i => i.Id == identifier)
                .ToArrayAsync(cancellationToken);

            return entities
                .Select(i => new UserToken()
                {
                    Key = new UserToken.TokenKey()
                    {
                        Identifier = i.Id,
                        Code = i.Code,
                    },
                    Value = Decrypt(i.Value),
                    CreatedAt = i.CreatedAt,
                    ExpiredAt = i.ExpiredAt,
                })
                .ToArray();
        }

        public async Task RemoveExpiredUserTokensAsync(CancellationToken cancellationToken)
        {
            await userTokens
                .AsNoTracking()
                .Where(i => i.ExpiredAt < DateTime.UtcNow)
                .ExecuteDeleteAsync(cancellationToken);
        }

        private string Encrypt(UserToken.TokenValue value) => dataProtector.Protect(JsonSerializer.Serialize(value));

        private UserToken.TokenValue Decrypt(string value) => JsonSerializer.Deserialize<UserToken.TokenValue>(dataProtector.Unprotect(value))!;
    }
}
