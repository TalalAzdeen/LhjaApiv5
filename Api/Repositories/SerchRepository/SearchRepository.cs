using Api.Repositories;
using AutoMapper;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TEP.Helper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CardShapping.Api.RepositoryAPI.SerchRepository
{
    public class SearchRepository<T> : BaseRepository<T>, ISearchRepository<T> where T : class
    {




        protected readonly DataContext _context;
        protected readonly IMapper _mapperr;









        public SearchRepository(DataContext db) : base(db)
        {
            _context= db;

        }


       

        public DbSet<T> DBSet { get => _context.Set<T>(); set => throw new NotImplementedException(); }

        public async  Task<IQueryable<T>> GetAll()
        {
            var quary = (_context.Set<T>());
            return quary;
        }

        public async Task<IQueryable<T>> ReadAllAsyncSerchName(Expression<Func<T, bool>> filter, string Typecolumn)
        {
            var quary = (_context.Set<T>()).Where(filter);
            return await Task.FromResult(quary);
        }

        public async Task<IQueryable<T>> SerchasyncId(Expression<Func<T, bool>> filter, int id)
        {
            var quary = (_context.Set<T>()).Where(filter);
            return await Task.FromResult(quary);
        }
    }
}
