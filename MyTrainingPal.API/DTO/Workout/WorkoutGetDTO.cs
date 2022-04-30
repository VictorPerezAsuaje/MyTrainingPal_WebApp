using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.API.DTO.Workout
{
    public class WorkoutGetDTO
    {
        public string Name { get; set; }
        public List<Set> Sets { get; set; } = new List<Set>();
        public WorkoutType WorkoutType { get; set; }
    }
}
