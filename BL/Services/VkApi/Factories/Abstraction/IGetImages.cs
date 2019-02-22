using BL.Entities.Images;
using BL.Entities.Params;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services.VkServices.Factories.Abstraction
{
    public interface IGetImages
    {
        Task<ImageResult> GetAsync(ImagesGetAllParams param);
        ImageResult Get(ImagesGetAllParams param);
    }
}
