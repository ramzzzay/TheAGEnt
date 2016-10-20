﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TheAGEnt.Domain.Entities;

namespace TheAGEnt.Domain.Abstract
{
    public interface IMainUserManager
    {
        Task<IdentityResult> ChangePasswordAsync(string email, string currentPassword, string newPassword);
        Task<IdentityResult> CreateAsync(User u);
        Task<List<User>> GetAllUsersAsync();
        Task<IdentityResult> AddClaimToUserAsync(string email, string claimName);
        Task<User> FindAsync(string name, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType);
        Task<User> FindByIdAsync(string userId);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByNickNameAsync(string nickname);
        Task<IdentityResult> AddPasswordAsync(string userId, string newPassword);
        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo userInfo);
        Task<IdentityResult> RemovePasswordAsync(string userId);
        Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo userInfo);
        Task<User> FindAsync(UserLoginInfo userLoginInfo);
        Task<IdentityResult> UpdateAsync(User u);
        IQueryable<User> FindByName(string Name);
    }
}