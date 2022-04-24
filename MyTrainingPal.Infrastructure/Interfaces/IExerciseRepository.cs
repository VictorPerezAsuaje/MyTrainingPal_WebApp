using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Interfaces;
using MyTrainingPal.Infrastructure.Filters;

namespace MyTrainingPal.Infrastructure.Interfaces
{
    public interface IExerciseRepository : IGenericRepository<Exercise>
    {
        public IEnumerable<Exercise> FindMatches(ExerciseFilter filter = null, int page = 0, int pageSize = 5);
    }
}
