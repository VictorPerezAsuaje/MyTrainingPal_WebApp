using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Service.DTO.User
{
    public class UserGetDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsPremium { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public string RegistrationDate { get; set; }
        public Dictionary<WorkoutType, int> WorkoutTypeFrequency
        {
            get
            {
                return GetWorkoutFrequency();
            }
        }

        public List<WorkoutHistory> CompletedWorkouts { get; set; } = new List<WorkoutHistory>();

        private Dictionary<WorkoutType, int> GetWorkoutFrequency()
        {
            Dictionary<WorkoutType, int> frequency = new Dictionary<WorkoutType, int>();

            CompletedWorkouts.ForEach(x =>
            {
                if(frequency.ContainsKey(x.Workout.WorkoutType))
                    frequency[x.Workout.WorkoutType]++;
                else
                    frequency[x.Workout.WorkoutType] = 1;
            });

            foreach(var pair in frequency)
            {
                frequency[pair.Key] = (pair.Value * 100) / CompletedWorkouts.Count ;
            }

            return frequency;
        }
    }

}
