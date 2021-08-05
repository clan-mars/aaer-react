using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        [HttpGet("{id}/activities")]
        public async Task<ActionResult<List<Activity>>> GetUserActivities(string id)
        {
            return HandleResult(await Mediator.Send(new ListActivitiesForUser.Query { Username = id }));
        }


        [HttpPut("{id}/activities/{activityId}")]
        public async Task<ActionResult<List<Activity>>> UpdateHostedActivity(string id, Guid activityId, ActivityDto activityDto)
        {
            return HandleResult(await Mediator.Send(new UpdateHostedActivity.Command { ActivityId = activityId, IsCancelled = activityDto.IsCancelled }));
        }
    }
}