using MyTrainingPal.Service.DTO.Workouts;
using MyTrainingPal.Service.Interfaces;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using MyTrainingPal.Infrastructure.Repositories;

namespace MyTrainingPal.Service.Services
{
    public interface IWorkoutMapper : IMapper<Workout, WorkoutGetDTO, WorkoutPostDTO, WorkoutPutDTO>
    {
        WorkoutPutDTO EntityToPutDTO(Workout entity);
    }

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
                NumberOfSets = entity.NumberOfSets,
                Sets = entity.Sets,
                WorkoutType = entity.WorkoutType,
                UserId = entity.UserId
            };

        public Result<Workout> PostDTOToEntity(WorkoutPostDTO postDTO)
        {
            Result<Workout> resultMap = Workout.Generate
            (
                name: postDTO.Name,
                workoutType: postDTO.WorkoutType,
                numberOfSets: postDTO.NumberOfSets,
                userId: postDTO.UserId
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
            workout.WithSets(sets, workout.NumberOfSets);

            return Result.Ok(workout);
        }

        public Result<Workout> PutDTOToEntity(Workout currentEntity, WorkoutPutDTO putDTO)
        {
            WorkoutType workoutType = currentEntity.WorkoutType;
            string name = currentEntity.Name;
            List<Set> sets = currentEntity.Sets;
            int numberSets = currentEntity.NumberOfSets;

            if(workoutType != putDTO.WorkoutType && putDTO.WorkoutType != default(WorkoutType))
                workoutType = putDTO.WorkoutType;

            if(name != putDTO.Name && !string.IsNullOrEmpty(putDTO.Name))
                name = putDTO.Name;

            if (numberSets != putDTO.NumberOfSets)
                numberSets = putDTO.NumberOfSets;

            Result<Workout> workoutResult = Workout.Generate
            (
                id: currentEntity.Id,
                workoutType: workoutType,
                name: name,
                numberOfSets: numberSets,
                userId: currentEntity.UserId
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
                updatedWorkout = workoutResult.Value.WithSets(sets, currentEntity.NumberOfSets != putDTO.NumberOfSets ? putDTO.NumberOfSets : currentEntity.NumberOfSets);
            else
                updatedWorkout = workoutResult.Value.WithSets(updatedSets, currentEntity.NumberOfSets != putDTO.NumberOfSets ? putDTO.NumberOfSets : currentEntity.NumberOfSets);

            return Result.Ok(updatedWorkout);
        }

        public WorkoutPutDTO EntityToPutDTO(Workout entity)
         => new WorkoutPutDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                NumberOfSets = entity.NumberOfSets,
                SetPostDTOs = entity.Sets.Select(x => new SetPostDTO()
                {
                    SetType = x.SetType,
                    SelectedSetType = Enum.GetName(x.SetType),
                    ExerciseId = x.Exercise.Id,
                    Hours = x.Hours,
                    Minutes = x.Minutes,
                    Seconds = x.Seconds,
                    Repetitions = x.Repetitions
                }).ToList(),
                WorkoutType = entity.WorkoutType,
            };
        
    }
}
