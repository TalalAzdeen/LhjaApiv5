using Data;
using Entities;

namespace Api.Repositories;

public interface IModelGatewayRepository : IBaseRepository<ModelGateway>
{
    Task<ModelGateway> GetDefault();
}
public class ModelGatewayRepository(DataContext db) : BaseRepository<ModelGateway>(db), IModelGatewayRepository
{
    public async Task<ModelGateway> GetDefault() => await GetByAsync(m => m.IsDefault);
}