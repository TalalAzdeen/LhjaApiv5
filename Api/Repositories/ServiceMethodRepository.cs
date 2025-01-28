using Data;
using Entities;

namespace Api.Repositories;

public interface IServiceMethodRepository : IBaseRepository<ServiceMethod>
{

}
public class ServiceMethodRepository(DataContext db) : BaseRepository<ServiceMethod>(db), IServiceMethodRepository
{
}