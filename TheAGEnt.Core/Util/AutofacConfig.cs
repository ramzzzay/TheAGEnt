using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using TheAGEnt.Core.Controllers;
using TheAGEnt.Domain.Abstract;
using TheAGEnt.Domain.Entities;
using TheAGEnt.Infrastructure.Concrete;
using TheAGEnt.Infrastructure.Infrastructure;

namespace TheAGEnt.Core.Util
{
    public class AutofacConfig
    {
        public static void Configure(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            var config = new HttpConfiguration();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<AccountController>().InstancePerRequest();

            //dependenses resolving
            builder.RegisterType<MainUserManager>().As<IMainUserManager>().InstancePerRequest();
            //builder.Register(c => new UserStore<User>(c.Resolve<TheAGEntContext>())).AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterType<UserStore<User>>().As<IUserStore<User>>().WithParameter("context", new TheAGEntContext());

            builder.RegisterType<ApplicationUserManager>().AsSelf();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            app.UseAutofacWebApi(config);
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }

        public static void Configure()
        {
            //Autofac configuration
            var builder = new ContainerBuilder();
            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<AccountController>().InstancePerRequest();

            //dependenses resolving
            builder.RegisterType<MainUserManager>().As<IMainUserManager>().InstancePerRequest();
            builder.RegisterType<UserStore<User>>().As<IUserStore<User>>().WithParameter("context", new TheAGEntContext());


            builder.RegisterType<ApplicationUserManager>().AsSelf();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}