﻿using MyTrainingPal.Service.DTO.Exercise;
using MyTrainingPal.Service.Interfaces;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;

namespace MyTrainingPal.Service.Services
{
    public interface IExerciseMapper : IMapper<Exercise, ExerciseGetDTO, ExercisePostDTO, ExercisePutDTO>
    {    }

    public class ExerciseMapper : IExerciseMapper
    {
        public List<ExerciseGetDTO> EntityListToGetDTOList(List<Exercise> entityList)
            => entityList.Select(e => EntityToGetDTO(e)).ToList();

        public ExerciseGetDTO EntityToGetDTO(Exercise entity)
            => new ExerciseGetDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                MuscleGroupsNames = entity.MuscleGroups.Select(x => x.Name).ToList(),
                Level = entity.Level,
                ForceType = entity.ForceType,
                Equipment = entity.Equipment
            };

        public Result<Exercise> PostDTOToEntity(ExercisePostDTO postDTO)
            => Exercise.Generate
            (
                name: postDTO.Name,
                forceType: postDTO.ForceType,
                hasEquipment: postDTO.Equipment,
                level: postDTO.Level,
                muscleGroups: postDTO._muscleGroups,
                videoUrl: postDTO.VideoUrl
            );

        public Result<Exercise> PutDTOToEntity(Exercise currentEntity, ExercisePutDTO putDTO)
        {
            throw new NotImplementedException();
        }
    }
}
