using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Service.DTO.Workouts
{
    public class WorkoutGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Set> Sets { get; set; } = new List<Set>();
        public WorkoutType WorkoutType { get; set; }
    }
}
