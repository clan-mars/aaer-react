using System;
using System.Threading.Tasks;
using Domain;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Application.Activities;
using System.Linq;
using System.Collections.Generic;

namespace Persistence
{

    public class ActivityRepository : IActivityRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;


        public ActivityRepository(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
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
                context.Activities.Update(activity);
            }

            var changed = await context.SaveChangesAsync() > 0;
            return changed;
        }

        public async Task<ActivityDto> GetActivity(Guid id, string activeUsername)
        {
            var activity = await context.Activities
                   .ProjectTo<ActivityDto>(mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(x => x.Id == id);
            activity.IsHost = activity.HostUsername == activeUsername;
            activity.IsGoing = activity.Attendees.Any(ac => ac.Username == activeUsername);

            return activity;
        }

        public async Task<List<ActivityDto>> ListActivities()
        {
            return await context.Activities
                .ProjectTo<ActivityDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<ActivityDto>> ListActivitiesForUser(string activeUsername)
        {
            var result = await context.Activities
               .ProjectTo<ActivityDto>(mapper.ConfigurationProvider)
               .ToListAsync();

            foreach (var a in result)
            {
                a.IsGoing = a.Attendees.Any(p => p.Username == activeUsername);
                a.IsHost = a.HostUsername == activeUsername;
                a.Host = a.Attendees.Single(ac => ac.Username == a.HostUsername);
            }

            return result;
        }

        public async Task<bool> DeleteActivity(Activity activity)
        {
            return await Task<bool>.Run(() =>
            {
                var changed = context.Activities.Remove(activity);
                return changed != null;
            });
        }
    }
}