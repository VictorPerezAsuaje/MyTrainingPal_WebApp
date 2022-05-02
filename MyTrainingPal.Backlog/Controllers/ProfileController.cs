using Microsoft.AspNetCore.Mvc;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Infrastructure.Repositories;
using MyTrainingPal.Service.DTO.User;
using MyTrainingPal.Service.Services;

namespace MyTrainingPal.Backlog.Controllers
{
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
            Result<User> resultUser = _userRepository.GetById(1);

            if (resultUser.IsFailure)
            {
                ViewData["ErrorMessage"] = resultUser.Error;
                return View();
            }

            UserGetDTO resultMap = _userMapper.EntityToGetDTO(resultUser.Value);

            return View(resultMap);
        }

    }
}