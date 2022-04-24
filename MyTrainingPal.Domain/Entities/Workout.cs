using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Domain
{
    public class Workout : BaseEntity
    {
        public IList<Set> Sets { get; private set; } = new List<Set>();
        public WorkoutType WorkoutType { get; private set; }
        public DateTime WorkoutDate { get; private set; }

        public Workout(WorkoutType workoutType, DateTime completionDate)
        {
            WorkoutType = workoutType;
            WorkoutDate = completionDate;
        }

        public Result AddWorkoutSets(List<Set> sets)
        {
            // This situation should not occur a priori
            if (sets == null || sets.Any(set => set.Exercise == null))
                return Result.Fail("Internal error. Sets where not added. Please contact support.");

            if (sets.Count == 0)
                return Result.Fail("You have to provide workout sets.");

            Sets = sets;
            return Result.Ok();
        }
    }
}
