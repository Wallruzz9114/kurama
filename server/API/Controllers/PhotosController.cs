using System.Threading.Tasks;
using Core.Actions.Photos;
using Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PhotosController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Photo>> Upload(Upload.Command uploadCommand) =>
            await Mediator.Send(uploadCommand);

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(string id) =>
            await Mediator.Send(new Delete.Command { Id = id });
    }
}