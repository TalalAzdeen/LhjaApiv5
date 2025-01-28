using Data;
using Entities;

namespace Api.Repositories
{
    public interface ISubscriptionRepository : IBaseRepository<Subscription>
    {
    }
    public class SubscriptionRepository(DataContext db)
        : BaseRepository<Subscription>(db: db), ISubscriptionRepository
    {

    }
}