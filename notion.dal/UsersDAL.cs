using System.Data;
using Dapper;
using notion.dal.models;

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
}
