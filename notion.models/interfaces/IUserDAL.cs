using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using notion.models.dto;
using utils;

namespace notion.models.interfaces
{
    public interface IUserDAL
    {
        Task<JustifiedValue<User>> CreateUser(User user);
        Task<JustifiedValue<User>> GetUserByEmail(string email);
    }
}