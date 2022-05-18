using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MyTrainingPal.Backlog.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private IUserRepository _userRepository;

        public RegisterModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public RegistrationForm RegistrationForm { get; set; } = new RegistrationForm();
        public IActionResult OnGet()
        {
            if (User.Identity == null)
                return Page();

            if (!User.Identity.IsAuthenticated)
                return Page();

            TempData["Error"] = "You have already logged in.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            Result<User?> result = _userRepository.FindUserByEmail(RegistrationForm.Email);

            if (result.IsFailure)
            {
                TempData["Error"] = "There was an error whilst trying to register your account. Please try again later.";
                return Page();
            }

            if(result.Value != null)
            {
                TempData["RegistrationError"] = "There's already another user registered with the data you provided, please try using another email address.";
                return Page();
            }

            Result<User> newUser = Domain.Entities.User.Generate
            (
                name: RegistrationForm.FirstName,
                lastName: RegistrationForm.LastName,
                password: RegistrationForm.Password,
                email: RegistrationForm.Email,
                isAdmin: false,
                isPremium: false,
                registrationDate: DateTime.Now
            );

            if(newUser.IsFailure)
            {
                TempData["RegistrationError"] = newUser.Error;
                return Page();
            }    

            try
            {
                Result newUserResult = _userRepository.Add(newUser.Value);

                if (newUserResult.IsFailure)
                {
                    TempData["RegistrationError"] = newUserResult.Error;
                    return Page();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There was an error whilst trying to register your account. Please try again later.";
                return Page();
            }

            TempData["Success"] = $"User {newUser.Value.FullName} registered successfully.";
            return RedirectToPage("/Account/Login");
        }
    }

    public class RegistrationForm
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public bool HasAcceptedConditions { get; set; }
    }
}
