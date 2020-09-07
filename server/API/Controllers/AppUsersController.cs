using System.Threading.Tasks;
using Core.Actions.AppUsers;
using Core.ViewModels;
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
        public async Task<ActionResult<AppUserViewModel>> Register(Register.Command command) =>
            await Mediator.Send(command);

        [HttpGet("get")]
        public async Task<ActionResult<AppUserViewModel>> GetCurrentAppUser() =>
            await Mediator.Send(new GetCurrentAppUser.Query());
    }
}