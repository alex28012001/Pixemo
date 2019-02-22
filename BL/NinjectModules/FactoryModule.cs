using BL.Services.Factories.VkServices.Implementations;
using BL.Services.VkServices.Factories.Abstraction;
using Ninject.Modules;
using Ninject.Web.Common;
using VkNet;
using VkNet.Abstractions;

namespace BL.NinjectModules
{
    public class FactoryModule:NinjectModule
    {
        public override void Load()
        { 
            Bind<IImagesFactory>().To<GroupsImagesFactory>().InRequestScope();   
        }
    }
}
