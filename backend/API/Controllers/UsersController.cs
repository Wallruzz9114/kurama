using System.Threading.Tasks;
using Core.Actions.AppUsers;
using Data.ViewModels;
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
    }
}