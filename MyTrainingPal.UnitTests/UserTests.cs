using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using System;
using Xunit;

namespace MyTrainingPal.UnitTests;

public class UserTests
{
    string _name = "Victor";
    string _lastName = "Pérez Asuaje";
    string _email = "v.perezasuaje@gmail.com";
    string _password = "Hola*2021";
    bool _isPremium = false;

    [Fact]
    public void GenerateUser_EveryParameterAdded_IsSuccess()
    {
        Result result = User.Generate(_name, _lastName, _email, _password, DateTime.Now, _isPremium);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void GenerateUser_NameEmpty_Fails()
    {
        Result result = User.Generate("", _lastName, _email, _password, DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_NameNull_Fails()
    {
        Result result = User.Generate(null, _lastName, _email, _password, DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_NameOver50Chars_Fails()
    {
        Result result = User.Generate("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer dapibus ligula sed magna auctor tempus.", _lastName, _email, _password, DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_LastNameEmpty_Fails()
    {
        Result result = User.Generate(_name, "", _email, _password, DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_LastNameNull_Fails()
    {
        Result result = User.Generate(_name, null, _email, _password, DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_LastNameOver150Chars_Fails()
    {
        Result result = User.Generate(_name, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer dapibus ligula sed magna auctor tempus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer dapibus ligula sed magna auctor tempus.", _email, _password, DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_EmailEmpty_Fails()
    {
        Result result = User.Generate(_name, _lastName, "", _password, DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_EmailNull_Fails()
    {
        Result result = User.Generate(_name, _lastName, null, _password, DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_EmailOver150Chars_Fails()
    {
        Result result = User.Generate(_name, _lastName, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer dapibus ligula sed magna auctor tempus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer dapibus ligula sed magna auctor tempus.", _password, DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_EmailInvalidEmail_Fails()
    {
        Result result = User.Generate(_name, _lastName, "asd.com", _password, DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_PasswordEmpty_Fails()
    {
        Result result = User.Generate(_name, _lastName, _email, "", DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_PasswordNull_Fails()
    {
        Result result = User.Generate(_name, _lastName, _email, null, DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_PasswordUnder8Chars_Fails()
    {
        Result result = User.Generate(_name, _lastName, _email, "Pass", DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_PasswordOver20Chars_Fails()
    {
        Result result = User.Generate(_name, _lastName, _email, "EsteEsUnPasswordSuperiorA20Caracteres", DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_PasswordWithoutUppercase_Fails()
    {
        Result result = User.Generate(_name, _lastName, _email, "esteesunpassword", DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_PasswordWithoutLowercase_Fails()
    {
        Result result = User.Generate(_name, _lastName, _email, "ESTEESUNPASSWORD", DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_PasswordWithoutNumber_Fails()
    {
        Result result = User.Generate(_name, _lastName, _email, "EsteEsUnPassword", DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_PasswordWithoutSpecialChars_Fails()
    {
        Result result = User.Generate(_name, _lastName, _email, "EsteEsUnP4ssword", DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GenerateUser_PasswordWithInvalidSpecialChars_Fails()
    {
        Result result = User.Generate(_name, _lastName, _email, "EsteEsUnP4ssword_", DateTime.Now, _isPremium);

        Assert.True(result.IsFailure);
    }
}
