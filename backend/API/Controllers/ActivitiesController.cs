using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Actions.Activities;
using Data.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<ActivityViewModel>>> GetAll() => await Mediator.Send(new GetAll.Query());

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityViewModel>> GetOne(Guid id) => await Mediator.Send(new GetOne.Query { Id = id });

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command createCommand) =>
            await Mediator.Send(createCommand);

        [Authorize(Policy = "IsHost")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command editCommand)
        {
            editCommand.Id = id;
            return await Mediator.Send(editCommand);
        }

        [Authorize(Policy = "IsHost")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id) => await Mediator.Send(new Delete.Command { Id = id });

        [HttpPost("attend/{id}")]
        public async Task<ActionResult<Unit>> Attend(Guid id) => await Mediator.Send(new Attend.Command { Id = id });

        [HttpDelete("reject/{id}")]
        public async Task<ActionResult<Unit>> Reject(Guid id) => await Mediator.Send(new Reject.Command { Id = id });
    }
}