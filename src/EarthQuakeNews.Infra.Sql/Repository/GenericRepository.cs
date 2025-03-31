using EarthQuakeNews.Domain.Interfaces.Repositories;
using EarthQuakeNews.Domain.Common.Base;
using EarthQuakeNews.Infra.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace EarthQuakeNews.Infra.Sql.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IAggregateRoot
    {
        private readonly EarthQuakeNewsSqlContext _context;
        private readonly DbSet<T> _entity;

        protected GenericRepository(EarthQuakeNewsSqlContext context)
        {
            _context = context;
            _entity = context.Set<T>();
        }

        public void Add(T entity)
        {
            _entity.Add(entity);
        }

        public void AddRange(List<T> entities)
        {
            _entity.AddRange(entities);
        }

        public void Update(T entity)
        {
            _entity.Update(entity);
        }

        public void UpdateRange(List<T> entities)
        {
            _entity.UpdateRange(entities);
        }

        public void Delete(T entity)
        {
            _entity.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
