namespace EarthQuakeNews.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<T>
    {
        void Add(T entity);
        void AddRange(List<T> entities);
        void Update(T entity);
        void UpdateRange(List<T> entities);
        void Delete(T entity);
        Task SaveAsync();
    }
}
