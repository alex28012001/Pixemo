using BL.Services.DbAbstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.DTO;
using DAL.UnitOfWork;
using AutoMapper;
using DAL.Entities;
using System;
using System.Linq.Expressions;

namespace BL.Services.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _db;
        public ChatService(IUnitOfWork db)
        {
            _db = db;
        }


        public void CreateChatRoom(string title)
        {
            if (!_db.ChatRooms.AnyWithExpressionTree(p => p.Title.Equals(title)))
            {
                _db.ChatRooms.Create(new ChatRoom() { Title = title });
                _db.Save();
            }
        }

        public async Task CreateChatRoomAsync(string title)
        {
            if (!_db.ChatRooms.AnyWithExpressionTree(p=>p.Title.Equals(title)))
            {
                _db.ChatRooms.Create(new ChatRoom() { Title = title });
                await _db.SaveAsync();
            }  
        }

        public int FindChatRoomIdByTitle(string title)
        {
            return _db.ChatRooms.FindWithExpressionTree(p => p.Title.Equals(title)).FirstOrDefault().Id;
        }


        public void SendMessage(MessageDTO messageDTO)
        {
            var message = GetMapperMessage(messageDTO);
            message.User = _db.ClientManager.FindWithExpressionTree(p => p.UserName.Equals(messageDTO.User.UserName)).FirstOrDefault();
            message.ChatRoom = _db.ChatRooms.GetByID(messageDTO.ChatRoom_Id);
            _db.Messages.Create(message);
            _db.Save();
        }

        public async Task SendMessageAsync(MessageDTO messageDTO)
        {
            var message = GetMapperMessage(messageDTO);
            message.User = _db.ClientManager.FindWithExpressionTree(p => p.UserName.Equals(messageDTO.User.UserName)).FirstOrDefault();
            message.ChatRoom = await _db.ChatRooms.GetByIDAsync(messageDTO.ChatRoom_Id);
            _db.Messages.Create(message);
            await _db.SaveAsync();
        }


        public IEnumerable<MessageDTO> GetMessageByChatRoomIdAndRemove(int chatRoomId, TimeSpan expirationDate)
        {
            RemoveExpirationMessages(chatRoomId, expirationDate);
            _db.Save();
            return GetMapperMessages(chatRoomId);
        }


        public async Task<IEnumerable<MessageDTO>> GetMessageByChatRoomIdAndRemoveAsync(int chatRoomId, TimeSpan expirationDate)
        {
            RemoveExpirationMessages(chatRoomId, expirationDate);
            await _db.SaveAsync();
            return GetMapperMessages(chatRoomId);
        }


        private void RemoveExpirationMessages(int chatRoomId, TimeSpan expirationDate)
        {
            Func<Message, Boolean> predicate = p => p.ChatRoom.Id.Equals(chatRoomId) && p.Date.Add(expirationDate) <= DateTime.Now;
            var expirationMessages = _db.Messages.Find(predicate);
            foreach (var msg in expirationMessages)
            {
                _db.Messages.Remove(msg);
            }
        }


        private IEnumerable<MessageDTO> GetMapperMessages(int chatRoomId)
        {
            var messages = _db.Messages.FindWithExpressionTree(p => p.ChatRoom.Id.Equals(chatRoomId));
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Message, MessageDTO>();
                cfg.CreateMap<ClientProfile, UserDTO>();
            }).CreateMapper();
            return mapper.Map<IEnumerable<Message>, IEnumerable<MessageDTO>>(messages);
        }

        private Message GetMapperMessage(MessageDTO messageDTO)
        { 
            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<MessageDTO, Message>();
                cfg.CreateMap<UserDTO, ClientProfile>();
            }).CreateMapper();
            return mapper.Map<MessageDTO, Message>(messageDTO);
        }
    }
}
