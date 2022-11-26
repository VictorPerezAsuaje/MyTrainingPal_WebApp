using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Domain.Entities
{
    public class Workout : BaseEntity
    {
        public string Name { get; private set; }
        public int NumberOfSets { get; private set; }
        public List<Set> Sets { get; private set; } = new List<Set>();
        public WorkoutType WorkoutType { get; private set; }
        public bool UserMade { get => User != null; }

        public int UserId { get; private set; }
        public User? User { get; private set; }

        Workout(){ }

        public static Result<Workout> Generate(WorkoutType workoutType, string name, int numberOfSets, int userId,
            /* OPTIONAL */
            int? id = null, User? user = null)
        {
            Workout workout = new Workout();

            if (string.IsNullOrEmpty(name))
                return Result.Fail<Workout>("Name is necessary");

            if (numberOfSets < 1)
                return Result.Fail<Workout>("The number of sets can not be less than 1");

            if (userId < 1)
                return Result.Fail<Workout>("User not found");


            // Generation
            if (id != null)
                workout.Id = (int)id;

            if (user != null)
                workout.User = (User)user;

            workout.UserId = userId;
            workout.Name = name;
            workout.WorkoutType = workoutType;
            workout.NumberOfSets = numberOfSets;

            return Result.Ok(workout); 
        }

        public Workout WithSets(List<Set> sets, int numberOfSets)
        {
            Sets = sets;
            NumberOfSets = numberOfSets;
            return this;
        }
    }
}
