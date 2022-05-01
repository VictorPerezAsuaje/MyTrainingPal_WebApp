using MyTrainingPal.Domain.Common;

namespace MyTrainingPal.Domain.Interfaces
{
    public interface IReadOnlyRepository<T>
    {
        Result<T> GetById(int id);
        Result<List<T>> GetAll();
    }
}
