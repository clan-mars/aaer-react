using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using DTO;

namespace Application.Interfaces
{
    public interface ICommentRepository
    {
        Task<bool> AddComment(Comment comment);
        Task<List<CommentDto>> GetComments(Guid activityId);
    }
}