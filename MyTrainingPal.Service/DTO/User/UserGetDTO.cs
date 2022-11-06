using MyTrainingPal.Backlog.Models;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.Service.DTO.User;

public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }
}

public class UserGetDTO
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public bool IsPremium { get; set; } = false;
    public bool IsAdmin { get; set; } = false;
    public string RegistrationDate { get; set; }

    public DateTime FirstCurrentWeekDay => DateTime.Now.StartOfWeek(DayOfWeek.Monday);
    public DateTime LastCurrentWeekDay => DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(6);

    public Dictionary<string, List<ChartDataPoint>> ThisWeeksChart { get; set; }

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

