using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TheAGEnt.Core.Models
{
    public class AccountViewModels
    {

        public class ExternalLoginViewModel
        {
            public string Name { get; set; }

            public string Url { get; set; }

            public string State { get; set; }
        }

        public class ManageInfoViewModel
        {
            public string LocalLoginProvider { get; set; }

            public string Email { get; set; }

            public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

            public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
        }

        public class UserInfoViewModel
        {
            public string Email { get; set; }

            public bool HasRegistered { get; set; }

            public string LoginProvider { get; set; }
        }
    }

    public class PersonalUserInfoViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NickName { get; set; }
        public string Address { get; set; }
        public string PathToPhoto { get; set; }
        public string PathToCard { get; set; }
        public ICollection<IdentityUserClaim> Claims { get; set; }
    }

    public class MiniUserInfoViewModer
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NickName { get; set; }
        public string PathToPhoto { get; set; }
        public string PathToCard { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}