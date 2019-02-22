using DAL.Context;
using DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CommentsRepository : IRepository<Comment>
    { 
        private PixemContext _db;
        public CommentsRepository(IdentityDbContext<ApplicationUser> db)
        {
            _db = (PixemContext)db;
        }
        public void Create(Comment item)
        {
            _db.Comments.Add(item);
        }

        public int CountWithExpressionTree(Expression<Func<Comment, Boolean>> predicate)
        {
            return _db.Comments.Where(predicate).Count();
        }

        public void RemoveByID(int id)
        {
            Comment comment = _db.Comments.Find(id);
            if (comment != null)
                _db.Comments.Remove(comment);
        }

        public async Task RemoveByIDAsync(int id)
        {
            Comment comment = await _db.Comments.FindAsync(id);
            if (comment != null)
                _db.Comments.Remove(comment);
        }

        public void Remove(Comment item)
        {
            _db.Comments.Remove(item);
        }

        public bool Any()
        {
            return _db.Comments.Any();
        }

        public bool Any(Func<Comment, bool> predicate)
        {
            return _db.Comments.Any(predicate);
        }

        public bool AnyWithExpressionTree(Expression<Func<Comment, bool>> predicate)
        {
            return _db.Comments.Any(predicate);
        }

        public void Update(Comment item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public Comment GetByID(int id)
        {
            return _db.Comments.Find(id);
        }

        public async Task<Comment> GetByIDAsync(int id)
        {
            return await _db.Comments.FindAsync(id);
        }

        public IEnumerable<Comment> Find(Func<Comment, Boolean> predicate)
        {
            return _db.Comments.Where(predicate);
        }

        public IEnumerable<Comment> FindWithExpressionTree(Expression<Func<Comment, bool>> predicate)
        {
            _db.Configuration.LazyLoadingEnabled = false;
            return _db.Comments.Where(predicate)
                                .Select(p => new
                                {
                                    CommentID = p.CommentID,
                                    Date = p.Date,
                                    Text = p.Text,
                                    ImageID = p.ImageID,
                                    UserName = p.User.UserName
                                }).AsEnumerable()
                                .Select(p => new Comment()
                                {
                                    CommentID = p.CommentID,
                                    ImageID = p.ImageID,
                                    Date = p.Date,
                                    Text = p.Text,
                                    User = new ApplicationUser() { UserName = p.UserName }
                                });
        }

        public IEnumerable<Comment> GetAll()
        {
            _db.Configuration.LazyLoadingEnabled = false;
            return _db.Comments.Include("User");
        }
    }
}
