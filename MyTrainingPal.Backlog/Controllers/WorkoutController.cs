using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTrainingPal.Backlog.Models;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using MyTrainingPal.Infrastructure.DTO.Workout;
using MyTrainingPal.Infrastructure.Repositories;
using MyTrainingPal.Service.DTO.Exercise;
using MyTrainingPal.Service.DTO.Workouts;
using MyTrainingPal.Service.Services;
using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace MyTrainingPal.Backlog.Controllers;

public class IndexViewModel
{
    public List<WorkoutGetDTO> Workouts { get; set; } = new List<WorkoutGetDTO>();
    public User? CurrentUser { get; set; }
    public WorkoutPutDTO WorkoutPutDTO { get; set; } = new WorkoutPutDTO();
}

public class WorkoutController : Controller
{
    private readonly IWorkoutRepository _workoutRepo;
    private readonly IWorkoutMapper _workoutMapper;
    private readonly IExerciseRepository _exerciseRepo;
    private readonly IExerciseMapper _exerciseMapper;
    private IUserRepository _userRepository;

    public WorkoutController(IWorkoutRepository workoutRepo, IWorkoutMapper workoutMapper, IUserRepository userRepository, IExerciseRepository exerciseRepository, IExerciseMapper exerciseMapper)
    {
        _workoutRepo = workoutRepo;
        _workoutMapper = workoutMapper;
        _userRepository = userRepository;
        _exerciseRepo = exerciseRepository;
        _exerciseMapper = exerciseMapper;
    }

    public IActionResult Index()
    {
        IndexViewModel indexVM = new IndexViewModel();

        string? userId = Convert.ToString(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        User? currentUser = null;
        if (!string.IsNullOrEmpty(userId))
        {
            Result<User> resultUser = _userRepository.GetById(Convert.ToInt32(userId));

            if (resultUser.IsFailure)
            {
                TempData["Error"] = resultUser.Error;
                return View();
            }

            currentUser = resultUser.Value;
        }

        Result<List<Workout>> result = _workoutRepo.GetAll();

        if (result.IsFailure)
        {
            TempData["Error"] = result.Error;
            return View();
        }

        if (result.Value.Count == 0)
            return View(new List<WorkoutGetDTO>());

        Result<List<Exercise>> resultExercise = _exerciseRepo.GetAll();

        if (resultExercise.IsFailure)
            return BadRequest(result.Error);

        List<ExerciseGetDTO> exerciseDTOs = _exerciseMapper.EntityListToGetDTOList(resultExercise.Value);

        indexVM.Workouts = _workoutMapper.EntityListToGetDTOList(result.Value);
        indexVM.CurrentUser = currentUser;
        indexVM.WorkoutPutDTO.AvailableExercisesJSON = JsonSerializer.Serialize(exerciseDTOs);

        return View(indexVM);
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
    [HttpPost]
    [Route("Workout/CreateOrEdit")]
    public IActionResult CreateOrEdit([FromBody]WorkoutPutDTO workoutPutDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest("Required fields missing");

        if(workoutPutDTO.SetPostDTOs.Count == 0)
            return BadRequest("The sets can not be empty");

        string? userId = Convert.ToString(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        if(userId == null) 
            return RedirectToAction("Index");

        Result<Workout> resultMap = null;

        if (workoutPutDTO.Id != null)
            resultMap = Workout.Generate
            (
                name: workoutPutDTO.Name,
                workoutType: workoutPutDTO.WorkoutType,
                numberOfSets: workoutPutDTO.NumberOfSets,
                userId: Convert.ToInt32(userId),
                id: workoutPutDTO.Id
            );
        else
            resultMap = Workout.Generate
            (
                name: workoutPutDTO.Name,
                workoutType: workoutPutDTO.WorkoutType,
                numberOfSets: workoutPutDTO.NumberOfSets,
                userId: Convert.ToInt32(userId)
            );

        if (resultMap.IsFailure)
            return BadRequest(resultMap.Error);

        List<Set> sets = new List<Set>();
        foreach (var setPostDTO in workoutPutDTO.SetPostDTOs)
        {
            Exercise exercise = _exerciseRepo.GetById(setPostDTO.ExerciseId).Value;
            Result<Set> setResult = null;

            SetType setType = (SetType)Enum.Parse(typeof(SetType), workoutPutDTO.SelectedSetType);

            if (setType == SetType.ByTime)
            {
                setResult = Set.Generate
                (
                    exercise: exercise,
                    setType: setType,
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
                    setType: setType,
                    repetitions: setPostDTO.Repetitions
                );
            }

            if (setResult.IsFailure)
                return BadRequest(setResult.Error);

            sets.Add(setResult.Value);
        }

        Workout workout = resultMap.Value;
        workout.WithSets(sets, workout.NumberOfSets);

        Result result = null;
        if (workoutPutDTO.Id != null)
            result = _workoutRepo.Update(workout);                 
        else
            result = _workoutRepo.Add(workout);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }

    [Authorize]
    public IActionResult Delete(int workoutId)
    {
        Result result = _workoutRepo.Delete(workoutId);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Error;
            return RedirectToAction("Index");
        }

        return Redirect("/");
    }

    [Authorize]
    public IActionResult EditWorkout(int workoutId)
    {
        string? userId = Convert.ToString(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        Result<Workout> result = _workoutRepo.GetById(workoutId);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Error;
            return RedirectToAction("Index");
        }

        WorkoutPutDTO workoutDTO = _workoutMapper.EntityToPutDTO(result.Value);

        Result<List<Exercise>> resultExercise = _exerciseRepo.GetAll();

        if (resultExercise.IsFailure)
            return BadRequest(result.Error);

        List<ExerciseGetDTO> exerciseDTOs = _exerciseMapper.EntityListToGetDTOList(resultExercise.Value);

        workoutDTO.AvailableExercisesJSON = JsonSerializer.Serialize(exerciseDTOs); 

        return PartialView("_CreateOrEditWorkoutPartial", workoutDTO);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}