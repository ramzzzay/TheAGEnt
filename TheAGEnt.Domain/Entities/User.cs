using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using TheAGEnt.Domain.Abstract;

namespace TheAGEnt.Domain.Entities
{
    public class User : IdentityUser
    {

        public User()
        {
            Albums = new List<Album>();
            Comments = new List<Comment>();
        }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NickName { get; set; }
        public string Adress { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(IMainUserManager manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }
}