using ASG.ApiService.Repositories;
using Data;
using Entities;
using System.Linq.Expressions;

namespace Api.Repositories
{
    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {
    }
    public class UserRepository(DataContext db, IUserClaims? userClaims = null) :
        BaseRepository<ApplicationUser>(db), IUserRepository
    {
        public override Task<ApplicationUser> GetByAsync(Expression<Func<ApplicationUser, bool>>? filter = null, bool tracked = false, Func<IQueryable<ApplicationUser>, IQueryable<ApplicationUser>>? setInclude = null)
        {
            return base.GetByAsync(filter ?? (u => u.Id == userClaims!.UserId), tracked, setInclude);
        }



    }
}
