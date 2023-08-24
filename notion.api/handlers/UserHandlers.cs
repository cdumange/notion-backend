using notion.models.dto;
using notion.models.interfaces;

namespace notion.api.handlers
{
    public static class UserHandlers
    {
        public static void RegisterHandlers(WebApplication app)
        {
            app.MapPost("/api/v1/users", CreateUser);
        }

        internal static async Task<IResult> CreateUser(IUserService s, User user)
        {
            var res = await s.CreateUser(user);
            if (res)
            {
                return Results.Created($"/users/{user.ID}", res.Value);
            }

            return Results.Problem(res.Exception.Message);
        }
    }
}