using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Actions.Activities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    public class ActivitiesController : BaseController
    {
        private readonly IMediator _mediator;

        public ActivitiesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetAll() => await _mediator.Send(new GetAll.Query());

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetOne(Guid id) => await _mediator.Send(new GetOne.Query { Id = id });

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command createCommand) =>
            await _mediator.Send(createCommand);

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command editCommand)
        {
            editCommand.Id = id;
            return await _mediator.Send(editCommand);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id) => await _mediator.Send(new Delete.Command { Id = id });
    }
}