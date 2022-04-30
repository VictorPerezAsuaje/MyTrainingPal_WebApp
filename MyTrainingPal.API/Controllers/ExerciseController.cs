using Microsoft.AspNetCore.Mvc;
using MyTrainingPal.API.DTO.Exercise;
using MyTrainingPal.API.Interfaces;
using MyTrainingPal.API.Services;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Infrastructure.Repositories;

namespace MyTrainingPal.API.Controllers
{
    [ApiController]
    [Route("api/exercises")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseRepo;
        private readonly IExerciseMapper _exerciseMapper; 

        public ExerciseController(IExerciseRepository exerciseRepo, IExerciseMapper exerciseMapper)
        {
            _exerciseRepo = exerciseRepo;
            _exerciseMapper = exerciseMapper;
        }

        [HttpGet]
        public ActionResult<List<ExerciseGetDTO>> GetExercises(int? page = null, int? pageSize = null)
        {
            Result<List<Exercise>> result = _exerciseRepo.GetAll(page, pageSize);

            if(result.IsFailure)
                return BadRequest(result.Error);

            List<ExerciseGetDTO> dtos = _exerciseMapper.EntityListToGetDTOList(result.Value);

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<ExerciseGetDTO> GetExercise(int id)
        {
            if (id < 0) 
                return NotFound("There's no exercise with that id.");

            Result<Exercise> result = _exerciseRepo.GetById(id);

            if (result.IsFailure)
                return BadRequest(result.Error);

            ExerciseGetDTO dto = _exerciseMapper.EntityToGetDTO(result.Value);

            return Ok(dto);
        }

        [HttpPost]
        public ActionResult PostExercise(ExercisePostDTO exercisePostDTO)
        {
            if (exercisePostDTO == null) 
                return BadRequest();

            Result<Exercise> result = _exerciseMapper.PostDTOToEntity(exercisePostDTO);

            if (result.IsFailure)
                return BadRequest(result.Error);

            Result resultAdd = _exerciseRepo.Add(result.Value);

            if (resultAdd.IsFailure)
                return BadRequest(result.Error);

            return Ok(exercisePostDTO);
        }
    }
}