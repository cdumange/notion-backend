using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using notion.dal;
using notion.migrations;
using notion.models.dto;
using notion.models.interfaces;
using notion.services;
using Npgsql;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUserDAL, UsersDAL>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IDbConnection>(x => NpgsqlDataSource.Create(builder.Configuration.GetConnectionString("postgres")).CreateConnection());

var app = builder.Build();

app.MapPost("/users/create", async (IUserService s, User user) =>
{
  var res = await s.CreateUser(user);
  if (res)
  {
    return Results.Created($"/users/{user.ID}", user);
  }

  return Results.Problem(res.Exception.Message);
});

app.Run();
