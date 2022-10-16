using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using System.Collections.Generic;
using Xunit;

namespace MyTrainingPal.UnitTests
{
    public class ExerciseTests
    {
        string _name = "Push-ups";
        DifficultyLevel _difficulty = DifficultyLevel.Beginner;
        ForceType _force = ForceType.Push;
        bool _hasEquipment = false;
        List<MuscleGroup> _listWithMuscleGroups = new List<MuscleGroup>()
        {
            new MuscleGroup(1, "Chest"),
            new MuscleGroup(2, "Back")
        };

        [Fact]
        public void GenerateExercise_EveryParameterAdded_IsSuccess()
        {         
            Result result = Exercise.Generate(_name, _listWithMuscleGroups, _difficulty, _force, _hasEquipment);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void GenerateExercise_NameEmpty_Fails()
        {
            Result result = Exercise.Generate("", _listWithMuscleGroups, _difficulty, _force, _hasEquipment);

            Assert.True(result.IsFailure);
        }

        [Fact]
        public void GenerateExercise_NameNull_Fails()
        {
            Result result = Exercise.Generate(null, _listWithMuscleGroups, _difficulty, _force, _hasEquipment);

            Assert.True(result.IsFailure);
        }

        [Fact]
        public void GenerateExercise_ListMuscleGroupsWithNulls_Fails()
        {
            MuscleGroup muscleGroupNull = null;
            MuscleGroup muscleGroup = new MuscleGroup(1, "Push-ups" );

            List<MuscleGroup> muscles = new List<MuscleGroup>();
            muscles.Add(muscleGroup);
            muscles.Add(muscleGroupNull);

            Result result = Exercise.Generate(_name, muscles, _difficulty, _force, _hasEquipment);

            Assert.True(result.IsFailure);
        }
    }
}
