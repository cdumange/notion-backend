using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using notion.models.dto;
using notion.models.interfaces;

namespace notion.api.handlers
{
    public static class UserHandlers
    {
        public static void RegisterHandlers(WebApplication app)
        {
            app.MapPost("/api/v1/users", CreateUser);
            app.MapGet("/api/v1/users/byemail", GetUserByEmail);
        }

        internal static async Task<IResult> CreateUser(IUserService s, User user)
        {
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(user, new ValidationContext(user), results, true))
            {
                return Results.BadRequest(results);
            }

            var res = await s.CreateUser(user);
            if (res)
            {
                return Results.Created($"/users/{user.ID}", res.Value);
            }

            return Results.Problem(res.Exception.Message);
        }

        internal static async Task<IResult> GetUserByEmail(IUserService s, string email)
        {
            var res = await s.GetUserByEmail(email);
            if (res)
            {
                return Results.Ok(res.Value);
            }

            if (res.Exception == User.Exceptions.UserNotFound)
            {
                return Results.NotFound();
            }

            return Results.Problem(res.Exception.Message);
        }
    }
}