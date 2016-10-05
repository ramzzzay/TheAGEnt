﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using TheAGEnt.Domain.Abstract;

namespace TheAGEnt.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(IMainUserManager manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }
}