using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Actions.Activities;
using Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseController
    {
        [HttpGet]
        public async Task<IReadOnlyList<ActivityViewModel>> ListAll() => await Mediator.Send(new FindAll.Query());

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActivityViewModel> GetOne(Guid id) => await Mediator.Send(new FindOne.Query { Id = id });

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command createCommand) =>
            await Mediator.Send(createCommand);

        [Authorize(Policy = "AppUserIsHostingActivity")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Update(Guid id, Update.Command updateCommand)
        {
            updateCommand.Id = id;
            return await Mediator.Send(updateCommand);
        }

        [Authorize(Policy = "AppUserIsHostingActivity")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id) => await Mediator.Send(new Delete.Command { Id = id });

        [HttpPost("attend/{id}")]
        public async Task<ActionResult<Unit>> Attend(Guid id) => await Mediator.Send(new Attend.Command { Id = id });

        [HttpDelete("cancelattendance/{id}")]
        public async Task<ActionResult<Unit>> CancelAttendance(Guid id) =>
            await Mediator.Send(new CancelAttendance.Command { Id = id });
    }
}