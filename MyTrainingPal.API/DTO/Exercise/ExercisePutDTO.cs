using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.API.DTO.Exercise
{
    public class ExercisePutDTO
    {
        public string Name { get; set; }
        public List<string> SelectedMuscleGroups { get; set; } = new List<string>();
        public DifficultyLevel Level { get; set; }
        public ForceType ForceType { get; set; }
        public bool Equipment { get; set; }
    }
}
