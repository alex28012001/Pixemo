using VkNet.Model.RequestParams;
using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Entities.Images;
using VkNet.Abstractions;
using BL.Entities.Params;
using VkNet.Model.Attachments;
using AutoMapper;
using BL.Services.VkServices.Factories.Abstraction;
using VkNet.Utils;

namespace BL.Services.Factories.VkServices.Implementations
{
    public class GetImages : IGetImages
    {
        private readonly IVkApiCategories _api;
        public GetImages(IVkApiCategories api)
        {
            _api = api;
        }

        public async Task<ImageResult> GetAsync(ImagesGetAllParams param)
        {
            var images = await _api.Photo.GetAllAsync(new PhotoGetAllParams()
            { OwnerId = param.Id * -1, Count = (ulong)param.Count , Offset = (ulong ?)param.Offset });
            // *-1 это нужно для vk api , он требует,чтобы id сообщества начинались с -

            return GetMapperImage(images);
        }

        public ImageResult Get(ImagesGetAllParams param)
        {
            var images = _api.Photo.GetAll(new PhotoGetAllParams()
            { OwnerId = param.Id, Count = (ulong)param.Count, Offset = (ulong ?)param.Offset });

            return GetMapperImage(images);
        }


        private ImageResult GetMapperImage(VkCollection<Photo> images)
        {
            var mapper = new MapperConfiguration(cfg =>
            { 
                cfg.CreateMap<VkCollection<Photo>, ImageResult>()
                .ForMember("Images", src=>src.MapFrom(p=>p.ToCollection()));
                cfg.CreateMap<Photo, Image>()
                .ForMember("Title", src => src.MapFrom(p => p.Text));
            }).CreateMapper();
            return mapper.Map<VkCollection<Photo>, ImageResult>(images);
        }
    }
}
