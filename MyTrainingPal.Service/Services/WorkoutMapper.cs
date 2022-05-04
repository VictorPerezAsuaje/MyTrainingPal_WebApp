using MyTrainingPal.Service.DTO.Workouts;
using MyTrainingPal.Service.Interfaces;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using MyTrainingPal.Infrastructure.Repositories;

namespace MyTrainingPal.Service.Services
{
    public interface IWorkoutMapper : IMapper<Workout, WorkoutGetDTO, WorkoutPostDTO, WorkoutPutDTO>
    {    }

    public class WorkoutMapper : IWorkoutMapper
    {
        private readonly IExerciseRepository _exerciseRepo;

        public WorkoutMapper(IExerciseRepository exerciseRepo)
        {
            _exerciseRepo = exerciseRepo;
        }

        public List<WorkoutGetDTO> EntityListToGetDTOList(List<Workout> entityList)
            => entityList.Select(w => EntityToGetDTO(w)).ToList();

        public WorkoutGetDTO EntityToGetDTO(Workout entity)
            => new WorkoutGetDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Sets = entity.Sets,
                WorkoutType = entity.WorkoutType
            };

        public Result<Workout> PostDTOToEntity(WorkoutPostDTO postDTO)
        {
            Result<Workout> resultMap = Workout.Generate
            (
                name: postDTO.Name,
                workoutType: postDTO.WorkoutType
            );

            if (resultMap.IsFailure)
                return Result.Fail<Workout>(resultMap.Error);

            List<Set> sets = new List<Set>();
            foreach (var setPostDTO in postDTO.SetPostDTOs)
            {
                Exercise exercise = _exerciseRepo.GetById(setPostDTO.ExerciseId).Value;
                Result<Set> result = null;

                if (setPostDTO.SetType == SetType.ByTime)
                {
                    result = Set.Generate
                    (
                        exercise: exercise,
                        setType: setPostDTO.SetType,
                        seconds: setPostDTO.Seconds,
                        minutes: setPostDTO.Minutes,
                        hours: setPostDTO.Hours
                    );
                }
                else
                {
                    result = Set.Generate
                    (
                        exercise: exercise,
                        setType: setPostDTO.SetType,
                        repetitions: setPostDTO.Repetitions
                    );
                }

                if (result.IsFailure)
                    return Result.Fail<Workout>(result.Error);

                sets.Add(result.Value);
            }

            Workout workout = resultMap.Value;
            workout.WithSets(sets);

            return Result.Ok(workout);
        }

        public Result<Workout> PutDTOToEntity(Workout currentEntity, WorkoutPutDTO putDTO)
        {
            WorkoutType workoutType = currentEntity.WorkoutType;
            string name = currentEntity.Name;
            List<Set> sets = currentEntity.Sets;

            if(workoutType != putDTO.WorkoutType && putDTO.WorkoutType != default(WorkoutType))
                workoutType = putDTO.WorkoutType;

            if(name != putDTO.Name && !string.IsNullOrEmpty(putDTO.Name))
                name = putDTO.Name;

            Result<Workout> workoutResult = Workout.Generate
            (
                id: currentEntity.Id,
                workoutType: workoutType,
                name: name
            );

            if(workoutResult.IsFailure)
                return workoutResult;

            List<Set> updatedSets = new List<Set>();
            foreach (var setPostDTO in putDTO.SetPostDTOs)
            {
                Exercise exercise = _exerciseRepo.GetById(setPostDTO.ExerciseId).Value;
                Result<Set> setResult = null;

                if (setPostDTO.SetType == SetType.ByTime)
                {
                    setResult = Set.Generate
                    (
                        exercise: exercise,
                        setType: setPostDTO.SetType,
                        seconds: setPostDTO.Seconds,
                        minutes: setPostDTO.Minutes,
                        hours: setPostDTO.Hours
                    );
                }
                else
                {
                    setResult = Set.Generate
                    (
                        exercise: exercise,
                        setType: setPostDTO.SetType,
                        repetitions: setPostDTO.Repetitions
                    );
                }

                if (setResult.IsFailure)
                    return Result.Fail<Workout>(setResult.Error);

                updatedSets.Add(setResult.Value);
            }

            Workout updatedWorkout = null;

            if (updatedSets.Count == 0)
                updatedWorkout = workoutResult.Value.WithSets(sets);
            else
                updatedWorkout = workoutResult.Value.WithSets(updatedSets);

            return Result.Ok(updatedWorkout);
        }
    }
}
