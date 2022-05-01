using Microsoft.AspNetCore.Mvc;
using MyTrainingPal.API.DTO.Workout;
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
        private readonly IWorkoutRepository _workoutRepo;
        private readonly IWorkoutMapper _workoutMapper;

        public WorkoutController(IWorkoutRepository workoutRepo, IWorkoutMapper workoutMapper)
        {
            _workoutRepo = workoutRepo;
            _workoutMapper = workoutMapper;
        }

        [HttpGet]
        public ActionResult<List<Workout>> GetWorkouts()
        {
            Result<List<Workout>> result = _workoutRepo.GetAll();

            if(result.IsFailure)
                return BadRequest(result.Error);

            if(result.Value.Count == 0)
                return new List<Workout>();

            List<WorkoutGetDTO> dtos = _workoutMapper.EntityListToGetDTOList(result.Value);

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<Workout> GetWorkout(int id)
        {
            Result<Workout> result = _workoutRepo.GetById(id);

            if (result.IsFailure)
                return BadRequest(result.Error);

            if (result.Value?.Id == null)
                return NotFound();

            WorkoutGetDTO dto = _workoutMapper.EntityToGetDTO(result.Value);

            return Ok(dto);
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

        [HttpPut("{id}")]
        public ActionResult UpdateWorkout(int id, WorkoutPutDTO workoutPutDTO)
        {
            if (id < 1)
                return NotFound();

            Result<Workout> result = _workoutRepo.GetById(id);

            if (result.IsFailure)
                BadRequest(result.Error);

            if (result.Value?.Id == null)
                return NotFound("The workout you wanted to update could not be found.");

            Result<Workout> workoutUpdated = _workoutMapper.PutDTOToEntity(result.Value, workoutPutDTO);

            if (workoutUpdated.IsFailure)
                BadRequest(workoutUpdated.Error);

            Result resultUpdate = _workoutRepo.Update(workoutUpdated.Value);

            if (resultUpdate.IsFailure)
                BadRequest(resultUpdate.Error);

            return Ok();
        }

        [HttpPost]
        public ActionResult AddWorkout(WorkoutPostDTO workoutPostDTO)
        {
            if (workoutPostDTO == null)
                return BadRequest();

            Result<Workout> resultMap = _workoutMapper.PostDTOToEntity(workoutPostDTO);

            if (resultMap.IsFailure)
                return BadRequest(resultMap.Error);

            Result resultAdd = _workoutRepo.Add(resultMap.Value);

            if (resultAdd.IsFailure)
                return BadRequest(resultAdd.Error);

            return Ok();
        }
    }
}