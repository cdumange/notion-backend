using System.Threading.Tasks.Dataflow;
using notion.models.dto;
using notion.models.interfaces;
using utils;

namespace notion.services;
public class UserService : IUserService
{
  private readonly IUserDAL _dal;

  public UserService(IUserDAL dal) => _dal = dal;

  public Task<JustifiedValue<User>> CreateUser(User user)
  {
    return _dal.CreateUser(user);
  }

  public Task<JustifiedValue<User>> GetUserByEmail(string email)
  {
    return _dal.GetUserByEmail(email);
  }
}

public interface IUserService
{
  Task<JustifiedValue<User>> CreateUser(User user);
  Task<JustifiedValue<User>> GetUserByEmail(string email);
}
