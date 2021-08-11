using System;
using System.Threading.Tasks;
using Domain;
using Application.Activities;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IActivityRepository
    {
        Task<bool> DeleteActivity(Guid id);
        Task<ActivityDto> GetActivity(Guid id, string activeUsername);
        Task<ActivityDto> GetActivity(Guid id);
        Task<Activity> GetRealActivity(Guid id);
        Task<List<ActivityDto>> ListActivities();
        Task<List<ActivityDto>> ListActivitiesForUser(string activeUsername);
        Task<bool> PlainSave();
        Task<bool> SaveActivity(Activity activity);
        Task<bool> SaveActivity(ActivityDto activityDto);
    }
}