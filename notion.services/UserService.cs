using System.Threading.Tasks.Dataflow;
using notion.models.dto;
using notion.models.interfaces;

namespace notion.services;
public class UserService
{
  private readonly IUserDAL _dal;

  public UserService(IUserDAL dal) => _dal = dal;

  public Task<User> CreateUser(User user)
  {
    return _dal.CreateUser(user);
  }
}
