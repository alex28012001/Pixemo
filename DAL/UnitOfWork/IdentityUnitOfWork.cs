using DAL.Repositories;
using DAL.Context;
using System;
using DAL.Entities;
using System.Data.Entity;
using DAL.Identity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using DAL.Entities.UserStore.DAL.Entities;

namespace DAL.UnitOfWork
{
    public class IdentityUnitOfWork: IUnitOfWork
    {
        private PixemContext _db;

        private IRepository<Comment> _commentsRepository;
        private IRepository<Advertising> _advertisingRepository;
        private IRepository<Message> _messagesRepository;
        private IRepository<ChatRoom> _chatRoomRepository;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private IRepository<ClientProfile> _clientManagerRepository;

        public IdentityUnitOfWork()
        {
            _db = new PixemContext("DefaultConnection");    
        }

   
        public IRepository<Comment> Comments
        {
            get
            {
                if (_commentsRepository == null)
                    _commentsRepository = new CommentsRepository(_db);
                return _commentsRepository;
            }
        }


        public IRepository<Message> Messages
        {
            get
            {
                if (_messagesRepository == null)
                    _messagesRepository = new MessageRepository(_db);
                return _messagesRepository;
            }
        }


        public IRepository<ChatRoom> ChatRooms
        {
            get
            {
                if (_chatRoomRepository == null)
                    _chatRoomRepository = new ChatRoomRepository(_db);
                return _chatRoomRepository;
            }
        }

       

        public IRepository<Advertising> Advertising
        {
            get
            {
                if (_advertisingRepository == null)
                    _advertisingRepository = new AdvertisingRepository(_db);
                return _advertisingRepository;
            }
        }


        public ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
                return _userManager;
            }
        }

        public IRepository<ClientProfile> ClientManager
        {
            get
            {
                if (_clientManagerRepository == null)
                    _clientManagerRepository = new ClientManagerRepository(_db);
                return _clientManagerRepository;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                if (_roleManager == null)
                    _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
                return _roleManager;
            }
        }


        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }


        public void Save()
        {
            _db.SaveChanges();
        }


        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
