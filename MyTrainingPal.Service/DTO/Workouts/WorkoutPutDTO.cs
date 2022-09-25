using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Service.DTO.Workouts
{
    public class WorkoutPutDTO
    {
        public string Name { get; set; }
        public int NumberOfSets { get; set; }
        public List<SetPostDTO> SetPostDTOs { get; set; } = new List<SetPostDTO>();
        public WorkoutType WorkoutType { get; set; }
    }
}
