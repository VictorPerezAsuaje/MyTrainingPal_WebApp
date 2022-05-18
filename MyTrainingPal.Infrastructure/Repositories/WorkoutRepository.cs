using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using MyTrainingPal.Domain.Interfaces;
using MyTrainingPal.Infrastructure.DTO.Workout;
using System.Data;
using System.Data.SqlClient;

namespace MyTrainingPal.Infrastructure.Repositories
{
    public interface IWorkoutRepository : IRepository<Workout>
    {
        Result<List<Workout>> FindMatch(WorkoutFilterDTO? filter = null);
    }
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=MyTrainingPalDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        private Result<Set> FillWorkoutEntity(SqlDataReader reader, Workout workout)
        {
            // Exercise generation

            Result<Exercise> exerciseResult = Exercise.Generate(
                id: (int)reader["ExerciseId"],
                name: (string)reader["ExerciseName"],
                muscleGroups: new List<MuscleGroup>(),
                level: (DifficultyLevel)reader["Level"],
                forceType: (ForceType)reader["ForceType"],
                hasEquipment: (bool)reader["RequiresEquipment"]);

            if (exerciseResult.IsFailure)
                return Result.Fail<Set>(exerciseResult.Error);

            Exercise exercise = exerciseResult.Value;

            // Set generation

            int? seconds = null;
            int? minutes = null;
            int? hours = null;
            int? reps = null;

            if (reader["Time"] != DBNull.Value)
            {
                DateTime time = (DateTime)reader["Time"];
                seconds = time.Second;
                minutes = time.Minute;
                hours = time.Hour;
            }

            if (reader["Repetitions"] != DBNull.Value)
                reps = (int)reader["Repetitions"];


            Result<Set> resultSet = Set.Generate
            (
                id: (int)reader["SetId"],
                exercise: exercise,
                setType: (SetType)reader["SetType"],
                seconds: seconds,
                minutes: minutes,
                hours: hours,
                repetitions: reps
            );

            if (resultSet.IsFailure)
                return Result.Fail<Set>(resultSet.Error);

            return resultSet;
        }


        private string GenerateFilter(SqlCommand cmd, WorkoutFilterDTO filter)
        {
            string sqlFilter = "";

            if (filter.Level != null)
            {
                sqlFilter += " AND Level = @Level";
                cmd.Parameters.AddWithValue("@Level", filter.Level);
            }

            if (filter.SetType != null)
            {
                sqlFilter += " AND SetType = @SetType";
                cmd.Parameters.AddWithValue("@SetType", filter.SetType);
            }

            if (filter.WorkoutType != null)
            {
                sqlFilter += " AND WorkoutType = @WorkoutType";
                cmd.Parameters.AddWithValue("@WorkoutType", filter.WorkoutType);
            }

            if (filter.Level != null)
            {
                sqlFilter += " AND RequiresEquipment = @Equipment";
                cmd.Parameters.AddWithValue("@Equipment", filter.Equipment);
            }

            if (filter.UserId != null)
            {
                sqlFilter += " AND UserId = @UserId";
                cmd.Parameters.AddWithValue("@UserId", filter.UserId);
            }

            return sqlFilter;
        }

        public Result<List<Workout>> FindMatch(WorkoutFilterDTO? filter = null)
        {
            List<Workout> workouts = new List<Workout>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT Workout.Id AS WorkoutId, Workout.Name AS WorkoutName, WorkoutType, Sets.Id AS SetId, SetType, Repetitions, Time, Exercises.Id AS ExerciseId, Exercises.Name AS ExerciseName, Level, ForceType, RequiresEquipment FROM Workout JOIN Sets ON Sets.WorkoutId = Workout.Id JOIN Exercises ON Sets.ExerciseId = Exercises.Id WHERE 1=1 ";

                    if(filter != null)
                        cmd.CommandText += GenerateFilter(cmd, filter);

                    SqlDataReader reader = cmd.ExecuteReader();
                    Result<Workout> workoutResult = null;
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            // Workout generation

                            workoutResult = Workout.Generate
                            (
                                id: (int)reader["WorkoutId"],
                                name: (string)reader["WorkoutName"],
                                workoutType: (WorkoutType)reader["WorkoutType"],
                                numberOfSets: (int)reader["NumberOfSets"]
                            );

                            if (workoutResult.IsFailure)
                                return Result.Fail<List<Workout>>(workoutResult.Error);

                            Workout workout = workoutResult.Value;

                            Result<Set> resultSet = FillWorkoutEntity(reader, workout);

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
                return Result.Fail<List<Workout>>("Some error has ocurred and the list of workouts could not be retrieved.");
            }

