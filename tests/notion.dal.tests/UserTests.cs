using System.Data;
using notion.migrations;
using Microsoft.Extensions.Logging.Abstractions;
using notion.models.dto;

namespace notion.dal.tests;

[Collection("Database collection")]
public class UserTests
{
    private readonly DBFixture dbFixture;
    public UserTests(DBFixture dbFixture)
    {
        this.dbFixture = dbFixture;
    }

    private void CleanTable()
    {
        var req = this.dbFixture.GetConnection().CreateCommand();
        req.CommandText = "DELETE FROM " + UsersDAL.TableName;

        req.ExecuteNonQuery();
    }

    [Fact]
    public async Task TestCreateAsync()
    {
        CleanTable();
        var u = new User { Email = "test@test.com" };
        var s = new UsersDAL(dbFixture.GetConnection());
        var ret = await s.CreateUser(u);

        Assert.True(ret);
        Assert.NotNull(ret.Value);

        var user = ret.Value;

        Assert.Equal(u.Email, user.Email);
        Assert.NotEqual(user.ID, Guid.Empty);
        Assert.NotEqual(user.CreationDate, DateTime.MinValue);

        // ensure unicity on email
        ret = await s.CreateUser(u);
        Assert.False(ret);
        Assert.Equal(User.Exceptions.UserAlreadyExists, ret.Exception);
    }

    [Fact]
    public async void TestGetUserByEmail()
    {
        CleanTable();
        // Given
        string existingEmail = "existing@email.com",
            absentEmail = "absent@gmail.com";
        var s = new UsersDAL(dbFixture.GetConnection());

        // When

        await s.CreateUser(new User { Email = existingEmail });

        var existing = await s.GetUserByEmail(existingEmail);
        var not = await s.GetUserByEmail(absentEmail);

        // Then

        Assert.True(existing);
        Assert.NotNull(existing.Value);
        Assert.Equal(existingEmail, existing.Value.Email);

        Assert.False(not);
        Assert.Null(not.Value);
        Assert.Equal(User.Exceptions.UserNotFound, not.Exception);
    }
}