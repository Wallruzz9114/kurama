using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Errors;
using Core.ViewModels;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Actions.Comments
{
    public class Create
    {
        public class Command : IRequest<CommentViewModel>
        {
            public string Content { get; set; }
            public Guid ActivityId { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Command, CommentViewModel>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<CommentViewModel> Handle(Command command, CancellationToken cancellationToken)
            {
                var activity = await _dataContext.Activities.FindAsync(command.ActivityId);

                if (activity == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { activity = "Not Found" });

                var appUser = await _dataContext.Users
                    .SingleOrDefaultAsync(appUser => appUser.UserName == command.Username);

                var comment = new Comment
                {
                    Author = appUser,
                    Activity = activity,
                    Content = command.Content,
                    CreatedDate = DateTime.Now
                };

                activity.Comments.Add(comment);

                var commentAdded = await _dataContext.SaveChangesAsync() > 0;

                if (commentAdded) return _mapper.Map<CommentViewModel>(comment);

                throw new Exception("Problem adding comment");
            }
        }
    }
}