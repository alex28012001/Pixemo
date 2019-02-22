using BL.Entities.Params;
using BL.Entities.Videos;
using System.Threading.Tasks;

namespace BL.Services.MediaAbstraction
{
    public interface IVideosFunctional
    {
        Task<VideoResult> SearchVideosAsync(VideosSearchParams param);
        VideoResult SearchVideos(VideosSearchParams param);
    }
}
