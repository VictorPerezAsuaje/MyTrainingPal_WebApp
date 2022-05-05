using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Infrastructure.Repositories;
using MyTrainingPal.Service.DTO.User;
using MyTrainingPal.Service.Services;
using System.Security.Claims;

namespace MyTrainingPal.Backlog.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private IUserRepository _userRepository;
        private IUserMapper _userMapper;
        public ProfileController(IUserRepository userRepository, IUserMapper userMapper)
        {
            _userRepository = userRepository;
            _userMapper = userMapper;
        }

        public IActionResult Index()
        {
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

    }
}