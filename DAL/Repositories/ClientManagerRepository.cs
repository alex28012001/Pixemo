using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Context;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq.Expressions;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class ClientManagerRepository : IRepository<ClientProfile>
    {
        private PixemContext _db;
        public ClientManagerRepository(IdentityDbContext<ApplicationUser> db)
        {
            _db = (PixemContext)db;
        }


        public void Create(ClientProfile item)
        {
            _db.ClientProfiles.Add(item);
        }

        public int CountWithExpressionTree(Expression<Func<ClientProfile, Boolean>> predicate)
        {
            return _db.ClientProfiles.Where(predicate).Count();
        }

        public bool Any()
        {
            return _db.ClientProfiles.Any();
        }

        public bool Any(Func<ClientProfile, bool> predicate)
        {
            return _db.ClientProfiles.Any(predicate);
        }

        public bool AnyWithExpressionTree(Expression<Func<ClientProfile, bool>> predicate)
        {
            return _db.ClientProfiles.Any(predicate);
        }


        public IEnumerable<ClientProfile> Find(Func<ClientProfile, bool> predicate)
        {
            return _db.ClientProfiles.Where(predicate);
        }

        public IEnumerable<ClientProfile> FindWithExpressionTree(Expression<Func<ClientProfile, bool>> predicate)
        {
            return _db.ClientProfiles.Where(predicate);
        }

        public IEnumerable<ClientProfile> GetAll()
        {
            return _db.ClientProfiles;
        }

        public ClientProfile GetByID(int id)
        {
            return _db.ClientProfiles.Find(id);
        }

        public Task<ClientProfile> GetByIDAsync(int id)
        {
            return _db.ClientProfiles.FindAsync(id);
        }

        public void Remove(ClientProfile item)
        {
            _db.ClientProfiles.Remove(item);
        }

        public void RemoveByID(int id)
        {
            ClientProfile profile = _db.ClientProfiles.Find(id);
            if (profile != null)
                _db.ClientProfiles.Remove(profile);
        }

        public async Task RemoveByIDAsync(int id)
        {
            ClientProfile profile = await _db.ClientProfiles.FindAsync(id);
            if (profile != null)
                _db.ClientProfiles.Remove(profile);
        }

        public void Update(ClientProfile item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
