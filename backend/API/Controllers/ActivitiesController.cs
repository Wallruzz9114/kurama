using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Actions.Activities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    public class ActivitiesController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetAll() => await Mediator.Send(new GetAll.Query());

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetOne(Guid id) => await Mediator.Send(new GetOne.Query { Id = id });

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command createCommand) =>
            await Mediator.Send(createCommand);

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command editCommand)
        {
            editCommand.Id = id;
            return await Mediator.Send(editCommand);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id) => await Mediator.Send(new Delete.Command { Id = id });
    }
}