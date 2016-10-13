using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using TheAGEnt.Core.Models;
using TheAGEnt.Core.Util;
using TheAGEnt.Domain.Abstract;
using TheAGEnt.Domain.Entities;

namespace TheAGEnt.Core.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private IMainUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(IMainUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        private IMainUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetAutofacLifetimeScope().Resolve<IMainUserManager>();
            }
            set { _userManager = value; }
        }

        private ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        [Authorize(Roles = "user")]
        public AccountViewModels.UserInfoViewModel GetUserInfo()
        {
            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new AccountViewModels.UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin?.LoginProvider
            };
        }

        // GET api/Account/AllUserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("AllUserInfo")]
        [Authorize(Roles = "user")]
        public async Task<PersonalUserInfoViewModel> GetAllUserInfo()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var viewUser = new PersonalUserInfoViewModel
            {
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                NickName = user.NickName,
                Claims = user.Claims,
                PathToPhoto = user.PathToPhoto,
                PathToCard = user.PathToCard
            };
            return viewUser;
        }


        // GET api/Account/SearchUsers
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("SearchUsers")]
        [HttpGet]
        [AllowAnonymous]
        public IQueryable SearchUsers(string searchData)
        {
            var users = UserManager.FindByName(searchData);
            var viewsUser = users.Select(user => new
            {
                user.Email,
                user.Name,
                user.Surname,
                user.NickName
            });

            return viewsUser;
        }

        // GET api/Account/GetAllUsersMiniInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetAllUsersMiniInfo")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<MiniUserInfoViewModer>> GetAllUsersMiniInfo()
        {
            var users = await UserManager.GetAllUsersAsync();
            var viewsUser = users.Select(user => new MiniUserInfoViewModer()
            {
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                NickName = user.NickName,
                PathToPhoto = user.PathToPhoto
            });

            return viewsUser;
        }

        // POST api/Account/UpdateAllUserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UpdateUserInfo")]
        [Authorize(Roles = "user")]
        public async Task<IHttpActionResult> UpdateUserInfo(PersonalUserInfoViewModel updatedUser)
        {
            if (!ModelState.IsValid) return BadRequest("Wrong model");
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (updatedUser.Name != "")
                user.Name = updatedUser.Name;
            if (updatedUser.Surname != "")
                user.Surname = updatedUser.Surname;
            if (updatedUser.NickName != "")
                user.NickName = updatedUser.NickName;
            if (updatedUser.Address != "")
                user.Address = updatedUser.Address;
            var response = await UserManager.UpdateAsync(user);
            return response.Succeeded
                ? Ok(new {Msg = response.Errors, IsOk = response.Succeeded})
                : GetErrorResult(response);
        }

        // POST api/Account/UpdateAllUserInfoByAdmin
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UpdateUserInfoByAdmin")]
        [Authorize(Roles = "admin")]
        public async Task<IHttpActionResult> UpdateUserInfoByAdmin(PersonalUserInfoViewModel updatedUser)
        {
            if (!ModelState.IsValid) return BadRequest("Wrong model");
            var user = await UserManager.FindByEmailAsync(updatedUser.Email);
            if (updatedUser.Email != "")
                user.Email = updatedUser.Email;
            if (updatedUser.Name != "")
                user.Name = updatedUser.Name;
            if (updatedUser.Surname != "")
                user.Surname = updatedUser.Surname;
            if (updatedUser.NickName != "")
                user.NickName = updatedUser.NickName;
            if (updatedUser.Address != "")
                user.Address = updatedUser.Address;
            var response = await UserManager.UpdateAsync(user);
            return response.Succeeded
                ? Ok(new {Msg = response.Errors, IsOk = response.Succeeded})
                : GetErrorResult(response);
        }


        [Route("UploadUserAvatar")]
        // POST api/Account/UploadUserAvatar
        [MimeMultipart]
        [Authorize(Roles = "user")]
        public async Task<IHttpActionResult> UploadUserAvatar()
        {
            var uploadPath = HttpContext.Current.Server.MapPath("~/Assets/imgs/ProfileImages/Avatars");

            var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

            // Read the MIME multipart asynchronously 
            await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

            var localFileName = multipartFormDataStreamProvider
                .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();
            var fileName = Path.GetFileName(localFileName);

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            user.PathToPhoto = $"/Assets/imgs/ProfileImages/Avatars/{fileName}";

            var response = await UserManager.UpdateAsync(user);
            return response.Succeeded
                ? Ok(new {Msg = response.Errors, IsOk = response.Succeeded, uploadedUrl = user.PathToPhoto})
                : GetErrorResult(response);
        }

        [Route("UploadUserCard")]
        // POST api/Account/UploadUserCard
        [MimeMultipart]
        [Authorize(Roles = "user")]
        public async Task<IHttpActionResult> UploadUserCard()
        {
            var uploadPath = HttpContext.Current.Server.MapPath("~/Assets/imgs/ProfileImages/Cards");

            var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

            // Read the MIME multipart asynchronously 
            await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

            var localFileName = multipartFormDataStreamProvider
                .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();
            var fileName = Path.GetFileName(localFileName);

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            user.PathToCard = $"/Assets/imgs/ProfileImages/Cards/{fileName}";

            var response = await UserManager.UpdateAsync(user);
            return response.Succeeded
                ? Ok(new {Msg = response.Errors, IsOk = response.Succeeded, uploadedUrl = user.PathToCard})
                : GetErrorResult(response);
        }


        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<AccountViewModels.ManageInfoViewModel> GetManageInfo(string returnUrl,
            bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
                return null;

            var logins = user.Logins.Select(linkedAccount => new UserLoginInfoViewModel
            {
                LoginProvider = linkedAccount.LoginProvider,
                ProviderKey = linkedAccount.ProviderKey
            }).ToList();

            if (user.PasswordHash != null)
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName
                });

            return new AccountViewModels.ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize(Roles = "user")]
        //public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            return !result.Succeeded ? GetErrorResult(result) : Ok(result);
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            return !result.Succeeded ? GetErrorResult(result) : Ok();
        }

        // POST api/Account/AddClaim
        [Route("AddClaim")]
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IHttpActionResult> AddClaim(ClaimAddModel c)
        {
            if (!ModelState.IsValid) return BadRequest("Something frong with adding claim to user!");
            var response = await UserManager.AddClaimToUserAsync(c.Email, c.NameOfClaim);
            return Ok(new {Msg = response.Errors, IsOk = response.Succeeded});
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                UserName = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                Address = model.Address,
                NickName = model.NickName,
                Email = model.Email,
                EmailConfirmed = false,
                Password = model.Password
            };

            var result = await UserManager.CreateAsync(user);

            //if (!result.Succeeded) return GetErrorResult(result);
            //var client = new SmtpClient("smtp.gmail.com", 587)
            //{
            //    EnableSsl = true,
            //    Timeout = 10000,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential("phonebooksender@gmail.com", "Qw12345678*")
            //};

            //var email = Email
            //    .From("phonebooksender@gmail.com")
            //    .To(user.Email)
            //    .Subject("Email confirmation")
            //    .Body(string.Format("For complete the registration, please go to link:" +
            //                        "<a href=\"{0}\" title=\"Accept\">{0}</a>",
            //        $"http://localhost/api/Account/ConfirmEmail?token={user.Email}"))
            //    .UsingClient(client);
            //email.Send();

            return Ok(new {Msg = result.Errors, IsOk = result.Succeeded});
        }

        [AllowAnonymous]
        [Route("ConfirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmail(string token)
        {
            var user = await UserManager.FindByEmailAsync(token);
            if (user == null) return BadRequest("Bad Token");
            user.EmailConfirmed = true;
            await UserManager.UpdateAsync(user);
            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<AccountViewModels.ExternalLoginViewModel> GetExternalLogins(string returnUrl,
            bool generateState = false)
        {
            var descriptions = Authentication.GetExternalAuthenticationTypes();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            return descriptions.Select(description => new AccountViewModels.ExternalLoginViewModel
            {
                Name = description.Caption,
                Url = Url.Route("ExternalLogin", new
                {
                    provider = description.AuthenticationType,
                    response_type = "token",
                    client_id = Startup.PublicClientId,
                    redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                    state
                }),
                State = state
            }).ToList();
        }

        #region Helpers

        private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
                return InternalServerError();

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error);

                if (ModelState.IsValid)
                    return BadRequest();

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; private set; }
            public string ProviderKey { get; private set; }
            public string UserName { get; set; }

            public IEnumerable<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                var providerKeyClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(providerKeyClaim?.Issuer) || string.IsNullOrEmpty(providerKeyClaim.Value))
                    return null;

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                    return null;

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static readonly RandomNumberGenerator Random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits%bitsPerByte != 0)
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", nameof(strengthInBits));

                var strengthInBytes = strengthInBits/bitsPerByte;

                var data = new byte[strengthInBytes];
                Random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}