using BL.DTO;
using BL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.DbAbstraction
{
    public interface ICommentService
    {
        IEnumerable<CommentDTO> FindCommentsByImageId(string imageId);
        OperationDetails AddComment(CommentDTO comment);
        Task<OperationDetails> AddCommentAsync(CommentDTO comment); 
    }
}
