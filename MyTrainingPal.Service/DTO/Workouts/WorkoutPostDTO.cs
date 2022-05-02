using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Service.DTO.Workouts
{
    public class WorkoutPostDTO
    {
        public string Name { get; set; }
        public List<SetPostDTO> SetPostDTOs { get; set; } = new List<SetPostDTO>();
        public WorkoutType WorkoutType { get; set; }
    }

    public class SetPostDTO
    {
        public SetType SetType { get; set; }
        public int ExerciseId { get; set; }
        public int? Hours { get; set; }
        public int? Minutes { get; set; }
        public int? Seconds { get; set; }
        public int? Repetitions { get; set; }
    }
}
