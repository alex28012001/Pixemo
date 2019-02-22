using Ninject.Modules;
using Ninject.Web.Common;
using BL.ServiceFactory;

namespace ErrorChat.NinjectModules
{
    public class SevicesModule:NinjectModule
    {
        public override void Load()
        {
            Bind<IServiceFactory>().To<ServiceFactory>().InRequestScope();
        }
    }
}