using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Service.DTO.Exercise
{
    public class ExerciseGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> MuscleGroupsNames { get; set; } = new List<string>();
        public DifficultyLevel Level { get; set; }
        public ForceType ForceType { get; set; }
        public bool Equipment { get; set; }
    }
}
