using MyTrainingPal.Domain.Common;

namespace MyTrainingPal.Domain.Interfaces
{
    public interface IRepository<T> : IReadOnlyRepository<T>
    {
        Result Add(T entity);
        Result Delete(int id);
        Result Update(T entity);
    }
}
