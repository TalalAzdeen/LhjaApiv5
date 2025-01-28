using Data;
using Entities;

namespace Api.Repositories;

public interface ISettingRepository : IBaseRepository<Setting>
{
}
public class SettingRepository(DataContext db) : BaseRepository<Setting>(db), ISettingRepository
{
}