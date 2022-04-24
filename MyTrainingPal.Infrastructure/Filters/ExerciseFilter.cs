using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Infrastructure.Filters
{
    // DTO to filter exercise repository 
    public class ExerciseFilter
    {
        public string? Name { get; set; } = null;
        public List<MuscleGroup>? MuscleGroups { get; set; } = null;
        public DifficultyLevel? Level { get; set; } = null;
        public ForceType? ForceType { get; set; } = null;
        public bool? Equipment { get; set; } = null;
    }
}
