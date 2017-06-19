using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ContosoUniversity.DAL;
using System.Data.Entity.Infrastructure.Interception;
using ContosoUniversity.Controllers;
using ContosoUniversity.Interfaces;
using ContosoUniversity.Services;
using ContosoUniversity.ViewModels;
using Microsoft.Practices.Unity;

namespace ContosoUniversity
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static UnityContainer Container;
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperConfig.Initialize();
            Container = new UnityContainer();

            Container.RegisterType<ICourseService, CourseService>(new InjectionFactory(c => new CourseService()));
            //note: if a real project, then would add data source info here, which the service would then use rather than the dbcontext which is there now...something like:
            //Container.RegisterType<ICourseService, CourseService>(new InjectionFactory(c => new CourseService(
            //new SomeConnection(someConnectionStringSource))));

            Container.RegisterType<CourseController>();
            Container.RegisterType<CourseViewModel>();
            
            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(Container));

            DbInterception.Add(new SchoolInterceptorTransientErrors());
            DbInterception.Add(new SchoolInterceptorLogging());
        }
    }
}
