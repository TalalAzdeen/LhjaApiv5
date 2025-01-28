using Data;
using Entities;

namespace Api.Repositories;



public interface IModelAiRepository : IBaseRepository<ModelAi>
{

}
public class ModelAiRepository(DataContext db) : BaseRepository<ModelAi>(db), IModelAiRepository
{

}