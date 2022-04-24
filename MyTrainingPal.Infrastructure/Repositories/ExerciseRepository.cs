using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using MyTrainingPal.Infrastructure.Filters;
using MyTrainingPal.Infrastructure.Interfaces;
using System.Data.SqlClient;

namespace MyTrainingPal.Infrastructure.Repositories
{
    public class ExerciseRepository : BaseRepository, IExerciseRepository
    {
        public void Add(Exercise entity) 
        {

        }

        public void Delete(int id)
        {

        }

        public IEnumerable<Exercise> GetAll(int page = 0, int pageSize = 5)
        {
            List<Exercise> exercises = new List<Exercise>();

            try
            {
                using SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT ExerciseId, Exercises.Name AS ExerciseName, Level, ForceType, RequiresEquipment, MuscleGroupId, MuscleGroups.Name AS MuscleGroupName FROM Exercises JOIN ExercisesMuscleGroups ON ExercisesMuscleGroups.ExerciseId = Exercises.Id JOIN MuscleGroups ON ExercisesMuscleGroups.MuscleGroupId = MuscleGroups.Id ORDER BY ExerciseId OFFSET(@Skip) ROWS FETCH NEXT(@Take) ROWS ONLY", con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Skip", page * pageSize);
                cmd.Parameters.AddWithValue("@Take", pageSize);

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

                        MuscleGroup muscleGroup = new MuscleGroup()
                        {
                            Id = (int)reader["MuscleGroupId"],
                            Name = (string)reader["MuscleGroupName"]
                        };

                        if(exercises.Any(e => e.Id == exercise.Id))
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

            }

            return exercises;
        }

        public IEnumerable<Exercise> FindMatches(ExerciseFilter filter = null, int page = 0, int pageSize = 5)
        {
            List<Exercise> exercises = new List<Exercise>();
            string sqlCondition = "";

            if (filter != null)
                sqlCondition = " WHERE ";

            if (filter?.ForceType != null)
                sqlCondition += "";
            

            try
            {
                using SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("", con);
                SqlDataReader reader = cmd.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return exercises;
        }

        public Exercise GetById(int id)
        {

            return null;
        }

        public void Update(Exercise entity)
        {

        }
    }
}
