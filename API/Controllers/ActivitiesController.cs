using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain;
using System;
using Microsoft.AspNetCore.Authorization;
using Mediators;
using API.Middleware;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            return await Process(new Activities.GetDetails { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            return await Process(new Activities.Create { Activity = activity });
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            return await Process(new Activities.Edit { Activity = activity });
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return await Process(new Activities.Delete { Id = id });
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id) {
            return await Process(new Attendance.AttendRequest{ActivityId = id});
        }

        [HttpDelete("{id}/attend")]
        public async Task<IActionResult> Unattend(Guid id) {
            return await Process(new Attendance.UnattendRequest{ActivityId = id});
        }
    }
}