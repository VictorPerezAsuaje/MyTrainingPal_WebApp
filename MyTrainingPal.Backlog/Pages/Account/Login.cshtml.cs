using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MyTrainingPal.Backlog.Pages.Account
{
    public class LoginModel : PageModel
    {
        private IUserRepository _userRepository;

        public LoginModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public LoginForm LoginForm { get; set; } = new LoginForm();
        public IActionResult OnGet()
        {
            if (User.Identity == null)
                return Page();

            if (!User.Identity.IsAuthenticated)
                return Page();

            TempData["Error"] = "You have already logged in.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid) return Page();

            Result<User> result = _userRepository.FindUserByCredentials(LoginForm.Email, LoginForm.Password);

            if (result.IsFailure)
            {
                TempData["LoginError"] = result.Error;
                return Page();
            }

            User user = result.Value;

            try
            {
                // Security context for Identity
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, "AuthCookie");

                // Container for the security context
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                // Encryption and serialization in a cookie for simplicity
                await HttpContext.SignInAsync("AuthCookie", principal);

            }
            catch (Exception ex)
            {
                TempData["LoginError"] = result.Error;
                return Page();
            }


            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index");
        }
    }

    public class LoginForm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public bool ImHuman { get; set; }
    }
}
