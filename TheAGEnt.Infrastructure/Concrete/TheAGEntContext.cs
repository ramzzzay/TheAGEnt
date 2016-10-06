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

        public TheAGEntContext Create()
        {
            return new TheAGEntContext();
        }
    }
}