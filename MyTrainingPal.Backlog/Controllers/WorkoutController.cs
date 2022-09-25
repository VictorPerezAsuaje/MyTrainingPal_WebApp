using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTrainingPal.Backlog.Models;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Infrastructure.DTO.Workout;
using MyTrainingPal.Infrastructure.Repositories;
using MyTrainingPal.Service.DTO.Workouts;
using MyTrainingPal.Service.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace MyTrainingPal.Backlog.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly IWorkoutRepository _workoutRepo;
        private readonly IWorkoutMapper _workoutMapper;
        private IUserRepository _userRepository;

        public WorkoutController(IWorkoutRepository workoutRepo, IWorkoutMapper workoutMapper, IUserRepository userRepository)
        {
            _workoutRepo = workoutRepo;
            _workoutMapper = workoutMapper;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            Result<List<Workout>> result = _workoutRepo.GetAll();

            if (result.IsFailure)
            {
                TempData["Error"] = result.Error;
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
                TempData["Error"] = result.Error;
                return PartialView("_WorkoutListPartial", new List<WorkoutGetDTO>());
            }

            if (result.Value.Count == 0)
                return PartialView("_WorkoutListPartial", new List<WorkoutGetDTO>());

            List<WorkoutGetDTO> workoutDTO = _workoutMapper.EntityListToGetDTOList(result.Value);

            return PartialView("_WorkoutListPartial", workoutDTO);
        }

        public IActionResult WorkoutDetails(int workoutId)
        {
            ViewData["CurrentExercise"] = null;
            Result<Workout> result = _workoutRepo.GetById(workoutId);

            if (result.IsFailure)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction("Index");
            }

            WorkoutGetDTO workoutDTO = _workoutMapper.EntityToGetDTO(result.Value);

            return View(workoutDTO);
        }

        [Route("{Controller}/{Action}/{workoutId:int}")]
        public IActionResult StartWorkout(int workoutId)
        {
            Result<Workout> result = _workoutRepo.GetById(workoutId);

            if (result.IsFailure)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction("Index");
            }

            WorkoutGetDTO workoutDTO = _workoutMapper.EntityToGetDTO(result.Value);

            return View(workoutDTO);
        }

        [Route("{Controller}/{Action}/{workoutId:int}")]
        public IActionResult SaveWorkoutAttempt(int workoutId)
        {
            Result result = null;
            try
            {
                result = _userRepository.AddWorkoutToHistory(workoutId, Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
            }
            catch(Exception ex)
            {
                TempData["Error"] = "There was an error trying to save the workout";
                return RedirectToAction("Index");
            }
            
            if(result.IsFailure)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Congratulations! Your attempt has been saved in your account 😁";
            return RedirectToAction("Index");
        }


        [Authorize]
        public IActionResult CreateWorkout()
        {
            return View();
        }

        [Authorize]
        public IActionResult EditWorkout()
        {
            return View();
        }

        [Authorize]
        public IActionResult DeleteWorkout()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}