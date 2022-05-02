using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Domain.Entities
{
    public class Set : BaseEntity
    {
        public SetType SetType { get; private set; }
        public Exercise Exercise { get; private set; }
        public int Hours { get; private set; }
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }
        public int Repetitions { get; private set; }

        Set(){  }

        public static Result<Set> Generate(Exercise exercise, SetType setType, 
            /* OPTIONAL */
            int? id = null, int? seconds = null, int? repetitions = null, int? minutes = null, int? hours = null)
        {
            Set set = new Set();

            // Validate

            if (exercise == null)
                return Result.Fail<Set>("There was not exercise provided.");

            if (setType == SetType.ByTime && seconds < 1 && minutes <= 0 && hours <= 0)
                return Result.Fail<Set>("The selected set requires that the exercise last at least 1 second.");

            if (setType == SetType.ByRepetition && repetitions <= 0)
                return Result.Fail<Set>("The selected set requires that the exercise to have at least 1 repetition.");


            // Sanitizing the generation
            if (setType == SetType.ByTime)
                repetitions = null;


            if (setType == SetType.ByRepetition)
            {
                hours = null;
                minutes = null;
                seconds = null;
            }

            // Generate

            if (id != null)
                set.Id = (int)id;

            if (repetitions != null)
                set.Repetitions = (int)repetitions;

            if(seconds != null)
                set.Seconds = (int)seconds;

            if (minutes != null)
                set.Minutes = (int)minutes;

            if (hours != null)
                set.Hours = (int)hours;

            set.Exercise = exercise;
            set.SetType = setType;

            return Result.Ok(set);
        }


        public Result<Set> AddTime(int seconds, int minutes = 0, int hours = 0)
        {
            if (SetType == SetType.ByRepetition)
                return Result.Fail<Set>("You can not add time to an exercise meant to be completed via repetitions");

            if (hours < 0 || minutes < 0 || seconds < 0)
                return Result.Fail<Set>("The time values can not be negative");

            if (hours == 0 && minutes == 0 && seconds == 0)
                return Result.Fail<Set>("An exercise needs to last at least 1 second.");

            Seconds = seconds;
            Minutes = minutes;
            Hours = hours;

            return Result.Ok(this);
        }

        public Result<Set> AddReps(int reps)
        {
            if (SetType == SetType.ByTime)
                return Result.Fail<Set>("You can not add repetitions to an exercise meant to be completed via repetitions");

            if (reps <= 0)
                return Result.Fail<Set>("The number of repetitions can not be 0 or lower.");

            Repetitions = reps;
            return Result.Ok(this);
        }
    }
}
