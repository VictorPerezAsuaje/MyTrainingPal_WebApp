using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using MyTrainingPal.Domain.Interfaces;
using System.Data.SqlClient;

namespace MyTrainingPal.Infrastructure.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
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
                    cmd.CommandText = "SELECT Users.Id AS UserId, Users.Name AS UserName, LastName, Email, Password, IsPremium, RegistrationDate, IsAdmin, Workout.Id AS WorkoutId, Workout.Name AS WorkoutName, WorkoutType FROM Users JOIN Workout ON Workout.UserId = Users.Id WHERE UserId = @UserId";
                    cmd.Parameters.AddWithValue("@UserId", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Workout> workouts = new List<Workout>();

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
                                return Result.Fail<User>(userResult.Error);

                            user = userResult.Value;
                        }

                        // Workout generation

                        Result<Workout> workoutResult = Workout.Generate
                        (
                            id: (int)reader["WorkoutId"],
                            name: (string)reader["WorkoutName"],
                            workoutType: (WorkoutType)reader["WorkoutType"]
                        );

                        if (workoutResult.IsFailure)
                            return Result.Fail<User>(workoutResult.Error);

                        workouts.Add(workoutResult.Value);
                    }

                    user.WithCreatedWorkouts(workouts);

                    SqlCommand cmdHistory = new SqlCommand();
                    cmdHistory.Connection = con;
                    cmdHistory.CommandText = "SELECT Workout.UserId AS UserId, Workout.Id AS WorkoutId, Workout.Name AS WorkoutName, WorkoutType, CompletionDate FROM UserWorkoutHistory JOIN Workout ON Workout.Id = UserWorkoutHistory.WorkoutId WHERE Workout.UserId = @UserId";
                    cmdHistory.Parameters.AddWithValue("@UserId", id);

                    SqlDataReader historyReader = cmdHistory.ExecuteReader();
                    List<WorkoutHistory> completedWorkouts = new List<WorkoutHistory>();

                    while (historyReader.Read())
                    {
                        Result<Workout> workoutResult = Workout.Generate
                        (
                            id: (int)historyReader["WorkoutId"],
                            name: (string)historyReader["WorkoutName"],
                            workoutType: (WorkoutType)historyReader["WorkoutType"]
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
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Result Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
