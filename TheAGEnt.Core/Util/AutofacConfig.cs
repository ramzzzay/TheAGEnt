﻿using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
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

            builder.RegisterControllers(typeof(WebApiApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<AccountController>().InstancePerRequest();
            builder.RegisterType<PhotosController>().InstancePerRequest();
            builder.RegisterType<EditingInfoController>().InstancePerRequest();
            builder.RegisterType<RegistrationController>().InstancePerRequest();
            builder.RegisterType<MainUserManager>().As<IMainUserManager>().InstancePerRequest();
            builder.RegisterType<PhotoManager>().As<IPhotoManager>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf();
            builder.RegisterType<TheAGEntContext>().AsSelf();

            builder.Register(c => new UserStore<User>(c.Resolve<TheAGEntContext>())).AsImplementedInterfaces().InstancePerRequest();



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
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterControllers(typeof(WebApiApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<AccountController>().InstancePerRequest();
            builder.RegisterType<PhotosController>().InstancePerRequest();
            builder.RegisterType<EditingInfoController>().InstancePerRequest();
            builder.RegisterType<RegistrationController>().InstancePerRequest();
            builder.RegisterType<MainUserManager>().As<IMainUserManager>().InstancePerRequest();
            builder.RegisterType<PhotoManager>().As<IPhotoManager>().InstancePerRequest();
            builder.RegisterType<UserStore<User>>().As<IUserStore<User>>().WithParameter("context", new TheAGEntContext());
            builder.RegisterType<ApplicationUserManager>().AsSelf();
            builder.RegisterType<TheAGEntContext>().AsSelf();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}