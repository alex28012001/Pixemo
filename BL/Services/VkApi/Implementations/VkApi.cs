using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Entities.Params;
using BL.Services.VkServices.Factories.Abstraction;
using VkNet.Model;
using BL.Entities.Images;
using System;
using BL.Entities.Videos;
using VkNet.Utils;
using AutoMapper;
using BL.Services.MediaAbstraction;
using BL.Helper;
using VkNet.Model.Attachments;

namespace BL.Services.VkServices.Implementations
{
    public class VkApi : IApi
    {
        private VkNet.VkApi _api;
        private readonly IImagesFactory _imagesFactory;
        private readonly string _accessToken;

        private const long minCountSubscribers = 5000;

        public VkApi(IImagesFactory imagesFactory, string accessToken)
        { 
            _imagesFactory = imagesFactory;
            _accessToken = accessToken;
        }


        public string AccessToken
        {
            get {  return _accessToken;}
        }

        public VkNet.VkApi Api
        {
            get
            {
                if (_api == null)
                {
                    _api = new VkNet.VkApi();
                    _api.Authorize(new ApiAuthParams() { AccessToken = _accessToken});
                }
                return _api;
            }
        }

        public bool IsAuthorized
        {
            get
            {
                return _api.IsAuthorized;
            }
        }

        public async Task<ImageResult> SearchImagesAsync(ImageSearchParams param)
        {
            if(param.OffsetGroups >= 13)
            {
                //отнимаем 1 т.к мы сдвигаем именно картинки по страницам и изначально
                //мы ищем на 1 странице ,поэтому мы отнимаем эту 1
                //умножаем на кол-во картинок(если ищем 10 картинок и мы заходим на 3 стр , то нужно перемножить стр и кол-во картинок)
                //и получаем сдвиг картинок
                var offsetImages = (--param.Offset - 13) * param.Count;

                var imagesVk = await Api.Photo.SearchAsync
                    (new VkNet.Model.RequestParams.PhotoSearchParams()
                    { Query = param.Query, Count = (ulong?)param.Count, Offset = (ulong?)offsetImages });
                return GetMapperImage(imagesVk);
            }

            var searchGroups = _imagesFactory.CreateGetGroups(Api);
            var membersGroups = _imagesFactory.CreateGetMembersByGroup(Api);
            var searchImagesInGroups = _imagesFactory.CreateGetImagesForGroups(Api);

            var groups = await searchGroups.GetAsync(new GroupSearchParams(param.Query) { Count = 4, Offset = param.OffsetGroups });
            for (int i = 0; i < groups.ToCollection().Count; i++)
            {
                if (groups.ToCollection()[i] != null)
                {
                    var info = await membersGroups.GetMembersAsync(groups.ToCollection()[i].Id.ToString());
                    if (info.CountSubscribers >= minCountSubscribers)
                    {
                        var result = await searchImagesInGroups.GetAsync(
                           new ImagesGetAllParams(groups.ToCollection()[i].Id, count: param.Count, offset: param.Offset));
                        if (result.TotalCount > 10)
                        {                                       //+1 т.к индекс начинается с 0, а нам нужен НОМЕР группы
                            result.CurrentNumberGroups = i + 1; //записываем номер группы для того что бы вызывающий код мог вызывать этот метод уже с новым сдвигом
                            return result;
                        }
                    }
                }
            }
            return null;
        }


