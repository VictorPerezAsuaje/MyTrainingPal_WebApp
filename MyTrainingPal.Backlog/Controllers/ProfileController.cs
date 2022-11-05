using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTrainingPal.Backlog.Models;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Infrastructure.Repositories;
using MyTrainingPal.Service.DTO.User;
using MyTrainingPal.Service.DTO.Workouts;
using MyTrainingPal.Service.Services;
using System.Security.Claims;

namespace MyTrainingPal.Backlog.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private IUserRepository _userRepository;
    private IUserMapper _userMapper;
    private int _pageLength = 2;
    private int _page = 0;

    public ProfileController(IUserRepository userRepository, IUserMapper userMapper)
    {
        _userRepository = userRepository;
        _userMapper = userMapper;
    }

    public IActionResult Index()
    {
        ViewData["Page"] = _page;
        ViewData["PageLength"] = _pageLength;

        string userId = Convert.ToString(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

        if (string.IsNullOrEmpty(userId))
        {
            TempData["Error"] = "There was an error finding the user logged. Please try to log in once again.";
            return Redirect("/");
        }

        Result<User> resultUser = _userRepository.GetById(Convert.ToInt32(userId));

        if (resultUser.IsFailure)
        {
            TempData["Error"] = resultUser.Error;
            return View();
        }

        UserGetDTO resultMap = _userMapper.EntityToGetDTO(resultUser.Value);

        return View(resultMap);
    }

    [Route("{page:int}")]
    public IActionResult ChangeHistoryWorkoutPage(int page)
    {
        ViewData["PageLength"] = _pageLength;
        ViewData["Page"] = page;

        string userId = Convert.ToString(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        Result<User> resultUser = _userRepository.GetById(Convert.ToInt32(userId));

        if (resultUser.IsFailure)
        {
            TempData["Error"] = resultUser.Error;
            return PartialView("_WorkoutHistoryTable", new List<WorkoutGetDTO>());
        }

        UserGetDTO resultMap = _userMapper.EntityToGetDTO(resultUser.Value);

        List<WorkoutHistory> filteredList = resultMap.CompletedWorkouts.Skip(page*_pageLength).Take(_pageLength).ToList();

        TempData["HistoryList"] = filteredList;

        return PartialView("_WorkoutHistoryTable", resultMap.CompletedWorkouts);
    }

    public IActionResult LoadUserEditableData()
    {
        string userId = Convert.ToString(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        Result<User> resultUser = _userRepository.GetById(Convert.ToInt32(userId));

        UserEditForm userEditForm = new UserEditForm();

        if (resultUser.IsFailure)
        {
            TempData["Error"] = resultUser.Error;
            return PartialView("_WorkoutHistoryTable", new List<WorkoutGetDTO>());
        }

        userEditForm.FormFirstName = resultUser.Value.Name.Trim();
        userEditForm.FormLastName = resultUser.Value.LastName.Trim();
        userEditForm.FormEmail = resultUser.Value.Email.Trim();

        return PartialView("_UserProfileEditModal", userEditForm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AcceptEditUser([FromForm]UserEditForm userEditForm)
    {
        if (!ModelState.IsValid)
            return View(userEditForm);

        string userId = Convert.ToString(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        Result<User> resultUser = _userRepository.GetById(Convert.ToInt32(userId));

        if (resultUser.IsFailure)
        {
            TempData["Error"] = resultUser.Error;
            return View();
        }

        User user = resultUser.Value;
        user.UpdateEditableFields(
            firstName: userEditForm.FormFirstName, 
            lastName: userEditForm.FormLastName, 
            email: userEditForm.FormEmail);

        try
        {
            Result updateResult = _userRepository.Update(user);
            if (updateResult.IsFailure)
            {
                TempData["Error"] = updateResult.Error;
                return View();
            }
        }
        catch (Exception ex)
        {

        }

        TempData["Success"] = "The user has been successfully updated.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult MakeMePremium()
    {
        string userId = Convert.ToString(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        Result<User> resultUser = _userRepository.GetById(Convert.ToInt32(userId));

        if (resultUser.IsFailure)
        {
            TempData["Error"] = resultUser.Error;
            return View();
        }

        User user = resultUser.Value;
        user.MakeMePremium();

        try
        {
            Result updateResult = _userRepository.Update(user);
            if (updateResult.IsFailure)
            {
                TempData["Error"] = updateResult.Error;
                return View();
            }
        }
        catch (Exception ex)
        {

        }

        TempData["Success"] = "The user has been successfully updated.";
        return RedirectToAction("Index");
    }
}