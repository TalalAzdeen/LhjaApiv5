
using Api.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CardShapping.Api.RepositoryAPI.SerchRepository
{
    public interface ISearchRepository<T> : IBaseRepository<T> where T : class

    {
        DbSet<T> DBSet { get; set; }
        Task<IQueryable<T>> ReadAllAsyncSerchName(Expression<Func<T,bool>> filter,string  Typecolumn);
        Task<IQueryable<T>>SerchasyncId(Expression<Func<T,bool>> filter,int id);
        Task<IQueryable<T>>GetAll();

    }
}
