using DAL.UnitOfWork;
using Ninject;
using BL.Services.DbAbstraction;
using BL.Services.MediaAbstraction;
using System;
using BL.Services.Implementations;

namespace BL.ServiceFactory
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IUnitOfWork _db;
        private readonly IApi _pixabayApi;
        private readonly IApi _vkApi;

        public ServiceFactory(IUnitOfWork db, [Named("ImagesFromGroups")]IApi vkApi,
            [Named("QualityImages")]IApi pixabayApi)
        {
            _db = db;
            _pixabayApi = pixabayApi;
            _vkApi = vkApi;
        }
        public IAdvertisingService CreateAdvertisingService()
        {
            return new AdvertisingService(_db);
        }

        public IChatService CreateChatService()
        {
            return new ChatService(_db);
        }

        public ICommentService CreateCommentService()
        {
            return new CommentService(_db);
        }

        public IMediaService CreateMediaService()
        {
            return new PixabayVkService(_vkApi,_pixabayApi);
        }

        public IUserService CreateUserService()
        {
            return new UserService(_db);
        }
    }
}
