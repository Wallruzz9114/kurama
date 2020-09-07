using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Actions.Activities;
using Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseController
    {
        [HttpGet]
        public async Task<IReadOnlyList<Activity>> ListAll() => await Mediator.Send(new ListAll.Query());

        [Authorize]
        [HttpGet("{id}")]
        public async Task<Activity> GetOne(Guid id) => await Mediator.Send(new GetOne.Query { Id = id });

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command) => await Mediator.Send(command);

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Update(Guid id, Update.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id) => await Mediator.Send(new Delete.Command { Id = id });
    }
}