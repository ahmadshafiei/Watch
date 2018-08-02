
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Watch.Business;
using Watch.Controllers;
using Watch.DataAccess;
using Watch.DataAccess.Identity;
using Watch.DataAccess.Repositories;
using Watch.DataAccess.UnitOfWork;
using Watch.Models;

namespace Watch
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configuration.Formatters.Clear();
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Re‌​ferenceLoopHandling = ReferenceLoopHandling.Ignore;

            ConfigAutofac();
        }

        public void ConfigAutofac()
        {
            #region [Web Api Resolver]
            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;

            builder.RegisterType<AuthenticationBusiness>().InstancePerRequest();
            builder.RegisterType<WatchBusiness>().InstancePerRequest();
            builder.RegisterType<RequestBusiness>().InstancePerRequest();
            builder.RegisterType<ProfileBusiness>().InstancePerRequest();
            builder.RegisterType<BookMarkBusiness>().InstancePerRequest();
            builder.RegisterType<StoreBusiness>().InstancePerRequest();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().AsSelf().InstancePerRequest();
            builder.RegisterType<WatchContext>().As<DbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<UserManager>().As<UserManager<User, int>>().AsSelf().InstancePerRequest();
            builder.RegisterType<UserStore>().As<IUserStore<User, int>>().AsSelf().InstancePerRequest();


            builder.RegisterType<UserRepository>().InstancePerRequest();
            builder.RegisterType<WatchRepository>().InstancePerRequest();
            builder.RegisterType<SellerRepository>().InstancePerRequest();
            builder.RegisterType<RoleRepository>().InstancePerRequest();
            builder.RegisterType<RequestRepository>().InstancePerRequest();
            builder.RegisterType<ImageRepository>().InstancePerRequest();
            builder.RegisterType<WatchBookmarkRepository>().InstancePerRequest();
            builder.RegisterType<BrandRepository>().InstancePerRequest();
            builder.RegisterType<SuggestPriceRepository>().InstancePerRequest();
            builder.RegisterType<UserRoleRepository>().InstancePerRequest();
            builder.RegisterType<StoreBookmarkRepository>().InstancePerRequest();
            builder.RegisterType<RoleBusiness>().InstancePerRequest();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterControllers(typeof(HomeController).Assembly);

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            #endregion

            #region [MVC Respolver]

            var builderMvc = new ContainerBuilder();

            builderMvc.RegisterControllers(typeof(HomeController).Assembly);

            builderMvc.RegisterType<UserRoleRepository>().InstancePerRequest();
            builderMvc.RegisterType<UserRepository>().InstancePerRequest();
            builderMvc.RegisterType<RoleRepository>().InstancePerRequest();
            builderMvc.RegisterType<UnitOfWork>().As<IUnitOfWork>().AsSelf().InstancePerRequest();
            builderMvc.RegisterType<WatchContext>().As<DbContext>().AsSelf().InstancePerRequest();
            builderMvc.RegisterType<UserStore>().As<IUserStore<User, int>>().AsSelf().InstancePerRequest();
            builderMvc.RegisterType<UserManager>().As<UserManager<User, int>>().AsSelf().InstancePerRequest();
            builderMvc.RegisterType<SignInManager<User, int>>().InstancePerRequest();

            var containerMvc = builderMvc.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(containerMvc));
            #endregion

        }
    }
}
