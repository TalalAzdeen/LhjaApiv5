using Data;
using Entities;

namespace Api.Repositories;

public interface IUserClaimRepository : IBaseRepository<ApplicationUserClaim>
{
}
public class UserClaimRepository(DataContext db) : BaseRepository<ApplicationUserClaim>(db), IUserClaimRepository
{
}