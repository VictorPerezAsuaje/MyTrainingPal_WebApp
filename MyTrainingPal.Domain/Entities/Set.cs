using MyTrainingPal.Domain.Common;

namespace MyTrainingPal.Domain.Entities
{
    public abstract class Set : BaseEntity
    {
        public Exercise Exercise { get; private set; }
        public Set(Exercise exercise)
        {
            Exercise = exercise;
        }
    }

    public class TimedSet : Set
    {
        public int Hours { get; private set; }
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }
        public TimedSet(Exercise exercise) : base(exercise) { }

        public Result AddTime(int seconds, int minutes = 0, int hours = 0)
        {
            if (hours < 0 || minutes < 0 || seconds < 0)
                return Result.Fail("The time values can not be negative");

            if (hours == 0 && minutes == 0 && seconds == 0)
                return Result.Fail("An exercise needs to last at least 1 second.");

            Seconds = seconds;
            Minutes = minutes;
            Hours = hours;

            return Result.Ok();
        }
    }

    public class RepetitionSet : Set
    {
        public int Repetitions { get; private set; } = 0;

        public RepetitionSet(Exercise exercise) : base(exercise) { }

        public Result AddTime(int reps)
        {
            if (reps <= 0)
                return Result.Fail("The number of repetitions can not be 0 or lower."); 

            Repetitions = reps;
            return Result.Ok();
        }
    }
}
