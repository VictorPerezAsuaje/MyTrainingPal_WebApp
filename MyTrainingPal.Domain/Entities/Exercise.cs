using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Domain.Entities
{

    public class Exercise : BaseEntity
    {
        public string Name { get; private set; } 
        public List<MuscleGroup> MuscleGroups { get; private set; } = new List<MuscleGroup>();
        public DifficultyLevel Level { get; private set; }
        public ForceType ForceType { get; private set; }
        public bool Equipment { get; private set; }
        public string VideoUrl { get; private set; }

        Exercise() { }

        public static Result<Exercise> Generate(string name, List<MuscleGroup> muscleGroups, DifficultyLevel level, ForceType forceType, bool hasEquipment, string videoUrl, int? id = null)
        {
            Exercise exercise = new Exercise();

            if (muscleGroups.Any(mg => mg == null || mg?.Id == null || mg?.Name == null))
                return Result.Fail<Exercise>("Internal error. Exercise could not be generated, please contact support.");

            if (name == "" || name == null)
                return Result.Fail<Exercise>("Name can not be empty.");

            if (string.IsNullOrEmpty(videoUrl))
                return Result.Fail<Exercise>("Video url can not be empty.");

            if (id != null) 
                exercise.Id = (int)id;

            exercise.Name = name;
            exercise.MuscleGroups = muscleGroups;
            exercise.Level = level;
            exercise.ForceType = forceType;
            exercise.Equipment = hasEquipment;
            exercise.VideoUrl = videoUrl;   

            return Result.Ok(exercise);
        }
    }
}
