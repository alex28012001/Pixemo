using BL.Entities.Images;
using BL.Entities.Params;
using System.Threading.Tasks;


namespace BL.Services.MediaAbstraction
{
    public interface IImagesFunctional
    {
        Task<ImageResult> SearchImagesAsync(ImageSearchParams param);
        ImageResult SearchImages(ImageSearchParams param);

        Task<Image> SearchImageByIdAsync(long idImage, long? idGroup);
        Image SearchImageById(long idImage, long? idGroup);
    }
}