            return Result.Ok(workouts);
        }

        public Result<List<Workout>> GetAll()
        {
            List<Workout> workouts = new List<Workout>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT Workout.Id AS WorkoutId, Workout.Name AS WorkoutName, WorkoutType, Sets.Id AS SetId, SetType, Repetitions, Time, Exercises.Id AS ExerciseId, Exercises.Name AS ExerciseName, Level, ForceType, RequiresEquipment, NumberOfSets FROM Workout JOIN Sets ON Sets.WorkoutId = Workout.Id JOIN Exercises ON Sets.ExerciseId = Exercises.Id";

                    SqlDataReader reader = cmd.ExecuteReader();
                    Result<Workout> workoutResult = null;
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            // Workout generation
                            
                            workoutResult = Workout.Generate
                            (
                                id: (int)reader["WorkoutId"],
                                name: (string)reader["WorkoutName"],
                                workoutType: (WorkoutType)reader["WorkoutType"],
                                numberOfSets: (int)reader["NumberOfSets"]
                            );

                            if (workoutResult.IsFailure)
                                return Result.Fail<List<Workout>>(workoutResult.Error);
                            

                            Workout workout = workoutResult.Value;

                            Result<Set> resultSet = FillWorkoutEntity(reader, workout);

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
                return Result.Fail<List<Workout>>("Some error has ocurred and the list of workouts could not be retrieved.");
            }

            return Result.Ok(workouts);
        }

        public Result<Workout> GetById(int id)
        {
            Workout workout = null;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT Workout.Id AS WorkoutId, Workout.Name AS WorkoutName, WorkoutType, Sets.Id AS SetId, SetType, Repetitions, Time, Exercises.Id AS ExerciseId, Exercises.Name AS ExerciseName, Level, ForceType, RequiresEquipment, NumberOfSets FROM Workout JOIN Sets ON Sets.WorkoutId = Workout.Id JOIN Exercises ON Sets.ExerciseId = Exercises.Id WHERE WorkoutId = @WorkoutId";
                    cmd.Parameters.AddWithValue("@WorkoutId", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    Result<Workout> workoutResult = null;

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            // Workout generation

                            if (workoutResult == null)
                            {
                                workoutResult = Workout.Generate
                                (
                                    id: (int)reader["WorkoutId"],
                                    name: (string)reader["WorkoutName"],
                                    workoutType: (WorkoutType)reader["WorkoutType"],
                                    numberOfSets: (int)reader["NumberOfSets"]
                                );

                                if (workoutResult.IsFailure)
                                    return Result.Fail<Workout>(workoutResult.Error);
                            }

                            workout = workoutResult.Value;

                            Result<Set> resultSet = FillWorkoutEntity(reader, workout);

                            if (resultSet.IsFailure)
                                return Result.Fail<Workout>(resultSet.Error);

                            workout.Sets.Add(resultSet.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<Workout>("There was an error and the workout could not be retrieved.");
            }

            return Result.Ok(workout);
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
                            DateTime time = Convert.ToDateTime($"{set.Hours}:{set.Minutes}:{set.Seconds}");
                            cmdSets.Parameters.AddWithValue("@Time", time);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                SqlCommand cmdWorkout = new SqlCommand("UPDATE Workout SET Name = @Name, WorkoutType = @WorkoutType WHERE Id = @WorkoutId", con);
                cmdWorkout.Transaction = transaction;
                cmdWorkout.Parameters.AddWithValue("@WorkoutId", entity.Id);
                cmdWorkout.Parameters.AddWithValue("@Name", entity.Name);
                cmdWorkout.Parameters.AddWithValue("@WorkoutType", entity.WorkoutType);

                try
                {
                    cmdWorkout.ExecuteNonQuery();

                    /* UPDATING SETS */

                    // Deleting previous sets

                    SqlCommand cmdDeleteSets = new SqlCommand();
                    cmdDeleteSets.Connection = con;
                    cmdDeleteSets.Transaction = transaction;
                    cmdDeleteSets.CommandText = "DELETE Sets WHERE WorkoutId = @WorkoutId";
                    cmdDeleteSets.Parameters.AddWithValue("@WorkoutId", entity.Id);
                    cmdDeleteSets.Transaction = transaction;
                    cmdDeleteSets.ExecuteNonQuery();

                    // Adding new sets

                    SqlCommand cmdSets = new SqlCommand();
                    cmdSets.Connection = con;
                    cmdSets.Transaction = transaction;

                    foreach (Set set in entity.Sets)
                    {
                        cmdSets.CommandText = "INSERT INTO Sets (WorkoutId, ExerciseId, SetType, ";
                        cmdSets.Parameters.Clear();
                        cmdSets.Parameters.AddWithValue("@WorkoutId", entity.Id);
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
                            DateTime time = Convert.ToDateTime($"{set.Hours}:{set.Minutes}:{set.Seconds}");
                            cmdSets.Parameters.AddWithValue("@Time", time);
                        }

                        cmdSets.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Result.Fail("Something happened whilst trying to update the workout.");
                }
            }

            return Result.Ok();
        }

        
    }
}
