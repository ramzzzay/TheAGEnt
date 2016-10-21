using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Owin;
using TheAGEnt.Core.Models;
using TheAGEnt.Domain.Abstract;
using TheAGEnt.Domain.Entities;

namespace TheAGEnt.Core.Controllers
{
    public class RegistrationController : Controller
    {
        private IMainUserManager _userManager;

        public RegistrationController()
        {
        }

        public RegistrationController(IMainUserManager userManager)
        {
            _userManager = userManager;
        }

        private IMainUserManager UserManager
        {
            get
            {
                return _userManager ?? Request
                    .GetOwinContext()
                    .GetAutofacLifetimeScope()
                    .Resolve<IMainUserManager>();
            }
            set { _userManager = value; }
        }

        // GET: UserManagement
        public ActionResult Index(RegisterBindingModel model)
        {
            return View();
        }

        public async Task<ActionResult> Registered(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
                return View();

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

            var response = await UserManager.CreateAsync(user);

            return response.Succeeded ? (ActionResult) RedirectToRoute("Default") : View(model);
        }
    }
}