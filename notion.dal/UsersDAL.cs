using System.Data;
using Dapper;
using notion.dal.models;
using utils;

namespace notion.dal;
public class UsersDAL
{
    private readonly IDbConnection db;
    public const string TableName = "users";
    public UsersDAL(IDbConnection db)
    {
        this.db = db;
    }

    public Task<User> CreateUser(User user)
    {
        return this.db.QueryFirstOrDefaultAsync<User>(
            @$"INSERT INTO {TableName} (Email) VALUES (@Email)
            RETURNING id, email, creation_date", user);
    }

    public async Task<JustifiedValue<User>> GetUserByEmail(string email)
    {
        var user = await this.db.QueryFirstOrDefaultAsync<User>(
            $"SELECT * FROM {TableName} WHERE email = @email", new { email }
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
