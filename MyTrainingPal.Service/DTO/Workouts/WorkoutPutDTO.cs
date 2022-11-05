using MyTrainingPal.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyTrainingPal.Service.DTO.Workouts;

public class WorkoutPutDTO
{
    public int? Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [Range(1, 20)]
    public int NumberOfSets { get; set; }

    [Required]
    public List<SetPostDTO> SetPostDTOs { get; set; } = new List<SetPostDTO>();

    public SetType SetType { get; set; }
    public string SelectedSetType { get; set; }

    public string? AvailableExercisesJSON { get; set; }

    public WorkoutType WorkoutType { get; set; }

    [Required]
    public string SelectedWorkoutType { get; set; }
}
