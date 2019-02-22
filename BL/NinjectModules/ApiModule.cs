using BL.Services.MediaAbstraction;
using BL.Services.PixabayServices.Implementations;
using BL.Services.VkServices.Implementations;
using Ninject.Modules;
using Ninject.Web.Common;
using System.Configuration;

namespace BL.NinjectModules
{
    public class ApiModule : NinjectModule
    {
        public override void Load()
        {
            AppSettingsReader reader = new AppSettingsReader();
            string vkAccessToken = (string)reader.GetValue("vkAccessToken", typeof(string));
            string pixabayAccessToken = (string)reader.GetValue("pixabayAccessToken", typeof(string));

            Bind<IApi>().To<VkApi>().InRequestScope().Named("ImagesFromGroups")
              .WithConstructorArgument("accessToken", vkAccessToken);

            Bind<IApi>().To<PixabayApi>().InRequestScope().Named("QualityImages")
                .WithConstructorArgument("accessToken", pixabayAccessToken);
        }
    }
}

