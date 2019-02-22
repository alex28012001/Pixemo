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
    public class ChatRoomRepository : IRepository<ChatRoom>
    {
        private PixemContext _db;
        public ChatRoomRepository(DbContext db)
        {
            _db = (PixemContext)db;
        }
        public void Create(ChatRoom item)
        {
            _db.ChatRooms.Add(item);
        }

        public int CountWithExpressionTree(Expression<Func<ChatRoom, Boolean>> predicate)
        {
            return _db.ChatRooms.Where(predicate).Count();
        }

        public bool Any()
        {
            return _db.ChatRooms.Any();
        }

        public bool Any(Func<ChatRoom, bool> predicate)
        {
            return _db.ChatRooms.Any(predicate);
        }

        public bool AnyWithExpressionTree(Expression<Func<ChatRoom, bool>> predicate)
        {
            return _db.ChatRooms.Any(predicate);
        }


        public IEnumerable<ChatRoom> Find(Func<ChatRoom, bool> predicate)
        {
            return _db.ChatRooms.Where(predicate);
        }

        public IEnumerable<ChatRoom> FindWithExpressionTree(Expression<Func<ChatRoom, bool>> predicate)
        {
            return _db.ChatRooms.Where(predicate);
        }

        public IEnumerable<ChatRoom> GetAll()
        {
            return _db.ChatRooms.Include("Message").Include("ClientProfile");
        }

        public ChatRoom GetByID(int id)
        {
            return _db.ChatRooms.Find(id);
        }

        public async Task<ChatRoom> GetByIDAsync(int id)
        {
            return await _db.ChatRooms.FindAsync(id);
        }


        public void Remove(ChatRoom item)
        {
            _db.ChatRooms.Remove(item);
        }

        public void RemoveByID(int id)
        {
            ChatRoom room = _db.ChatRooms.Find(id);
            if (room != null)
                _db.ChatRooms.Remove(room);
        }

        public async Task RemoveByIDAsync(int id)
        {
            ChatRoom room = await _db.ChatRooms.FindAsync(id);
            if (room != null)
                _db.ChatRooms.Remove(room);
        }

        public void Update(ChatRoom item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
