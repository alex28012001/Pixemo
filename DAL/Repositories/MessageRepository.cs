using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class MessageRepository : IRepository<Message>
    {
        private PixemContext _db;
        public MessageRepository(DbContext db)
        {
            _db = (PixemContext)db;
        }
        public void Create(Message item)
        {
            _db.Messages.Add(item);
        }

        public int CountWithExpressionTree(Expression<Func<Message, Boolean>> predicate)
        {
            return _db.Messages.Where(predicate).Count();
        }

        public bool Any()
        {
            return _db.Messages.Any();
        }

        public bool Any(Func<Message, bool> predicate)
        {
            return _db.Messages.Any(predicate);
        }

        public bool AnyWithExpressionTree(Expression<Func<Message, bool>> predicate)
        {
            return _db.Messages.Any(predicate);
        }


        public IEnumerable<Message> Find(Func<Message, bool> predicate)
        {
            return _db.Messages.Where(predicate);
        }

        public IEnumerable<Message> FindWithExpressionTree(Expression<Func<Message, bool>> predicate)
        {
            _db.Configuration.LazyLoadingEnabled = false;
            return _db.Messages.Where(predicate)
                               .Select(p => new
                               {
                                   Date = p.Date,
                                   Text = p.Text,
                                   UserName = p.User.UserName
                               }).AsEnumerable()
                               .Select(p => new Message()
                               {
                                   Date = p.Date,
                                   Text = p.Text,
                                   User = new ClientProfile() { UserName = p.UserName }
                               });           
        }
    

        public IEnumerable<Message> GetAll()
        {
            _db.Configuration.LazyLoadingEnabled = false;
            return _db.Messages.Include("User").Include("ChatRoom"); //!!!
        }

        public Message GetByID(int id)
        {
            return _db.Messages.Find(id);
        }

        public async Task<Message> GetByIDAsync(int id)
        {
            return await _db.Messages.FindAsync(id);
        }

        public void Remove(Message item)
        {
            _db.Messages.Remove(item);
        }

        public void RemoveByID(int id)
        {
            Message message = _db.Messages.Find(id);
            if (message != null)
                _db.Messages.Remove(message);
        }

        public async Task RemoveByIDAsync(int id)
        {
            Message message = await _db.Messages.FindAsync(id);
            if (message != null)
                _db.Messages.Remove(message);
        }

        public void Update(Message item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }  
    }
}
