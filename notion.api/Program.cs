using System.Data;
using notion.api.handlers;
using notion.dal;
using notion.models.dto;
using notion.models.interfaces;
using notion.services;
using Npgsql;



internal class Program
{
  private static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddScoped<IUserDAL, UsersDAL>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddSingleton<IDbConnection>(x => NpgsqlDataSource.Create(builder.Configuration.GetConnectionString("postgres")).CreateConnection());

    var app = builder.Build();

    UserHandlers.RegisterHandlers(app);

    app.Run();
  }

}