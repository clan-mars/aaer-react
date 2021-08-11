using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Photos;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Mediators
{
    public class Photos
    {
        public class Add : IRequest<Result<Photo>>
        {
            public IFormFile File { get; set; }
        }

        public class AddHandler : IRequestHandler<Add, Result<Photo>>
        {
            private readonly IPhotoRepository photoRepository;
            private readonly IUserRepository userRepository;
            private readonly Application.Photos.IPhotoAccessor photoAccessor;

            public AddHandler(IPhotoRepository photoRepository, IUserRepository userRepository, Application.Photos.IPhotoAccessor photoAccessor)
            {
                this.photoRepository = photoRepository;
                this.userRepository = userRepository;
                this.photoAccessor = photoAccessor;
            }

            public async Task<Result<Photo>> Handle(Add request, CancellationToken cancellationToken)
            {
                Photo result = null;
                try
                {
                    result = await new Application.Photos.Add(photoRepository, userRepository, photoAccessor)
                    .PerformAdd(request.File);
                    if (result == null) return Result<Photo>.Failure("Photo could not be added");
                }
                catch (Exception e)
                {
                    return Result<Photo>.Failure(e.Message);
                }

                return Result<Photo>.Success(result);
            }
        }

        public class Delete : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class DeleteHandler : IRequestHandler<Delete, Result<Unit>>
        {
            private readonly IUserRepository userRepository;
            private readonly IPhotoAccessor photoAccessor;

            public DeleteHandler(IUserRepository userRepository, IPhotoAccessor photoAccessor)
            {
                this.userRepository = userRepository;
                this.photoAccessor = photoAccessor;
            }

            public async Task<Result<Unit>> Handle(Delete request, CancellationToken cancellationToken)
            {
                var result = await new Application.Photos.Delete(photoAccessor, userRepository).PerformDelete(request.Id);
                return result? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Could not delete image");
            }
        }

        public class SetMain : IRequest<Result<Unit>> {
            public string Id { get; set; }
        }

        public class SetMainHandler : IRequestHandler<SetMain, Result<Unit>>
        {
            private readonly IUserRepository userRepository;

            public SetMainHandler(IUserRepository userRepository)
            {
                this.userRepository = userRepository;
            }

            public async Task<Result<Unit>> Handle(SetMain request, CancellationToken cancellationToken)
            {
                var result = await new Application.Photos.SetMain(userRepository).PerformSetMain(request.Id);
                return result? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to set photo as main");
            }
        }
    }
}