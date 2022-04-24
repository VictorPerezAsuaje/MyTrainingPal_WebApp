using Microsoft.AspNetCore.Mvc;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Infrastructure.Interfaces;

namespace MyTrainingPal.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseRepo;

        public ExerciseController(IExerciseRepository exerciseRepo)
        {
            _exerciseRepo = exerciseRepo;
        }

        [HttpGet]
        public List<Exercise> GetExercises()
        {
            return (List<Exercise>)_exerciseRepo.GetAll();
        }
    }
}