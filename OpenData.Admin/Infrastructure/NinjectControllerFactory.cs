using Ninject;
using OpenData.Domain.Abstract;
using OpenData.Domain.Concrete;
using OpenData.Domain.Entities;
using OpenData.Admin.Infrastructure.Abstract;
using OpenData.Admin.Infrastructure.Concrete;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace OpenData.Admin.Infrastructure
{
   public class NinjectControllerFactory : DefaultControllerFactory
  {
    private IKernel ninjectKernel;
    public NinjectControllerFactory()
    {
      ninjectKernel = new StandardKernel();
      AddBindings();
    }

    protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
    {
      return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
    }
    private void AddBindings()
    {

        ninjectKernel.Bind<IURepository>().To<EFURepository>();
        ninjectKernel.Bind<IODRepository>().To<EFODRepository>();
        ninjectKernel.Bind<IARepository>().To<EFARepository>();
        ninjectKernel.Bind<ICRepository>().To<EFCRepository>();
        EmailSettings emailSettings = new EmailSettings
        {
            WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
        };
        ninjectKernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("setting", emailSettings);
        ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();

        ninjectKernel.Inject(Membership.Provider);
        ninjectKernel.Inject(Roles.Provider);
       
    }
  }
}