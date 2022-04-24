namespace MyTrainingPal.Domain.Interfaces
{
    public interface IGenericRepository<T>
    {
        void Add(T entity);
        void Delete(int id);
        T GetById(int id);
        IEnumerable<T> GetAll(int page = 0, int pageSize = 5);
        void Update(T entity);

    }
}
