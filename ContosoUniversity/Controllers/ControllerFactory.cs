using System;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace ContosoUniversity.Controllers
{
    public class ControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;
        
        public ControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {

            IController controller = _container.Resolve(controllerType) as IController ??
                                     base.GetControllerInstance(requestContext, controllerType);

            if (controller == null)
            {
                requestContext.HttpContext.Response.StatusCode = 404;
                return null;
            }
            return controller;
        }
    }
}