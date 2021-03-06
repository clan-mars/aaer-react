using Application.Activities;
using Application.Core;
using Application.Interfaces;
using Application.Photos;
using Infrastructure.Photos;
using Infrastructure.Security;
using Mediators;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Interfaces;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    { 
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddCors(opt => {
                opt.AddPolicy("CorsPolicy", policy=> {
                    policy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:3000")
                    .AllowCredentials()
                    ;
                });
            });

            services.AddMediatR(typeof(Activities.CreateHandler).Assembly);
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
            services.AddSignalR();

            return services;
        }
    }
}