        public ImageResult SearchImages(ImageSearchParams param)
        {
            if (param.OffsetGroups >= 20)
            {
                var offsetImages = --param.Offset * param.Count;
                var imagesVk = Api.Photo.Search
                    (new VkNet.Model.RequestParams.PhotoSearchParams()
                    { Query = param.Query, Count = (ulong?)param.Count, Offset = (ulong?)offsetImages });
                return GetMapperImage(imagesVk);
            }

            var searchGroups = _imagesFactory.CreateGetGroups(Api);
            var membersGroups = _imagesFactory.CreateGetMembersByGroup(Api);
            var searchImagesInGroups = _imagesFactory.CreateGetImagesForGroups(Api);

            var groups = searchGroups.Get(new GroupSearchParams(param.Query) { Count = 4, Offset = param.OffsetGroups });
            for (int i = 0; i < groups.ToCollection().Count; i++)
            {
                if (groups.ToCollection()[i] != null)
                {
                    var info = membersGroups.GetMembers(groups.ToCollection()[i].Id.ToString());
                    if (info.CountSubscribers >= minCountSubscribers)
                    {
                        var result = searchImagesInGroups.Get(
                           new ImagesGetAllParams(groups.ToCollection()[i].Id, count: param.Count, offset: param.Offset));
                        if (result.TotalCount > 10)
                        {                                       
                            result.CurrentNumberGroups = i + 1; 
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<VideoResult> SearchVideosAsync(VideosSearchParams param)
        {
            var videos = await Api.Video.SearchAsync(new VkNet.Model.RequestParams.VideoSearchParams()
            {Query = param.Query,Count = param.Count,Offset = param.Offset });
            return GetMapperVideos(videos);
        }

        public VideoResult SearchVideos(VideosSearchParams param)
        {
            var videos = Api.Video.Search(new VkNet.Model.RequestParams.VideoSearchParams()
            { Query = param.Query, Count = param.Count, Offset = param.Offset });
            return GetMapperVideos(videos);
        }


        public async Task<Entities.Images.Image> SearchImageByIdAsync(long idImage,long ? idGroup)
        {
            var infoGroup = _imagesFactory.CreateGetInfoGroup(Api);
            string[] id = { (idGroup.ToString() + "_" + idImage.ToString()) };
            var photoVkById = (await Api.Photo.GetByIdAsync(id))[0];
            var imagebyId = GenericMapperHelper<Photo,BL.Entities.Images.Image>.Convert(photoVkById);

            var photoCommentsVk = await Api.Photo.GetCommentsAsync(new VkNet.Model.RequestParams.PhotoGetCommentsParams()
            { OwnerId = idGroup, PhotoId = (ulong)idImage });
            var imageComments = GetMapperComments(photoCommentsVk);


            foreach (var it in imageComments)
            {
                string senderName = null;
                if (it.SenderId.Equals(idGroup))
                {
                    senderName = (await infoGroup.GetInfoAsync((idGroup *-1).ToString())).Name;
                }    
                else
                {
                    long[] senderId = { it.SenderId };
                    var sender = await Api.Users.GetAsync(senderId);
                    var senderInfo = sender.ToCollection()[0];
                    senderName = senderInfo.FirstName + " " + senderInfo.LastName;
                }
                it.SenderName = senderName;
            }
            imagebyId.Comments = imageComments;
            return imagebyId;
        }

        public Entities.Images.Image SearchImageById(long idImage, long? idGroup)
        {
            var infoGroup = _imagesFactory.CreateGetInfoGroup(Api);
            string[] id = { (idGroup.ToString() + "_" + idImage.ToString()) };
            var photoVkById = Api.Photo.GetById(id)[0];
            var imagebyId = GenericMapperHelper<Photo, BL.Entities.Images.Image>.Convert(photoVkById);

            var photoCommentsVk = Api.Photo.GetComments(new VkNet.Model.RequestParams.PhotoGetCommentsParams()
            { OwnerId = idGroup, PhotoId = (ulong)idImage });
            var imageComments = GetMapperComments(photoCommentsVk);


            foreach (var it in imageComments)
            {
                string senderName = null;
                if (it.SenderId.Equals(idGroup))
                {
                    senderName = infoGroup.GetInfo((idGroup * -1).ToString()).Name;
                }
                else
                {
                    long[] senderId = { it.SenderId };
                    var sender = Api.Users.Get(senderId);
                    var senderInfo = sender.ToCollection()[0];
                    senderName = senderInfo.FirstName + " " + senderInfo.LastName;
                }
                it.SenderName = senderName;
            }
            imagebyId.Comments = imageComments;
            return imagebyId;
        }


        private ImageResult GetMapperImage(VkCollection<Photo> images)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VkCollection<Photo>, ImageResult>()
                .ForMember("Images", src => src.MapFrom(p => p.ToCollection()));
                cfg.CreateMap<Photo, Entities.Images.Image>()
                .ForMember("Title", src => src.MapFrom(p => p.Text));
            }).CreateMapper();
            return mapper.Map<VkCollection<Photo>, ImageResult>(images);
        }


        private IEnumerable<Entities.Comments.Comment> GetMapperComments(IEnumerable<Comment> comments)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Comment, Entities.Comments.Comment>()
            .ForMember("SenderId",src=>src.MapFrom(p=>p.FromId))
            ).CreateMapper();
            return mapper.Map<IEnumerable<Comment>, IEnumerable<Entities.Comments.Comment>>(comments);
        }


        private VideoResult GetMapperVideos(VkCollection<VkNet.Model.Attachments.Video> videos)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VkCollection<VkNet.Model.Attachments.Video>, VideoResult>()
                .ForMember("Videos", src => src.MapFrom(p => p.ToCollection()));

                cfg.CreateMap<VkNet.Model.Attachments.Video, Entities.Videos.Video>()
                .ForMember("Url", src => src.MapFrom(p => p.Player))
                .ForMember("SmallPoster", src => src.MapFrom(p => p.Photo130.AbsoluteUri))
                .ForMember("MediumPoster", src => src.MapFrom(p => p.Photo640.AbsoluteUri))
                .ForMember("BigPoster", src => src.MapFrom(p => p.Photo130.AbsoluteUri));
            }).CreateMapper();

            return mapper.Map<VkCollection<VkNet.Model.Attachments.Video>, VideoResult>(videos);
        } 
    }
}
