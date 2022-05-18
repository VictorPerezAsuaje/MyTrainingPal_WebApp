using MyTrainingPal.Domain.Common;
using System.Net.Mail;

namespace MyTrainingPal.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public string FullName => $"{Name} {LastName}";
        public string Email { get; private set; }
        public string Password { get; private set; }
        public bool IsPremium { get; private set; } = false;
        public bool IsAdmin { get; private set; } = false;
        public DateTime _RegistrationDate { get; private set; }
        public string RegistrationDate { get => _RegistrationDate.ToShortDateString(); }

        public List<Workout> CreatedWorkouts { get; private set; } = new List<Workout>();

        public List<WorkoutHistory> CompletedWorkouts { get; private set; } = new List<WorkoutHistory>();

        User() { }

        // I will keep it simple for now, I will need to further encrypt and decrypt passwords.
        public bool ValidateSesion(string password)
            => Password == password;

        // Temporarily here, but I'm guessing this would fit better in a service that serves as a layer between the creation of an user and the encription of the pass.
        private static Result ValidatePassword(string password)
        {
            List<char> allowedChars = new List<char> { '@', '!', '¡', '#', '$', '*' };

            if (password.Length < 8 || password.Length > 20)
                return Result.Fail("Password length can not be lower than 8 characters nor more than 20 characters.");

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

        private static Result ValidateEmail(string email)
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

        public static Result<User> Generate(string name, string lastName, string email, string password, DateTime registrationDate, bool isPremium,
            /* OPCIONAL */
            int? id = null, bool? isAdmin = null)
        {
            User user = new User();

            // Validation

            if (string.IsNullOrEmpty(name))
                return Result.Fail<User>("The name can not be empty.");

            if (name.Length > 50)
                return Result.Fail<User>("The name can not have more than 50 characters.");

            if (string.IsNullOrEmpty(lastName))
                return Result.Fail<User>("The last name can not be empty.");

            if (lastName.Length > 150)
                return Result.Fail<User>("The last name can not have more than 150 characters.");

            if (string.IsNullOrEmpty(email))
                return Result.Fail<User>("The email can not be empty.");

            if (email.Length > 150)
                return Result.Fail<User>("The email can not have more than 150 characters.");

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

            if (isAdmin == null)
                user.IsAdmin = false;
            else
                user.IsAdmin = (bool)isAdmin;

            user.Name = name;
            user.LastName = lastName;
            user.Email = email;
            user.Password = password;
            user.IsPremium = isPremium;
            user._RegistrationDate = registrationDate;

            return Result.Ok(user);
        }

        public User WithCreatedWorkouts(List<Workout> workouts)
        {
            CreatedWorkouts = workouts;
            return this;
        }

        public User WithCompletedWorkouts(List<WorkoutHistory> workouts)
        {
            CompletedWorkouts = workouts;
            return this;
        }

        public User UpdateEditableFields(string? firstName = null, string? lastName = null, string? email = null)
        {
            if(!string.IsNullOrEmpty(firstName))
                Name = firstName.Trim();

            if(!string.IsNullOrEmpty(lastName))
                LastName = lastName.Trim();

            if(!string.IsNullOrEmpty(email))
                Email = email.Trim();

            return this;
        }
    }
}
