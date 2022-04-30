using Microsoft.AspNetCore.Mvc;
using MyTrainingPal.API.DTO.Exercise;
using MyTrainingPal.API.DTO.Workout;
using MyTrainingPal.API.Interfaces;
using MyTrainingPal.API.Services;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Infrastructure.Repositories;

namespace MyTrainingPal.API.Controllers
{
    [ApiController]
    [Route("api/workouts")]
    public class WorkoutController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseRepo;
        private readonly IExerciseMapper _exerciseMapper;
        private readonly IWorkoutRepository _workoutRepo;
        private readonly IWorkoutMapper _workoutMapper;

        public WorkoutController(IExerciseRepository exerciseRepo, IExerciseMapper exerciseMapper, IWorkoutRepository workoutRepo, IWorkoutMapper workoutMapper)
        {
            _exerciseRepo = exerciseRepo;
            _exerciseMapper = exerciseMapper;
            _workoutRepo = workoutRepo;
            _workoutMapper = workoutMapper;
        }

        [HttpGet]
        public ActionResult<List<Workout>> GetWorkouts(int? page = null, int? pageSize = null)
        {
            Result<List<Workout>> result = _workoutRepo.GetAll(page, pageSize);

            if(result.IsFailure)
                return BadRequest(result.Error);

            //List<ExerciseGetDTO> dtos = _exerciseMapper.EntityListToGetDTOList(result.Value);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public ActionResult<Workout> GetWorkout(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteWorkout(int id)
        {
            if (id < 1)
                return NotFound();

            Result result = _workoutRepo.Delete(id);

            if (result.IsFailure)
                BadRequest(result.Error);

            return Ok();
        }

        [HttpPost]
        public ActionResult PostWorkout(WorkoutPostDTO workoutPostDTO)
        {
            if (workoutPostDTO == null)
                return BadRequest();

            Result<Workout> resultMap = _workoutMapper.PostDTOToEntity(workoutPostDTO);

            List<Set> sets = new List<Set>();            
            foreach(SetPostDTO setPostDTO in workoutPostDTO.SetPostDTOs)
            {
                Exercise exercise = _exerciseRepo.GetById(setPostDTO.ExerciseId).Value;

                Result<Set> result = Set.Generate
                (
                    exercise: exercise,
                    setType: setPostDTO.SetType,
                    seconds: setPostDTO.Seconds,
                    minutes: setPostDTO.Minutes,
                    hours: setPostDTO.Hours, 
                    repetitions: setPostDTO.Repetitions
                );

                if (result.IsFailure)
                    return BadRequest(result.Error);

                sets.Add(result.Value);
            }

            if (resultMap.IsFailure)
                return BadRequest(resultMap.Error);

            Workout workout = resultMap.Value;
            workout.WithSets(sets);

            Result resultAdd = _workoutRepo.Add(workout);

            if (resultAdd.IsFailure)
                return BadRequest(resultAdd.Error);

            return Ok();

        }
    }
}