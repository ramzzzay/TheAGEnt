using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Owin;
using TheAGEnt.Core.Models;
using TheAGEnt.Domain.Abstract;

namespace TheAGEnt.Core.Controllers
{
    public class EditingInfoController : Controller
    {
        private IMainUserManager _userManager;

        public EditingInfoController()
        {
        }

        public EditingInfoController(IMainUserManager userManager)
        {
            _userManager = userManager;
        }

        private IMainUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetAutofacLifetimeScope().Resolve<IMainUserManager>();
            }
            set { _userManager = value; }
        }

        // GET: EditingInfo
        public async Task<ActionResult> Index(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            var viewUser = new PersonalUserInfoViewModel
            {
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                NickName = user.NickName,
                Address = user.Address
            };
            return View(viewUser);
        }

        public async Task<ActionResult> Edited(PersonalUserInfoViewModel updatedUser)
        {
            if (!ModelState.IsValid) return View();
            var user = await UserManager.FindByEmailAsync(updatedUser.Email);
            if (updatedUser.Name != null)
                user.Name = updatedUser.Name;
            if (updatedUser.Surname != null)
                user.Surname = updatedUser.Surname;
            if (updatedUser.NickName != null)
                user.NickName = updatedUser.NickName;
            if (updatedUser.Address != null)
                user.Address = updatedUser.Address;
            var response = await UserManager.UpdateAsync(user);
            return response.Succeeded ? (ActionResult) RedirectToRoute("Default") : View(updatedUser);
        }
    }
}