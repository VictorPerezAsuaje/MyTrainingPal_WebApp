using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Infrastructure.DTO.Workout
{
    public class WorkoutFilterDTO
    {
        public SetType? SetType { get; set; } = null;
        public WorkoutType? WorkoutType { get; set; } = null;
        public DifficultyLevel? Level { get; set; } = null;
        public bool? Equipment { get; set; } = null;
        public int? UserId { get; set; } = null;
    }
}
