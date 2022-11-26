using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using System.Collections.Generic;
using Xunit;

namespace MyTrainingPal.UnitTests;

public class SetTests
{
    SetType _timedType = SetType.ByTime;
    SetType _repType = SetType.ByRepetition;
    int _hours = 1;
    int _minutes = 2;
    int _seconds = 3;
    int _reps = 4;
    Exercise _exercise = Exercise.Generate(
            "Ejercicio 1",
            new List<MuscleGroup>(),
            DifficultyLevel.Advanced,
            ForceType.Pull,
            true,
            "urlprueba").Value;

    [Fact]
    public void GenerateSet_NoExerciseAdded_Fails()
    {
        Result result = Set.Generate(null, _timedType);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateTimedSet_EveryParameterAdded_IsSuccess()
    {
        Result result = Set.Generate(_exercise, _timedType, hours: _hours, minutes: _minutes, seconds: _seconds);
        Assert.True(result.IsSuccess);
    }


    [Fact]
    public void GenerateTimedSet_NoTimeAdded_Fails()
    {
        Result result = Set.Generate(_exercise, _timedType);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateTimedSet_LessThanASecond_Fails()
    {
        Result result = Set.Generate(_exercise, _timedType, hours: 0, minutes: 0, seconds: 0);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateTimedSet_AtLeastASecond_IsSuccess()
    {
        Result result = Set.Generate(_exercise, _timedType, hours: 0, minutes: 0, seconds: 1);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void GenerateTimedSet_AddingReps_Fails()
    {
        Set set = Set.Generate(_exercise, _timedType, hours: _hours, minutes: _minutes, seconds: _seconds).Value;
        Result result = set.AddReps(1);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateRepsSet_EveryParameterAdded_IsSuccess()
    {
        Result result = Set.Generate(_exercise, _repType, repetitions: _reps);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void GenerateRepsSet_LessThanARepetition_Fails()
    {
        Result result = Set.Generate(_exercise, _repType, repetitions: 0);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateRepsSet_AddingTime_Fails()
    {
        Set set = Set.Generate(_exercise, _repType, repetitions: _reps).Value;
        Result result = set.AddTime(1, 0, 0);

        Assert.True(result.IsFailure);
    }
}
