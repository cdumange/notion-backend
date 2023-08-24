using System.Data;
using System.Net.Http.Headers;
using Dapper;
using notion.models.dto;
using notion.models.interfaces;
using utils;
using static notion.models.dto.User;

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

    public async Task<JustifiedValue<User>> CreateUser(User user)
    {
        try
        {
            User ret = await this.db.QueryFirstOrDefaultAsync<User>(
            @$"INSERT INTO {TableName} (Email) VALUES (@Email)
            RETURNING {userProjection}", user);
            return ret;
        }
        catch (Exception e)
        {
            if (e.Message.Contains("unique constraint \"users_email_key\""))
            {
                return Exceptions.UserAlreadyExists;
            }
            throw;
        }

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
}
