using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using TheAGEnt.Domain.Entities;

namespace TheAGEnt.Infrastructure.Concrete
{
    public class TheAGEntContext : IdentityDbContext<User>
    {
        public TheAGEntContext() : base("TheAGEntContext", true)
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        public TheAGEntContext Create()
        {
            return new TheAGEntContext();
        }
    }
}