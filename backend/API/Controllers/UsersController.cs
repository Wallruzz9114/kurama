using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Actions.AppUsers;
using Data.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AppUserViewModel>> Login(Login.Query loginQuery) => await Mediator.Send(loginQuery);

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AppUserViewModel>> Register(Register.Command registerCommand) =>
            await Mediator.Send(registerCommand);

        [HttpGet("getcurrentuser")]
        public async Task<ActionResult<AppUserViewModel>> GetCurrentUser() => await Mediator.Send(new GetCurrentUser.Query());

        [HttpGet("getprofile/{username}")]
        public async Task<ActionResult<ProfileViewModel>> GetProfile(string username) =>
            await Mediator.Send(new GetProfile.Query { Username = username });

        [HttpPut("updatebio")]
        public async Task<ActionResult<Unit>> UpdateBio(UpdateBio.Command updateBioCommand) =>
            await Mediator.Send(updateBioCommand);

        [HttpPost("follow/{username}")]
        public async Task<ActionResult<Unit>> Follow(string username) =>
            await Mediator.Send(new Follow.Command { Username = username });

        [HttpDelete("unfollow/{username}")]
        public async Task<ActionResult<Unit>> Unfollow(string username) =>
            await Mediator.Send(new Unfollow.Command { Username = username });

        [HttpGet("getrelationship/{username}")]
        public async Task<ActionResult<List<ProfileViewModel>>> GetRelationship(string username, string relationship) =>
            await Mediator.Send(new GetRelationship.Query { Username = username, Relationship = relationship });
    }
}