using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using MyTrainingPal.Domain.Interfaces;
using System.Data.SqlClient;

namespace MyTrainingPal.Infrastructure.Repositories
{
    public interface IWorkoutRepository : IRepository<Workout>
    {
    }
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=MyTrainingPalDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public Result<List<Workout>> GetAll(int? page = null, int? pageSize = null)
        {
            if(page < 0)
                return Result.Fail<List<Workout>>("The page can not be less than 0.");

            if (pageSize < 1)
                return Result.Fail<List<Workout>>("The page size can not be less than 1.");

            if ((page == null && pageSize != null) || (page != null && pageSize == null))
                return Result.Fail<List<Workout>>("If a page or a page size is requested, the other one must be indicated as well. Please make sure to request both the page and page size if you want pagination.");

            List<Workout> workouts = new List<Workout>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT Workout.Id AS WorkoutId, Workout.Name AS WorkoutName, WorkoutType, SetType, Repetitions, Time, Exercises.Id AS ExerciseId, Exercises.Name AS ExerciseName, Level, ForceType, RequiresEquipment FROM Workout JOIN Sets ON Sets.WorkoutId = Workout.Id JOIN Exercises ON Sets.ExerciseId = Exercises.Id";

                    cmd.Parameters.Clear();

                    if (page != null && pageSize != null)
                    {
                        cmd.CommandText += " OFFSET(@Skip) ROWS FETCH NEXT(@Take) ROWS ONLY ";
                        cmd.Parameters.AddWithValue("@Skip", page * pageSize);
                        cmd.Parameters.AddWithValue("@Take", pageSize);
                    }

                    SqlDataReader reader = cmd.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            // Workout generation

                            Result<Workout> workoutResult = Workout.Generate
                            (
                                id: (int)reader["WorkoutId"],
                                name: (string)reader["WorkoutName"],
                                workoutType: (WorkoutType)reader["WorkoutType"]
                            );

                            if (workoutResult.IsFailure)
                                return Result.Fail<List<Workout>>(workoutResult.Error);

                            Workout workout = workoutResult.Value;

                            // Exercise generation

                            Result<Exercise> exerciseResult = Exercise.Generate(
                                id: (int)reader["ExerciseId"],
                                name: (string)reader["ExerciseName"],
                                muscleGroups: new List<MuscleGroup>(),
                                level: (DifficultyLevel)reader["Level"],
                                forceType: (ForceType)reader["ForceType"], 
                                hasEquipment: (bool)reader["RequiresEquipment"]);

                            if (exerciseResult.IsFailure)
                                return Result.Fail<List<Workout>>(exerciseResult.Error);

                            Exercise exercise = exerciseResult.Value;

                            // Set generation

                            int? seconds = null;
                            int? minutes = null;
                            int? hours = null;
                            int? reps = null;

                            if (reader["Time"] != DBNull.Value)
                            {
                                TimeOnly time = (TimeOnly)reader["Time"];
                                seconds = time.Second;
                                minutes = time.Minute;
                                hours = time.Hour;
                            }

                            if (reader["Repetitions"] != DBNull.Value)
                                reps = (int)reader["Repetitions"];
                            

                            Result<Set> resultSet = Set.Generate
                            (
                                exercise: exercise,
                                setType: (SetType)reader["SetType"],
                                seconds: seconds,
                                minutes: minutes,
                                hours: hours,
                                repetitions: reps
                            );

                            if (resultSet.IsFailure)
                                return Result.Fail<List<Workout>>(resultSet.Error);

                            Set set = resultSet.Value;

                            if (workouts.Any(w => w.Id == workout.Id))
                            {
                                workouts.Where(e => e.Id == workout.Id)
                                .FirstOrDefault()?.Sets.Add(set);
                            }
                            else
                            {
                                workout.Sets.Add(set);
                                workouts.Add(workout);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Result.Fail<List<Workout>>("The list of workouts could not be retrieved.");
            }

            return Result.Ok(workouts);
        }

        public Result<Workout> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Result Add(Workout entity)
        {
            bool isOkey = false;
            SqlTransaction transaction = null;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    transaction = con.BeginTransaction();
                    SqlCommand cmdWorkout = new SqlCommand();
                    cmdWorkout.Connection = con;
                    cmdWorkout.Transaction = transaction;
                    cmdWorkout.CommandText = "INSERT INTO Workout (Name, WorkoutType) OUTPUT INSERTED.Id VALUES (@Name, @WorkoutType)";

                    cmdWorkout.Parameters.AddWithValue("@Name", entity.Name);
                    cmdWorkout.Parameters.AddWithValue("@WorkoutType", entity.WorkoutType);

                    int insertedWorkoutId = Convert.ToInt32(cmdWorkout.ExecuteScalar());

                    SqlCommand cmdSets = new SqlCommand();
                    cmdSets.Connection = con;
                    cmdSets.Transaction = transaction;

                    foreach (Set set in entity.Sets)
                    {
                        cmdSets.CommandText = "INSERT INTO Sets (WorkoutId, ExerciseId, SetType, ";
                        cmdSets.Parameters.Clear();
                        cmdSets.Parameters.AddWithValue("@WorkoutId", insertedWorkoutId);
                        cmdSets.Parameters.AddWithValue("@ExerciseId", set.Exercise.Id);
                        cmdSets.Parameters.AddWithValue("@SetType", set.SetType);

                        if (set.SetType == SetType.ByRepetition)
                        {
                            cmdSets.CommandText += " Repetitions) OUTPUT INSERTED.Id VALUES(@WorkoutId, @ExerciseId, @SetType, @Repetitions)";
                            cmdSets.Parameters.AddWithValue("@Repetitions", set.Repetitions);
                        }
                        else
                        {
                            cmdSets.CommandText += " Time) OUTPUT INSERTED.Id VALUES(@WorkoutId, @ExerciseId, @SetType, @Time)";
                            TimeOnly timeOnly = new TimeOnly(hour: set.Hours, minute: set.Minutes, second: set.Seconds);
                            cmdSets.Parameters.AddWithValue("@Time", timeOnly);
                        }

                        cmdSets.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    isOkey = true;
                }
            }
            catch (Exception ex)
            {
                isOkey = false;
                transaction.Rollback();
            }

            return isOkey ? Result.Ok() : Result.Fail("The workout could not be added.");
        }

        public Result Delete(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Workout WHERE Id = @WorkoutId", con);
                    cmd.Parameters.AddWithValue("@WorkoutId", id);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                }                
            }
            catch (Exception ex)
            {
                return Result.Fail("There was an error deleting the workout.");
            }

            return Result.Ok();
        }

        public Result Update(Workout entity)
        {
            throw new NotImplementedException();
        }
    }
}
