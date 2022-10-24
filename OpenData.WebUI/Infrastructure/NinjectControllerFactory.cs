using Moq;
using Ninject;
using OpenData.Domain.Abstract;
using OpenData.Domain.Concrete;
using OpenData.Domain.Entities;
using OpenData.WebUI.Infrastructure.Abstract;
using OpenData.WebUI.Infrastructure.Concrete;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace OpenData.WebUI.Infrastructure
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
     //Mock<IODRepository> mock = new Mock<IODRepository>();
     //     mock.Setup(m => m.OpenData).Returns(new List<OpenDatum> {
     //     new OpenDataSet { Name = "Football", INN="3525000001" },
     //     new OpenDataSet { Name = "Surf board",INN="3525000002" },
     //     new OpenDataSet { Name = "Running shoes",INN="3525000003" }
     //   }.AsQueryable());


        ninjectKernel.Bind<IODRepository>().To<EFODRepository>();
        ninjectKernel.Bind<IARepository>().To<EFARepository>();
        ninjectKernel.Bind<ICRepository>().To<EFCRepository>();
        ninjectKernel.Bind<IURepository>().To<EFURepository>();
        EmailSettings emailSettings = new EmailSettings
        {
            WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
        };
        ninjectKernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("setting", emailSettings);
        ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
       
    }
  }
}