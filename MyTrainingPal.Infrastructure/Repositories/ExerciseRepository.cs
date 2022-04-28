using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Interfaces;

namespace MyTrainingPal.Infrastructure.Repositories
{
    public interface IExerciseRepository : IRepository<Exercise>
    {
    }
    public class ExerciseRepository : IExerciseRepository
    {
        public void Add(Exercise entity) 
        {
            throw new NotImplementedException();

        }

        public void Delete(int id)
        {
            throw new NotImplementedException();

        }

        public IEnumerable<Exercise> GetAll(int page = 0, int pageSize = 5)
        {
           throw new NotImplementedException();
        }

        public Exercise GetById(int id)
        {

            throw new NotImplementedException();
        }

        public void Update(Exercise entity)
        {
            throw new NotImplementedException();

        }
    }
}
