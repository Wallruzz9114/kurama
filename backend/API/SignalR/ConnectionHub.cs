using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Actions.Comments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class ConnectionHub : Hub
    {
        private readonly IMediator _mediator;

        public ConnectionHub(IMediator mediator) => _mediator = mediator;

        public async Task SendComment(Send.Command sendCommand)
        {
            sendCommand.Username = GetUsername();
            var comment = await _mediator.Send(sendCommand);

            await Clients.Group(sendCommand.ActivityId.ToString()).SendAsync("ReceiveComment", comment);
        }

        private string GetUsername() =>
            Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{ GetUsername() } has joined the group");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{ GetUsername() } has left the group");
        }
    }
}