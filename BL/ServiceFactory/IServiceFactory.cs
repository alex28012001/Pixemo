using BL.Services.DbAbstraction;
using BL.Services.MediaAbstraction;

namespace BL.ServiceFactory
{
    public interface IServiceFactory
    {
        IMediaService CreateMediaService();
        IUserService CreateUserService();
        ICommentService CreateCommentService();
        IChatService CreateChatService();
        IAdvertisingService CreateAdvertisingService();
    }
}
