using BL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.DbAbstraction
{
    public interface IChatService
    {
        void CreateChatRoom(string title);
        Task CreateChatRoomAsync(string title);
        void SendMessage(MessageDTO message);
        Task SendMessageAsync(MessageDTO message);
        IEnumerable<MessageDTO> GetMessageByChatRoomIdAndRemove(int chatRoomId, TimeSpan expirationDate);
        Task<IEnumerable<MessageDTO>> GetMessageByChatRoomIdAndRemoveAsync(int chatRoomId, TimeSpan expirationDate);
        int FindChatRoomIdByTitle(string title);
    }
}
