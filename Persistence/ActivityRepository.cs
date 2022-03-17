using System;
using System.Threading.Tasks;
using Domain;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Application.Activities;
using System.Linq;
using System.Collections.Generic;
using Application.Interfaces;
using Persistence.Interfaces;
using Application.Core;

namespace Persistence
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly IUserAccessor userAccessor;

        public ActivityRepository(DataContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            this.context = context;
            this.mapper = mapper;
            this.userAccessor = userAccessor;
        }

        public async Task<bool> SaveActivity(Activity activity)
        {
            var existing = await context.Activities.FindAsync(activity.Id);
            if (existing == null)
            {
                await context.Activities.AddAsync(activity);
            }
            else
            {
                mapper.Map(activity, existing);
                existing.Category = activity.Category;
                existing.City = activity.City;
                existing.IsCancelled = activity.IsCancelled;
                existing.Title = activity.Title;
                existing.Venue = activity.Venue;
                existing.Description = activity.Description;
                existing.Date = activity.Date;
            }

            var changed = await context.SaveChangesAsync() > 0;
            return changed;
        }

        public async Task<bool> PlainSave()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveActivity(ActivityDto activity)
        {
            var existing = await context.Activities.FindAsync(activity.Id);
            if (existing == null)
            {
                existing = new Activity();
            }

            existing.Category = activity.Category;
            existing.City = activity.City;
            existing.IsCancelled = activity.IsCancelled;
            existing.Title = activity.Title;
            existing.Venue = activity.Venue;
            existing.Description = activity.Description;
            existing.Date = activity.Date;

            return await SaveActivity(existing);
        }

        public async Task<ActivityDto> GetActivity(Guid id, string activeUsername)
        {
            var activity = await GetActivity(id);
            activity.IsHost = activity.HostUsername == activeUsername;
            activity.IsGoing = activity.Attendees.Any(ac => ac.Username == activeUsername);

            return activity;
        }

        public async Task<ActivityDto> GetActivity(Guid id)
        {
            var result =  await DtoMappedActivities()
                   .FirstOrDefaultAsync(x => x.Id == id);

            result.Host = result.Attendees.SingleOrDefault(ac => ac.Username == result.HostUsername);
            
            return result;
        }

        public async Task<PagedList<ActivityDto>> ListActivities(PagingParams pagingParams)
        {
            var result = DtoMappedActivities().OrderBy(d => d.Date).AsQueryable();
            return await PagedList<ActivityDto>.CreateAsync(result, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<List<ActivityDto>> ListActivitiesForUser(string activeUsername)
        {
            var result = await DtoMappedActivities().ToListAsync();

            foreach (var a in result)
            {
                a.IsGoing = a.Attendees.Any(p => p.Username == activeUsername);
                a.IsHost = a.HostUsername == activeUsername;
                a.Host = a.Attendees.SingleOrDefault(ac => ac.Username == a.HostUsername);
            }

            return result;
        }

        public async Task<bool> DeleteActivity(Guid id)
        {
            var activity = await context.Activities.FindAsync(id);
            if (activity == null) return true;

            context.Activities.Remove(activity);
            var changed = await context.SaveChangesAsync();
            return changed > 0;
        }

        public async Task<Activity> GetRealActivity(Guid id)
        {
            return await context.Activities
                .Include(a => a.Attendees).ThenInclude(u => u.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        private IQueryable<ActivityDto> DtoMappedActivities()
        {
            return context.Activities
                               .ProjectTo<ActivityDto>(mapper.ConfigurationProvider,
                                    new { currentUsername = userAccessor.GetUsername() });
        }
    }
}