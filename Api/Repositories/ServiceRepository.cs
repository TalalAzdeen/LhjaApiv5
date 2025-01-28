using Data;
using Entities;

namespace Api.Repositories;

public interface IServiceRepository : IBaseRepository<Service>
{
}
public class ServiceRepository(DataContext db) : BaseRepository<Service>(db), IServiceRepository
{
}