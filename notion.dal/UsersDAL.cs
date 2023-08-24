using System.Data;
using Dapper;
using notion.models.dto;
using notion.models.interfaces;
using utils;

namespace notion.dal;
public class UsersDAL : IUserDAL
{
    private readonly IDbConnection db;
    public const string TableName = "users";
    private const string userProjection = "id, email, creation_date as CreationDate, modification_date as ModificationDate";
    public UsersDAL(IDbConnection db)
    {
        this.db = db;
    }

    public Task<User> CreateUser(User user)
    {
        return this.db.QueryFirstOrDefaultAsync<User>(
            @$"INSERT INTO {TableName} (Email) VALUES (@Email)
            RETURNING {userProjection}", user);
    }

    public async Task<JustifiedValue<User>> GetUserByEmail(string email)
    {
        var user = await this.db.QueryFirstOrDefaultAsync<User>(
            $"SELECT {userProjection} FROM {TableName} WHERE email = @email", new { email }
        );

        if (user == null)
        {
            return Exceptions.UserNotFound;
        }

        return user;
    }


    public class Exceptions
    {
        public static Exception UserNotFound = new Exception("no user for this email");
    }
}
