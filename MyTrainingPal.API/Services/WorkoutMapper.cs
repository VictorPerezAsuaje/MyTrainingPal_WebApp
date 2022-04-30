using MyTrainingPal.API.DTO.Exercise;
using MyTrainingPal.API.DTO.Workout;
using MyTrainingPal.API.Interfaces;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;

namespace MyTrainingPal.API.Services
{
    public interface IWorkoutMapper : IMapper<Workout, WorkoutGetDTO, WorkoutPostDTO>
    {    }

    public class WorkoutMapper : IWorkoutMapper
    {
        public List<WorkoutGetDTO> EntityListToGetDTOList(List<Workout> entityList)
            => entityList.Select(w => EntityToGetDTO(w)).ToList();

        public WorkoutGetDTO EntityToGetDTO(Workout entity)
            => new WorkoutGetDTO
            {
                Name = entity.Name,
                Sets = entity.Sets,
                WorkoutType = entity.WorkoutType
            };

        public Result<Workout> PostDTOToEntity(WorkoutPostDTO postDTO)
            => Workout.Generate
            (
                name: postDTO.Name,
                workoutType: postDTO.WorkoutType
            );
    }
}
