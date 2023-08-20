using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace GameModule.Repositories
{
    internal interface ITribeRepository : IRepository
    {
        Task<Tribe> GetByAccountId(Guid accountId);
    }

    internal class TribeRepository : BaseRepository, ITribeRepository
    {
        public TribeRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {
        }

        async Task<Tribe> ITribeRepository.GetByAccountId(Guid accountId)
        {
            return await _gameModuleDbContext
                .Tribes
                .SingleOrDefaultAsync(x => x.AccountId == accountId) 
                ?? throw new ArgumentException($"Ops GameModul! Given accountId {accountId} have no tribe linked :/");
        }
    }
}
