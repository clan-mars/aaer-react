using System;
using System.Threading.Tasks;
using Domain;
using Application.Activities;
using System.Collections.Generic;

namespace Persistence
{
    public interface IActivityRepository
    {
        Task<bool> DeleteActivity(Activity activity);
        Task<ActivityDto> GetActivity(Guid id, string activeUsername);
        Task<List<ActivityDto>> ListActivities();
        Task<List<ActivityDto>> ListActivitiesForUser(string activeUsername);
        Task<bool> SaveActivity(Activity activity);
    }
}