using Api.Repositories;
using Data;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{




    public interface ISpaceRepository : IBaseRepository<Space>
    {
        Task<Space?> GetByTokenAsync(string token);
        Task<List<Space>> GetBySubscriptionIdAsync(string subscriptionId);
        Task<List<Space>> GetSpacesByRamAsync(int ram);
    }

    public class SpaceRepository : BaseRepository<Space>, ISpaceRepository
    {
        public SpaceRepository(DataContext db) : base(db) { }

        public async Task<Space?> GetByTokenAsync(string token)
        {
            return await _dbSet.FirstOrDefaultAsync(space => space.Token == token);
        }

        public async Task<List<Space>> GetBySubscriptionIdAsync(string subscriptionId)
        {
            return await _dbSet.Where(space => space.SubscriptionId == subscriptionId).ToListAsync();
        }

        public async Task<List<Space>> GetSpacesByRamAsync(int ram)
        {
            return await _dbSet.Where(space => space.Ram == ram).ToListAsync();
        }
    }
}