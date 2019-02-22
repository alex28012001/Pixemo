using DAL.Entities;
using DAL.Identity;
using DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IRepository<ClientProfile> ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }
        IRepository<Comment> Comments { get; }
        IRepository<Advertising> Advertising { get; }
        IRepository<Message> Messages { get; }
        IRepository<ChatRoom> ChatRooms { get; }
        Task SaveAsync();
        void Save();
    }
}
