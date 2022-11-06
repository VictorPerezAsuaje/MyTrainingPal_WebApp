using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Service.DTO.Exercise
{
    public class ExercisePostDTO
    {
        public string Name { get; set; }
        public List<MuscleGroup> _muscleGroups { get; private set; } = new List<MuscleGroup>();
        public List<string> SelectedMuscleGroups { get; set; } = new List<string>();
        public DifficultyLevel Level { get; set; }
        public ForceType ForceType { get; set; }
        public bool Equipment { get; set; }
        public string VideoUrl { get; set; }
    }
}
