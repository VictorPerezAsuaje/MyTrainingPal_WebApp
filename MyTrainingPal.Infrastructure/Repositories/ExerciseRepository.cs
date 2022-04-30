using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using MyTrainingPal.Domain.Interfaces;
using System.Data.SqlClient;

namespace MyTrainingPal.Infrastructure.Repositories
{
    public interface IExerciseRepository : IRepository<Exercise>
    {
    }
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=MyTrainingPalDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public Result<List<Exercise>> GetAll(int? page = null, int? pageSize = null)
        {
            if(page < 0)
                return Result.Fail<List<Exercise>>("The page can not be less than 0.");

            if (pageSize < 1)
                return Result.Fail<List<Exercise>>("The page size can not be less than 1.");

            if ((page == null && pageSize != null) || (page != null && pageSize == null))
                return Result.Fail<List<Exercise>>("If a page or a page size is requested, the other one must be indicated as well. Please make sure to request both the page and page size if you want pagination.");

            List<Exercise> exercises = new List<Exercise>();

            try
            {
                using SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT ExerciseId, Exercises.Name AS ExerciseName, Level, ForceType, RequiresEquipment, MuscleGroupId, MuscleGroups.Name AS MuscleGroupName FROM Exercises JOIN ExercisesMuscleGroups ON ExercisesMuscleGroups.ExerciseId = Exercises.Id JOIN MuscleGroups ON ExercisesMuscleGroups.MuscleGroupId = MuscleGroups.Id ORDER BY ExerciseId ";

                cmd.Parameters.Clear();

                if(page != null && pageSize != null)
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
                        int id = (int)reader["ExerciseId"];
                        string name = (string)reader["ExerciseName"];
                        DifficultyLevel level = (DifficultyLevel)reader["Level"];
                        ForceType forceType = (ForceType)reader["ForceType"];
                        bool requiresEquipment = (bool)reader["RequiresEquipment"];

                        Exercise exercise = Exercise.Generate(name, new List<MuscleGroup>(), level, forceType, requiresEquipment, id).Value;

                        MuscleGroup muscleGroup = new MuscleGroup(
                            id: (int)reader["MuscleGroupId"],
                            name: (string)reader["MuscleGroupName"]);

                        if (exercises.Any(e => e.Id == exercise.Id))
                        {
                            exercises.Where(e => e.Id == exercise.Id)
                            .FirstOrDefault()?.MuscleGroups.Add(muscleGroup);
                        }
                        else
                        {
                            exercise.MuscleGroups.Add(muscleGroup);
                            exercises.Add(exercise);
                        }
                    }
                }

                con.Close();
            }
            catch (Exception ex)
            {
                Result.Fail<List<Exercise>>("The list of exercises could not be retrieved.");
            }

            return Result.Ok(exercises);
        }

        public Result<Exercise> GetById(int id)
        {
            Exercise exercise = null;

            try
            {
                using SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT Id, Name, Level, ForceType, RequiresEquipment FROM Exercises WHERE Id = @ExerciseId";
                cmd.Parameters.AddWithValue("@ExerciseId", id);

                SqlDataReader reader = cmd.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        if(exercise == null)
                        {
                            Result<Exercise> exerciseResult = Exercise.Generate(
                            id: (int)reader["Id"],
                            name: (string)reader["Name"],
                            level: (DifficultyLevel)reader["Level"],
                            forceType: (ForceType)reader["ForceType"],
                            hasEquipment: (bool)reader["RequiresEquipment"],
                            muscleGroups: new List<MuscleGroup>());

                            if (exerciseResult.IsFailure)
                                return Result.Fail<Exercise>(exerciseResult.Error);

                            exercise = exerciseResult.Value;
                        }
                        
                        MuscleGroup muscleGroup = new MuscleGroup(
                            id: (int)reader["MuscleGroupId"],
                            name: (string)reader["MuscleGroupName"]);

                        exercise.MuscleGroups.Add(muscleGroup);
                    }
                }

                con.Close();
            }
            catch (Exception ex)
            {
                Result.Fail<List<Exercise>>("The list of exercises could not be retrieved.");
            }

            return Result.Ok(exercise);
        }

        public Result Add(Exercise entity)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Result Update(Exercise entity)
        {
            throw new NotImplementedException();
        }
    }
}
