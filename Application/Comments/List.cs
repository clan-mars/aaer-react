using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using DTO;

namespace Application.Comments
{
    public class List
    {
        public ICommentRepository CommentRepository { get; }
        public List(ICommentRepository commentRepository)
        {
            this.CommentRepository = commentRepository;

        }
        public async Task<List<CommentDto>> GetCommments(Guid activityId)
        {
            return await CommentRepository.GetComments(activityId);
        }
    }
}