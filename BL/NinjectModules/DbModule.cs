using DAL.UnitOfWork;
using Ninject.Modules;
using Ninject.Web.Common;


namespace BL.NinjectModules
{
    public class DbModule:NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<IdentityUnitOfWork>().InRequestScope();
        }
    }
}
