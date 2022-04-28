namespace MyTrainingPal.Domain.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Delete(int id);
        void Update(T entity);
        T GetById(int id);
        IEnumerable<T> GetAll(int page = 0, int pageSize = 5);
    }
}
