using DAL.Context;
using DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class AdvertisingRepository : IRepository<Advertising>
    {
        private PixemContext _db;
        public AdvertisingRepository(IdentityDbContext<ApplicationUser> db)
        {
            _db = (PixemContext)db;
        }
        public void Create(Advertising item)
        {
            _db.Advertising.Add(item);
        }

        public int CountWithExpressionTree(Expression<Func<Advertising, Boolean>> predicate)
        {
            return _db.Advertising.Where(predicate).Count();
        }


        public bool Any()
        {
            return _db.Advertising.Any();
        }

        public bool Any(Func<Advertising, bool> predicate)
        {
            return _db.Advertising.Any(predicate);
        }

        public bool AnyWithExpressionTree(Expression<Func<Advertising, bool>> predicate)
        {
            return _db.Advertising.Any(predicate);
        }

        public IEnumerable<Advertising> Find(Func<Advertising, bool> predicate)
        {
            return _db.Advertising.Where(predicate);      
        }

        public IEnumerable<Advertising> FindWithExpressionTree(Expression<Func<Advertising, bool>> predicate)
        {
            _db.Configuration.LazyLoadingEnabled = false;
            return  _db.Advertising.Where(predicate)
                                   .Select(p => new
                                   {
                                       AdId = p.Id,
                                       ImageId = p.ImageId,
                                       ImageUrl = p.ImageUrl,
                                       ExpirationDate = p.ExpirationDate,
                                       AdvertisingLink = p.AdvertisingLink,
                                       UserId = p.User.Id
                                   }).AsEnumerable()
                                   .Select(p => new Advertising()
                                   {
                                       Id = p.AdId,
                                       ImageId = p.ImageId,
                                       ImageUrl = p.ImageUrl,
                                       AdvertisingLink = p.AdvertisingLink,
                                       ExpirationDate = p.ExpirationDate,
                                       User = new ApplicationUser() { Id = p.UserId }
                                   });                                     
        }

        public IEnumerable<Advertising> GetAll()
        {
            _db.Configuration.LazyLoadingEnabled = false;
            return _db.Advertising.Include("User");
        }

        public async Task<Advertising> GetByIDAsync(int id)
        {
            return await _db.Advertising.FindAsync(id);
        }

        public Advertising GetByID(int id)
        {
            return _db.Advertising.Find(id);
        }
 

        public void Remove(Advertising item)
        {
            _db.Advertising.Remove(item);
        }

        public void RemoveByID(int id)
        {
            Advertising ad = _db.Advertising.Find(id);
            if (ad != null)
                _db.Advertising.Remove(ad);
        }

        public async Task RemoveByIDAsync(int id)
        {
            Advertising ad = await _db.Advertising.FindAsync(id);
            if (ad != null)
                _db.Advertising.Remove(ad);
        }

        public void Update(Advertising item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
