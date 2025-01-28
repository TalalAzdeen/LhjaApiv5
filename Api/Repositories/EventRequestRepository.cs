using Data;
using Entities;

namespace Api.Repositories;

public interface IEventRequestRepository : IBaseRepository<EventRequest>
{
}
public class EventRequestRepository(DataContext db) : BaseRepository<EventRequest>(db), IEventRequestRepository
{
}