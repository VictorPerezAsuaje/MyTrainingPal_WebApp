using Microsoft.AspNetCore.Mvc;
using MyTrainingPal.Backlog.Models;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Infrastructure.DTO.Workout;
using MyTrainingPal.Infrastructure.Repositories;
using MyTrainingPal.Service.DTO.Workouts;
using MyTrainingPal.Service.Services;
using System.Diagnostics;

namespace MyTrainingPal.Backlog.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly IWorkoutRepository _workoutRepo;
        private readonly IWorkoutMapper _workoutMapper;

        public WorkoutController(IWorkoutRepository workoutRepo, IWorkoutMapper workoutMapper)
        {
            _workoutRepo = workoutRepo;
            _workoutMapper = workoutMapper;
        }

        public IActionResult Index()
        {
            Result<List<Workout>> result = _workoutRepo.GetAll();

            if (result.IsFailure)
            {
                ViewData["Error"] = result.Error;
                return View();
            }

            if (result.Value.Count == 0)
                return View(new List<WorkoutGetDTO>());

            List<WorkoutGetDTO> workoutDTO = _workoutMapper.EntityListToGetDTOList(result.Value);

            return View(workoutDTO);
        }

        public IActionResult FilterWorkouts(WorkoutFilterDTO filter)
        {
            Result<List<Workout>> result = _workoutRepo.FindMatch(filter);

            if (result.IsFailure)
            {
                ViewData["Error"] = result.Error;
                return PartialView("_WorkoutListPartial", new List<WorkoutGetDTO>());
            }

            if (result.Value.Count == 0)
                return PartialView("_WorkoutListPartial", new List<WorkoutGetDTO>());

            List<WorkoutGetDTO> workoutDTO = _workoutMapper.EntityListToGetDTOList(result.Value);

            return PartialView("_WorkoutListPartial", workoutDTO);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}