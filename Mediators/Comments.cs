using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using DTO;
using MediatR;

namespace Mediators
{
    public class Comments
    {
        public class Add : IRequest<Result<CommentDto>>
        {
            public string Body { get; set; }
            public Guid ActivityId { get; set; }
        }

        public class AddHandler : IRequestHandler<Add, Result<CommentDto>>
        {
            private readonly IUserRepository userRepository;
            private readonly IActivityRepository activityRepository;
            private readonly ICommentRepository commentRepository;
            private readonly IMapper mapper;

            public AddHandler(IUserRepository userRepository, IActivityRepository activityRepository, ICommentRepository commentRepository, IMapper mapper)
            {
                this.userRepository = userRepository;
                this.activityRepository = activityRepository;
                this.commentRepository = commentRepository;
                this.mapper = mapper;
            }
            async Task<Result<CommentDto>> IRequestHandler<Add, Result<CommentDto>>.Handle(Add request, CancellationToken cancellationToken)
            {
                var newComment = await new Application.Comments.Add(userRepository, activityRepository, commentRepository).AddComment(request.ActivityId, request.Body);
                if (newComment == null)
                {
                    return Result<CommentDto>.Failure("Failed to add comment");
                }

                return Result<CommentDto>.Success(mapper.Map<CommentDto>(newComment));
            }
        }

        public class ListComments : IRequest<Result<List<CommentDto>>> {
            public Guid ActivityId { get; set; }
        }

        public class ListHandler : IRequestHandler<ListComments, Result<List<CommentDto>>>
        {
            private readonly ICommentRepository commentRepository;

            public ListHandler(ICommentRepository commentRepository)
            {
                this.commentRepository = commentRepository;
            }
            public async Task<Result<List<CommentDto>>> Handle(ListComments request, CancellationToken cancellationToken)
            {
                var result = await new Application.Comments.List(commentRepository).GetCommments(request.ActivityId);
                if (result == null) return Result<List<CommentDto>>.Failure("Failed to get list of comments");

                return Result<List<CommentDto>>.Success(result);
            }
        }
    }
}