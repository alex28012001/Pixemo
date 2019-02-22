using BL.Services.VkApi.Factories.Abstraction;
using VkNet.Abstractions;

namespace BL.Services.VkServices.Factories.Abstraction
{
    public interface IImagesFactory
    {
        IGetGroups CreateGetGroups(IVkApiCategories api);
        IGetMembersByGroup CreateGetMembersByGroup(IVkApiCategories api);
        IGetInfoGroup CreateGetInfoGroup(IVkApiCategories api);
        IGetImages CreateGetImagesForGroups(IVkApiCategories api);
    }
}
