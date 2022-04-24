using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Domain.Entities
{
    public class Exercise : BaseEntity
    {
        public string Name { get; } 
        public List<MuscleGroup> MuscleGroups { get; } = new List<MuscleGroup>();
        public DifficultyLevel Level { get; }
        public ForceType ForceType { get; }
        public bool Equipment { get; }
        public MediaGallery? Gallery { get; } 

        Exercise(string name, List<MuscleGroup> muscleGroups, DifficultyLevel level, ForceType forceType, bool hasEquipment, int? id = null) 
        {
            if (id != null) Id = (int)id;
            Name = name;
            MuscleGroups = muscleGroups;
            Level = level;
            ForceType = forceType;
            Equipment = hasEquipment;
        }

        public static Result<Exercise> Generate(string name, List<MuscleGroup> muscleGroups, DifficultyLevel level, ForceType forceType, bool hasEquipment, int? id = null)
        {
            if (muscleGroups.Any(mg => mg == null || mg?.Id == null || mg?.Name == null))
                return Result.Fail<Exercise>(new Tuple<ResultType, string>(ResultType.NullParameter, "Internal error. Exercise could not be generated, please contact support."));

            if (name == "" || name == null) 
                return Result.Fail<Exercise>(new Tuple<ResultType, string>(ResultType.NullParameter, "Name can not be empty."));

            Exercise exercise = new Exercise(name, muscleGroups, level, forceType, hasEquipment, id);

            return Result.Ok(exercise);
        }
    }
}
