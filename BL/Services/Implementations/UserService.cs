using BL.DTO;
using BL.Infrastructure;
using BL.Services.DbAbstraction;
using DAL.Entities;
using DAL.Entities.UserStore.DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace BL.Services.Implementations
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _db;

        public UserService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await _db.UserManager.FindByNameAsync(userDto.UserName);
            if (user == null)
            {
                user = new ApplicationUser() {UserName = userDto.UserName};
                var result = await _db.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                await _db.UserManager.AddToRoleAsync(user.Id, userDto.Role); // добавляем роль
               
                ClientProfile clientProfile = new ClientProfile { Id = user.Id, UserName = userDto.UserName }; // создаем профиль клиента
                _db.ClientManager.Create(clientProfile);
                await _db.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует","Login");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await _db.UserManager.FindAsync(userDto.UserName, userDto.Password);
            if (user != null)
                claim = await _db.UserManager.CreateIdentityAsync(user,  DefaultAuthenticationTypes.ApplicationCookie);                           
            return claim;
        }
      

        public async Task<string> FindIdByUserNameAsync(string userName)
        {
            return (await _db.UserManager.FindByNameAsync(userName)).Id;
        }
        public string FindIdByUserName(string userName)
        {
            return _db.UserManager.FindByName(userName).Id;
        }

        // начальная инициализация бд
        public async Task SetInitialData(UserDTO adminDto, IEnumerable<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await _db.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await _db.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }

        public bool IsInRole(string userId, string role)
        {
            return _db.UserManager.IsInRole(userId, role);
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            return await _db.UserManager.IsInRoleAsync(userId, role);
        }


        private bool disposing = false;
        public void Dispose()
        {
           if(!disposing)
           {
               _db.Dispose();
           }
           disposing = true;
        }
    }
}
