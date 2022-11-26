using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using System.Collections.Generic;
using Xunit;

namespace MyTrainingPal.UnitTests;

public class WorkoutTests
{
    string _name = "Workout test";
    int _numberOfSets = 5;
    WorkoutType _type = WorkoutType.Yoga;
    List<Set> _sets = new List<Set>()
    {
        Set.Generate(Exercise.Generate(
            "Ejercicio 1", 
            new List<MuscleGroup>(),
            DifficultyLevel.Advanced,
            ForceType.Pull,
            true,
            "urlprueba").Value,
            SetType.ByRepetition
            ).Value
    };
    int _creatorId = 1;

    [Fact]
    public void GenerateWorkout_EveryParameterAdded_IsSuccess()
    {
        Result result = Workout.Generate(_type, _name, _numberOfSets, _creatorId);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void GenerateWorkout_NameEmpty_Fails()
    {
        Result result = Workout.Generate(_type, "", _numberOfSets, _creatorId);

        Assert.True(result.IsFailure);
    }


    [Fact]
    public void GenerateWorkout_NameNull_Fails()
    {
        Result result = Workout.Generate(_type, null, _numberOfSets, _creatorId);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateWorkout_NumberOfSetsLessThanOne_Fails()
    {
        Result result = Workout.Generate(_type, _name, 0, _creatorId);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateWorkout_NumberOfSetsNull_Fails()
    {
        Result result = Workout.Generate(_type, _name, 0, _creatorId);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateWorkout_CreatorIdLessThanOne_Fails()
    {
        Result result = Workout.Generate(_type, _name, 0, _creatorId);

        Assert.True(result.IsFailure);
    }
}
