using System.Threading.Tasks;
using Core.Actions.AppUsers;
using Core.Actions.Photos;
using Core.ViewModels;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AppUsersController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AppUserViewModel>> Login(Login.Query query) => await Mediator.Send(query);

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AppUserViewModel>> Register(Register.Command registerCommand) =>
            await Mediator.Send(registerCommand);

        [HttpGet("get")]
        public async Task<ActionResult<AppUserViewModel>> GetCurrentAppUser() =>
            await Mediator.Send(new GetCurrentAppUser.Query());

        [HttpGet("{username}")]
        public async Task<ActionResult<ProfileViewModel>> GetProfile(string username) =>
            await Mediator.Send(new GetProfile.Query { Username = username });
    }
}