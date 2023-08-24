using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using notion.api.handlers;
using notion.models.dto;
using notion.models.interfaces;
using NSubstitute;

namespace notion.api.tests;

public class UserHandlerTests
{
    [Fact]
    public async void TestCreateUser_Ok()
    {
        var user = new User
        {
            Email = "anemail@gmail.com"
        };
        var expectedUser = new User
        {
            Email = "anemail@gmail.com",
            ID = Guid.NewGuid(),
            CreationDate = DateTime.Now
        };

        var servicemok = Substitute.For<IUserService>();

        servicemok.CreateUser(user).Returns(expectedUser);

        var res = await UserHandlers.CreateUser(servicemok, user);
        Assert.IsType<Created<User>>(res);
        Assert.Equal(expectedUser, ((Created<User>)res).Value);

        await servicemok.Received(1).CreateUser(user);
    }

    [Fact]
    public async void TestCreateUser_Existing()
    {
        var user = new User
        {
            Email = "anemail@gmail.com"
        };

        var servicemok = Substitute.For<IUserService>();

        servicemok.CreateUser(user).Returns(User.Exceptions.UserAlreadyExists);

        var res = await UserHandlers.CreateUser(servicemok, user);
        Assert.IsType<ProblemHttpResult>(res);
        Assert.Equal(User.Exceptions.UserAlreadyExists.Message, ((ProblemHttpResult)res).ProblemDetails.Detail);
        await servicemok.Received(1).CreateUser(user);
    }

    [Fact]
    public async Task TestCreateUser_BadRequestAsync()
    {
        var user = new User
        {
            Email = "anemail  gmail.com"
        };


        var servicemok = Substitute.For<IUserService>();

        var res = await UserHandlers.CreateUser(servicemok, user);
        Assert.IsType<BadRequest<List<ValidationResult>>>(res);
    }

    [Fact]
    public async void TestGetUserByEmail_Ok()
    {
        // Given
        var email = "anemail@gmail.com";
        var expectedUser = new User { Email = email };

        var servicemok = Substitute.For<IUserService>();
        servicemok.GetUserByEmail(email).Returns(expectedUser);

        // When

        var res = await UserHandlers.GetUserByEmail(servicemok, email);

        // Then
        Assert.IsType<Ok<User>>(res);
        Assert.Equal(expectedUser, ((Ok<User>)res).Value);
    }

    [Fact]
    public async void TestGetUserByEmail_NotFound()
    {
        // Given
        var email = "anemail@gmail.com";

        var servicemok = Substitute.For<IUserService>();
        servicemok.GetUserByEmail(email).Returns(User.Exceptions.UserNotFound);

        // When

        var res = await UserHandlers.GetUserByEmail(servicemok, email);

        // Then
        Assert.IsType<NotFound>(res);
    }

    [Fact]
    public async void TestGetUserByEmail_Error()
    {
        // Given
        var email = "anemail@gmail.com";
        var expectedMessage = "an error message";

        var servicemok = Substitute.For<IUserService>();
        servicemok.GetUserByEmail(email).Returns(new Exception(expectedMessage));

        // When

        var res = await UserHandlers.GetUserByEmail(servicemok, email);

        // Then
        Assert.IsType<ProblemHttpResult>(res);
        Assert.Equal(expectedMessage, ((ProblemHttpResult)res).ProblemDetails.Detail);
    }
}