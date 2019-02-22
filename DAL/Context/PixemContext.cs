using System.Data.Entity;
using DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.Context
{
    public class PixemContext: IdentityDbContext<ApplicationUser>
    {
        public PixemContext()
            : base("DefaultConnection") { }

        public PixemContext(string ConnectionString)
            : base(ConnectionString) { }

       

        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Advertising> Advertising { get; set; }
        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
