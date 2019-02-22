using BL.DTO;
using BL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BL.Services.DbAbstraction
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task<string> FindIdByUserNameAsync(string userName);
        string FindIdByUserName(string userName);
        Task SetInitialData(UserDTO adminDto, IEnumerable<string> roles);  
        bool IsInRole(string userId,string role);
        Task<bool> IsInRoleAsync(string userId, string role);
    }
}
