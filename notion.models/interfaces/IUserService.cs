using notion.models.dto;
using utils;

namespace notion.models.interfaces
{

    public interface IUserService
    {
        Task<JustifiedValue<User>> CreateUser(User user);
        Task<JustifiedValue<User>> GetUserByEmail(string email);
    }
}