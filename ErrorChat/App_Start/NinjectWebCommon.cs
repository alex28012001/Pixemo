using BL.NinjectModules;
using ErrorChat.App_Start;
using ErrorChat.NinjectModules;
using Microsoft.AspNet.SignalR;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using Ninject.Web.Mvc;
using System;
using System.Web;
using System.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
 [assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace ErrorChat.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            //сопоставления зависимостей!!!
            var kernel = new StandardKernel(
                new DbModule(),new FactoryModule(),new ApiModule(), new SevicesModule());
                 
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            //Для разпознавания зависимостей signaIR
            GlobalHost.DependencyResolver = new NinjectSignalRDependencyResolver(kernel);

            RegisterServices(kernel);
            return kernel;
        }
        private static void RegisterServices(IKernel kernel)
        {
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
