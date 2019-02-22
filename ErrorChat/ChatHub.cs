using BL.ServiceFactory;
using BL.Services.DbAbstraction;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;

namespace ErrorChat
{
    public class ChatHub:Hub
    {
        private IChatService _chatService;
        public ChatHub(IServiceFactory serviceFactory)
        {
            _chatService = serviceFactory.CreateChatService();
        }

        public async Task Send(string name,string msg,string chatRoomTitle)
        {
            int chatRoomId = _chatService.FindChatRoomIdByTitle(chatRoomTitle);
            var message = new BL.DTO.MessageDTO()
            { ChatRoom_Id = chatRoomId, User = new BL.DTO.UserDTO() { UserName = name }, Text = msg, Date = DateTime.Now };
            await _chatService.SendMessageAsync(message);
            Clients.Others.SendAllClients(name, msg);
        }
    }
}