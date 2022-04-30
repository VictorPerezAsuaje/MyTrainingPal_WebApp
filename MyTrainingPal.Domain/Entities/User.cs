using MyTrainingPal.Domain.Common;
using System.Net.Mail;

namespace MyTrainingPal.Domain.Entities
{
    public class User : BaseEntity
    {
        private string Name { get; set; }
        private string LastName { get; set; }
        public string FullName => $"{Name} {LastName}";
        public string Email { get; private set; }
        private string Password { get; set; }
        public bool IsPremium { get; private set; } = false;
        private DateTime _RegistrationDate { get; set; }
        public string RegistrationDate { get => _RegistrationDate.ToShortDateString(); }

        User() { }

        // I will keep it simple for now, I will need to further encrypt and decrypt passwords.
        public bool ValidateSesion(string password)
            => Password == password;

        // Temporarily here, but I'm guessing this would fit better in a service that serves as a layer between the creation of an user and the encription of the pass.
        private Result ValidatePassword(string password)
        {
            List<char> allowedChars = new List<char> { '@', '!', '¡', '#', '$', '*' };

            if (password.Length < 8)
                return Result.Fail("Password length can not be lower than 8 characters.");

            if (!password.Any(char.IsUpper))
                return Result.Fail($"The password should contain at least one uppercase.");

            if (!password.Any(char.IsLower))
                return Result.Fail($"The password should contain at least one lowercase.");

            if (!password.Any(char.IsNumber))
                return Result.Fail($"The password should contain at least one number.");

            if (!password.Any(c => allowedChars.Contains(c)))
                return Result.Fail($"The password should contain at least one of the following special characters: @, !, ¡, #, $, * .");            

            return Result.Ok();
        }

        private Result ValidateEmail(string email)
        {
            string trimmedMail = email.Trim();

            try
            {
                string validEmail = new MailAddress(email).ToString();
            }
            catch (Exception ex)
            {
                return Result.Fail("Invalid email");
            }

            return Result.Ok();
        }

        public Result<User> Generate(string name, string lastName, string email, string password, DateTime registrationDate, bool isPremium,
            /* OPCIONAL */
            int? id = null)
        {
            User user = new User();

            // Validation

            if (string.IsNullOrEmpty(name))
                return Result.Fail<User>("The name can not be empty.");

            if (string.IsNullOrEmpty(lastName))
                return Result.Fail<User>("The last name can not be empty.");

            if (string.IsNullOrEmpty(email))
                return Result.Fail<User>("The email can not be empty.");

            Result emailValidation = ValidateEmail(email);
            if (emailValidation.IsFailure)
                return Result.Fail<User>(emailValidation.Error);

            if (string.IsNullOrEmpty(password))
                return Result.Fail<User>("The password can not be empty.");

            Result passValidation = ValidatePassword(password);
            if (passValidation.IsFailure)
                return Result.Fail<User>(passValidation.Error);

            // Generation
            if(id != null)
                user.Id = (int)id;

            user.Name = name;
            user.LastName = lastName;
            user.Email = email;
            user.Password = password;
            user.IsPremium = isPremium;
            user._RegistrationDate = registrationDate;

            return Result.Ok(user);
        }
    }
}
