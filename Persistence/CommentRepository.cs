using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{

    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        public CommentRepository(DataContext context, IMapper mapper, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.context = context;

        }
        public async Task<bool> AddComment(Comment comment)
        {
            var activity = await context.Activities.FindAsync(comment.Activity.Id);
            if (activity == null) return false;

            activity.Comments.Add(comment);

            return await context.SaveChangesAsync() > 0;
        }

        public async Task<List<CommentDto>> GetComments(Guid activityId) {
            return await context.Comments.Where(x => x.Activity.Id == activityId).OrderByDescending(x => x.CreatedAt)
            .ProjectTo<CommentDto>(mapper.ConfigurationProvider)
            .ToListAsync();
        }

    }
}