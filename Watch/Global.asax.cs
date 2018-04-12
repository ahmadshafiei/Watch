
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Watch.Business;
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

            ConfigAutofac();
        }

        public void ConfigAutofac()
        {
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
            builder.RegisterType<UserManager>().As<UserManager<User,int>>().AsSelf().InstancePerRequest();
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

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
