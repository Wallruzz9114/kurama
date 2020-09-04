using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Activities;
using Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseController
    {
        private readonly IMediator _mediator;

        public ActivitiesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IReadOnlyList<Activity>> ListAll() => await _mediator.Send(new ListAll.Query());

        [HttpGet("{id}")]
        public async Task<Activity> GetOne(Guid id) => await _mediator.Send(new GetOne.Query { Id = id });

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command) => await _mediator.Send(command);

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Update(Guid id, Update.Command command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id) => await _mediator.Send(new Delete.Command { Id = id });
    }
}