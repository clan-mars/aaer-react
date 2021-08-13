using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using DTO;

namespace Application.Comments
{
    public class Add
    {
        private readonly IUserRepository userRepository;
        private readonly IActivityRepository activityRepository;
        private readonly ICommentRepository commentRepository;

        public Add(IUserRepository userRepository, IActivityRepository activityRepository, ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
            this.userRepository = userRepository;
            this.activityRepository = activityRepository;
        }

        public async Task<Comment> AddComment(Guid activityId, string body)
        {
            var activity = await activityRepository.GetRealActivity(activityId);
            if (activity == null) return null;

            var user = await userRepository.GetActiveUserWithPhotos();

            var comment = new Comment
            {
                Author = user,
                Activity = activity,
                Body = body
            };

            var result = await commentRepository.AddComment(comment);
            if (!result) return null;

            return comment;
        }
    }
}