using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Domain.Entities
{
    public class Workout : BaseEntity
    {
        public string Name { get; private set; }
        public List<Set> Sets { get; private set; } = new List<Set>();
        public WorkoutType WorkoutType { get; private set; }

        Workout(){ }

        public static Result<Workout> Generate(WorkoutType workoutType, string name,
            /* OPTIONAL */
            int? id = null)
        {
            Workout workout = new Workout();

            // Generation
            if(id != null)
                workout.Id = (int)id;

            workout.Name = name;
            workout.WorkoutType = workoutType;

            return Result.Ok(workout); 
        }

        public Workout WithSets(List<Set> sets)
        {
            Sets = sets;
            return this;
        }
    }
}
