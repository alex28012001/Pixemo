using System.Collections.Generic;
using BL.DTO;
using DAL.Repositories;
using AutoMapper;
using DAL.UnitOfWork;
using DAL.Entities;
using System.Threading.Tasks;
using BL.Services.DbAbstraction;
using BL.Infrastructure;

namespace BL.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _db;
        public CommentService(IUnitOfWork db)
        {
            _db = db;
        }
        public IEnumerable<CommentDTO> FindCommentsByImageId(string imageId)
        {
            var comments = _db.Comments.FindWithExpressionTree(p=>p.ImageID.Equals(imageId)); 
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommentDTO, Comment>();
                cfg.CreateMap<UserDTO, ApplicationUser>();
            }).CreateMapper();
            return mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(comments);
        }


        public async Task<OperationDetails> AddCommentAsync(CommentDTO commentDTO)
        {
            var sender = await _db.UserManager.FindByIdAsync(commentDTO.User.Id);
            if(sender != null)
            {
                Comment comment = GetMapperComment(commentDTO);
                comment.User = sender;

                _db.Comments.Create(comment);
                await _db.SaveAsync();
                return new OperationDetails(true,"Коментарий добавлен","");
            }
            return new OperationDetails(false,"не найден отправитель", "userId");
          
        }


        public OperationDetails AddComment(CommentDTO commentDTO)
        {
            var sender = _db.UserManager.FindByIdAsync(commentDTO.User.Id).Result;
            if (sender != null)
            {
                Comment comment = GetMapperComment(commentDTO);
                comment.User = sender;

                _db.Comments.Create(comment);
                _db.Save();
                return new OperationDetails(true, "Коментарий добавлен", "");
            }
            return new OperationDetails(false, "не найден отправитель", "userId");
        }

        private  Comment GetMapperComment(CommentDTO commentDTO)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommentDTO, Comment>();
                cfg.CreateMap<UserDTO, ApplicationUser>();
            }).CreateMapper();
            return  mapper.Map<CommentDTO, Comment>(commentDTO);
        }
    }
}
