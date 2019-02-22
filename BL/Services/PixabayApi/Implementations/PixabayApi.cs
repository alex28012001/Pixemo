using System.Threading.Tasks;
using BL.Entities.Images;
using BL.Entities.Params;
using AutoMapper;
using PixabaySharp;
using PixabaySharp.Utility;
using BL.Entities.Videos;
using BL.Services.MediaAbstraction;

namespace BL.Services.PixabayServices.Implementations
{
    public class PixabayApi : IApi
    {
        private PixabaySharpClient _api;
        private readonly string _accessToken;

        public PixabayApi(string accessToken)
        {
            _accessToken = accessToken;
        }

        public string AccessToken
        {
            get { return _accessToken; }
        }

        public bool IsAuthorized
        {
            get
            {
                if (_api != null)
                    return true;
                return false;
            }
        }

        public PixabaySharpClient Api
        {
            get
            {
                if (_api == null)
                    _api = new PixabaySharpClient(_accessToken);
                return _api;
            }
        }


        public async Task<ImageResult> SearchImagesAsync(ImageSearchParams param)
        {
            var images = await Api.QueryImagesAsync(new ImageQueryBuilder()
            { Query = param.Query, PerPage = param.Count, Page = param.Offset });
            return GetMapperImage(images);
        }


        public ImageResult SearchImages(ImageSearchParams param)
        {
            var images = Api.QueryImagesAsync(new ImageQueryBuilder()
            { Query = param.Query, PerPage = param.Count, Page = param.Offset });
            return GetMapperImage(images.Result);
        }

        public async Task<VideoResult> SearchVideosAsync(VideosSearchParams param)
        {
            var videos = await Api.QueryVideosAsync(new VideoQueryBuilder()
            { Query = param.Query, PerPage = param.Count, Page = param.Offset });
            return GetMapperVideos(videos);
        }

        public VideoResult SearchVideos(VideosSearchParams param)
        {
            var videos = Api.QueryVideosAsync(new VideoQueryBuilder()
            { Query = param.Query, PerPage = param.Count, Page = param.Offset });
            return GetMapperVideos(videos.Result);
        }

        public Task<Image> SearchImageByIdAsync(long idImage, long? idGroup)
        {
            return null;
        }

        public Image SearchImageById(long idImage, long? idGroup)
        {
            return null;
        }


        private ImageResult GetMapperImage(PixabaySharp.Models.ImageResult images)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PixabaySharp.Models.ImageResult, ImageResult>()
                .ForMember("TotalCount", src => src.MapFrom(p => p.Total));
                cfg.CreateMap<PixabaySharp.Models.ImageItem, Image>()
                .ForMember(src => src.Comments, p => p.Ignore())
                .ForMember("Title", src => src.MapFrom(p => p.Tags));
            }).CreateMapper();
            return mapper.Map<PixabaySharp.Models.ImageResult, ImageResult>(images);
        }

        private VideoResult GetMapperVideos(PixabaySharp.Models.VideoResult videos)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PixabaySharp.Models.VideoResult, VideoResult>()
                .ForMember("TotalCount", src => src.MapFrom(p => p.Total));
                cfg.CreateMap<PixabaySharp.Models.VideoItem, Video>()
                .ForMember("Width", src => src.MapFrom(p => p.Videos.Medium.Width))
                .ForMember("Height", src => src.MapFrom(p => p.Videos.Medium.Height))
                .ForMember("Url", src => src.MapFrom(p => p.Videos.Medium.Url));
            }).CreateMapper();

            return mapper.Map<PixabaySharp.Models.VideoResult, VideoResult>(videos);
        } 
    }
}
