using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin.Security.AesDataProtectorProvider;
using Owin.Security.AesDataProtectorProvider.CrypticProviders;
using TheAGEnt.Domain.Entities;

namespace TheAGEnt.Infrastructure.Infrastructure
{
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store)
            : base(store)
        {
            UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireDigit = true
            };

            UserLockoutEnabledByDefault = false;

            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = new AesDataProtectorProvider(
                new Sha512CspFactory(),
                new Sha256CspFactory(),
                new AesCspFactory()
                );
            UserTokenProvider = new DataProtectorTokenProvider<User>(
                dataProtectionProvider
                .Create("ASP.NET Identity")
                );
        }
    }
}