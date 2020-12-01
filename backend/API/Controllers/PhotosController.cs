using System.Threading.Tasks;
using Core.Actions.Photos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    public class PhotosController : BaseController
    {
        [HttpPost("upload")]
        public async Task<ActionResult<Photo>> Upload([FromForm] Upload.Command uploadCommand) =>
            await Mediator.Send(uploadCommand);

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Unit>> Delete(string id) => await Mediator.Send(new Delete.Command { Id = id });

        [HttpPost("setasmain/{id}")]
        public async Task<ActionResult<Unit>> SetAsMain(string id) => await Mediator.Send(new SetAsMain.Command { Id = id });
    }
}