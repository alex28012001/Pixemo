using VkNet.Model;
using VkNet;
using BL.Services.VkServices.Factories.Abstraction;
using BL.Services.VkServices.Factories.Implementations;
using System.Threading.Tasks;
using VkNet.Abstractions;
using BL.Services.VkApi.Factories.Abstraction;
using System;
using BL.Services.VkApi.Factories.Implementations;

namespace BL.Services.Factories.VkServices.Implementations
{
    public class GroupsImagesFactory : IImagesFactory
    {
        public IGetGroups CreateGetGroups(IVkApiCategories api)
        {
            return new GetGroups(api);
        }

        public IGetMembersByGroup CreateGetMembersByGroup(IVkApiCategories api)
        {
            return new GetMembersByGroup(api);
        }

        public IGetImages CreateGetImagesForGroups(IVkApiCategories api)
        {
            return new GetImages(api);
        }


        public IGetInfoGroup CreateGetInfoGroup(IVkApiCategories api)
        {
            return new GetInfoGroup(api);
        }
    }
}
