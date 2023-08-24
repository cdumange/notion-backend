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
    }
}