using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using Mediators;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        [HttpGet("{id}/activities")]
        public async Task<ActionResult<List<Activity>>> GetUserActivities(string id)
        {
            return await Process((new Activities.ListUserActivities { Username = id }));
        }


        [HttpPut("{id}/activities/{activityId}")]
        public async Task<ActionResult<List<Activity>>> UpdateHostedActivity(string id, Guid activityId, ActivityDto activityDto)
        {
            return await Process(new HostedActivities.ToggleActivityCanceledRequest { ActivityId = activityId, IsCancelled = activityDto.IsCancelled });
        }
    }
}