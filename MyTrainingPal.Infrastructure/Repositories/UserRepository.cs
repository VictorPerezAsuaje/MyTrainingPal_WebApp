using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using MyTrainingPal.Domain.Interfaces;
using System.Data.SqlClient;

namespace MyTrainingPal.Infrastructure.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Result<User> FindUserByCredentials(string email, string password);
        Result<User?> FindUserByEmail(string email);
        Result AddWorkoutToHistory(int workoutId, int userId);
    }
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=MyTrainingPalDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public Result<List<User>> GetAll()
        {
            List<User> exercises = new List<User>();

            try
            {
                using SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "";

                SqlDataReader reader = cmd.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        
                    }
                }

                con.Close();
            }
            catch (Exception ex)
            {
                Result.Fail<List<User>>("The list of exercises could not be retrieved.");
            }

            return Result.Ok(exercises);
        }

        public Result<User> GetById(int id)
        {
            User user = null;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT Id, Name AS UserName, LastName, Email, Password, IsPremium, RegistrationDate, IsAdmin FROM Users WHERE Id = @UserId";
                    cmd.Parameters.AddWithValue("@UserId", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Result<User> userResult = User.Generate
                        (
                            id: (int)reader["Id"],
                            name: (string)reader["UserName"],
                            lastName: (string)reader["LastName"],
                            password: (string)reader["Password"],
                            email: (string)reader["Email"],
                            isAdmin: (bool)reader["IsAdmin"],
                            isPremium: (bool)reader["IsPremium"],
                            registrationDate: (DateTime)reader["RegistrationDate"]
                        );

                        if (userResult.IsFailure)
                            return Result.Fail<User>(userResult.Error);

                        user = userResult.Value;
                    }

                    if (user == null)
                        return Result.Fail<User>("The user could not be retrieved");

                    SqlCommand cmdWorkouts = new SqlCommand();
                    cmdWorkouts.Connection = con;
                    cmdWorkouts.CommandText = "SELECT Id AS WorkoutId, Name AS WorkoutName, WorkoutType, NumberOfSets FROM Workout WHERE UserId = @UserId";
                    cmdWorkouts.Parameters.AddWithValue("@UserId", id);

                    SqlDataReader readerWorkouts = cmdWorkouts.ExecuteReader();

                    List<Workout> workouts = new List<Workout>();

                    while (readerWorkouts.Read())
                    {
                        Result<Workout> workoutResult = Workout.Generate
                        (
                            id: (int)readerWorkouts["WorkoutId"],
                            name: (string)readerWorkouts["WorkoutName"],
                            workoutType: (WorkoutType)readerWorkouts["WorkoutType"],
                            numberOfSets: (int)readerWorkouts["NumberOfSets"]
                        );

                        if (workoutResult.IsFailure)
                            return Result.Fail<User>(workoutResult.Error);

                        workouts.Add(workoutResult.Value);
                    }

                    user.WithCreatedWorkouts(workouts);

                    SqlCommand cmdHistory = new SqlCommand();
                    cmdHistory.Connection = con;
                    cmdHistory.CommandText = "SELECT Workout.UserId AS UserId, Workout.Id AS WorkoutId, Workout.Name AS WorkoutName, WorkoutType, CompletionDate, NumberOfSets FROM UserWorkoutHistory JOIN Workout ON Workout.Id = UserWorkoutHistory.WorkoutId WHERE Workout.UserId = @UserId";
                    cmdHistory.Parameters.AddWithValue("@UserId", id);

                    SqlDataReader historyReader = cmdHistory.ExecuteReader();
                    List<WorkoutHistory> completedWorkouts = new List<WorkoutHistory>();

                    while (historyReader.Read())
                    {
                        Result<Workout> workoutResult = Workout.Generate
                        (
                            id: (int)historyReader["WorkoutId"],
                            name: (string)historyReader["WorkoutName"],
                            workoutType: (WorkoutType)historyReader["WorkoutType"],
                            numberOfSets: (int)historyReader["NumberOfSets"]
                        );

                        if (workoutResult.IsFailure)
                            return Result.Fail<User>(workoutResult.Error);

                        completedWorkouts.Add(new WorkoutHistory(workoutResult.Value, (DateTime)historyReader["CompletionDate"]));
                    }

                    user.WithCompletedWorkouts(completedWorkouts);
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<User>("The user could not be retrieved.");
            }

            return Result.Ok(user);
        }

        public Result Add(User entity)
        {
            Result result = Result.Fail("User registration has failed.");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "INSERT INTO Users(Name, LastName, Email, Password, IsPremium, RegistrationDate, IsAdmin) VALUES (@Name, @LastName, @Email, @Password, @IsPremium, @RegistrationDate, @IsAdmin)";
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@LastName", entity.LastName);
                cmd.Parameters.AddWithValue("@Email", entity.Email);
                cmd.Parameters.AddWithValue("@Password", entity.Password);
                cmd.Parameters.AddWithValue("@IsPremium", entity.IsPremium);
                cmd.Parameters.AddWithValue("@RegistrationDate", entity._RegistrationDate);
                cmd.Parameters.AddWithValue("@IsAdmin", false);

                try
                {
                    result = cmd.ExecuteNonQuery() > 0 ? Result.Ok() : result;
                }
                catch (Exception ex)
                {

                }
            }

            return result;
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Result Update(User entity)
        {
            Result result = Result.Fail("User update has failed.");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "UPDATE Users SET Name = @Name, LastName = @LastName, Email = @Email WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@LastName", entity.LastName);
                cmd.Parameters.AddWithValue("@Email", entity.Email);

                try
                {
                    result = cmd.ExecuteNonQuery() > 0 ? Result.Ok() : result;
                }
                catch (Exception ex)
                {

                }
            }

            return result;
        }

        public Result AddWorkoutToHistory(int workoutId, int userId)
        {
            Result result = Result.Fail("Workout could not be saved for this user.");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "INSERT INTO UserWorkoutHistory (WorkoutId, UserId, CompletionDate) VALUES(@WorkoutId, @UserId, @CompletionDate)";
                cmd.Parameters.AddWithValue("@WorkoutId", workoutId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@CompletionDate", DateTime.Now);

                try
                {
                    result = cmd.ExecuteNonQuery() > 0 ? Result.Ok() : result;
                }
                catch (Exception ex)
                {

                }
            }

            return result;
        }

        public Result<User> FindUserByCredentials(string email, string password)
        {
            User user = null;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT Id AS UserId, Name AS UserName, LastName, Email, Password, IsPremium, RegistrationDate, IsAdmin FROM Users WHERE Email = @UserEmail AND Password = @UserPassword";
                    cmd.Parameters.AddWithValue("@UserEmail", email);
                    cmd.Parameters.AddWithValue("@UserPassword", password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (user == null)
                        {
                            Result<User> userResult = User.Generate
                            (
                                id: (int)reader["UserId"],
                                name: (string)reader["UserName"],
                                lastName: (string)reader["LastName"],
                                password: (string)reader["Password"],
                                email: (string)reader["Email"],
                                isAdmin: (bool)reader["IsAdmin"],
                                isPremium: (bool)reader["IsPremium"],
                                registrationDate: (DateTime)reader["RegistrationDate"]
                            );

                            if (userResult.IsFailure)
                                return Result.Fail<User>("There was an error retrieving user data.");

                            user = userResult.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<User>("There was an error retrieving user data.");
            }

            return user != null ? Result.Ok(user) : Result.Fail<User>("Incorrect credentials.");
        }

        public Result<User?> FindUserByEmail(string email)
        {
            User user = null;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT Id AS UserId, Name AS UserName, LastName, Email, Password, IsPremium, RegistrationDate, IsAdmin FROM Users WHERE Email = @UserEmail";
                    cmd.Parameters.AddWithValue("@UserEmail", email);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (user == null)
                        {
                            Result<User> userResult = User.Generate
                            (
                                id: (int)reader["UserId"],
                                name: (string)reader["UserName"],
                                lastName: (string)reader["LastName"],
                                password: (string)reader["Password"],
                                email: (string)reader["Email"],
                                isAdmin: (bool)reader["IsAdmin"],
                                isPremium: (bool)reader["IsPremium"],
                                registrationDate: (DateTime)reader["RegistrationDate"]
                            );

                            if (userResult.IsFailure)
                                return Result.Fail<User>("There was an error retrieving user data.");

                            user = userResult.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<User>("There was an error retrieving user data.");
            }

            return Result.Ok(user);
        }
    }
}
