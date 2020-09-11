using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Actions.SocialLinks;
using Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SocialLinksController : BaseController
    {
        [HttpPost("follow/{username}")]
        public async Task<ActionResult<Unit>> Follow(string username) =>
            await Mediator.Send(new Follow.Command { Username = username });

        [HttpDelete("unfollow/{username}")]
        public async Task<ActionResult<Unit>> Unfollow(string username) =>
            await Mediator.Send(new Unfollow.Command { Username = username });

        [HttpGet("getfollowers/{username}")]
        public async Task<ActionResult<IReadOnlyList<ProfileViewModel>>> GetFollowers(string username) =>
            await Mediator.Send(new GetFollowers.Query { Username = username });

        [HttpGet("getfavourites/{username}")]
        public async Task<ActionResult<IReadOnlyList<ProfileViewModel>>> GetFavourites(string username) =>
            await Mediator.Send(new GetFavourites.Query { Username = username });
    }
}