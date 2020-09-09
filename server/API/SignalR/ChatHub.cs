using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Actions.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        public ChatHub(IMediator mediator) => _mediator = mediator;

        public async Task SendComment(Create.Command createCommand)
        {
            createCommand.Username = GetAppUserUsername();

            var comment = await _mediator.Send(createCommand);
            await Clients.Group(createCommand.ActivityId.ToString()).SendAsync("ReceiveComment", comment);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{ GetAppUserUsername() } has joined the group");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{ GetAppUserUsername() } has left the group");
        }

        private string GetAppUserUsername() => Context.User?.Claims?
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}