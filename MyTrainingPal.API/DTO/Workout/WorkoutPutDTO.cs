﻿using MyTrainingPal.Domain.Enums;

namespace MyTrainingPal.API.DTO.Workout
{
    public class WorkoutPutDTO
    {
        public string Name { get; set; }
        public List<SetPostDTO> SetPostDTOs { get; set; } = new List<SetPostDTO>();
        public WorkoutType WorkoutType { get; set; }
    }
}
