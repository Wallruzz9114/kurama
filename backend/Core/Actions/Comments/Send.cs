using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Errors;
using Data.Contexts;
using Data.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Core.Actions.Comments
{
    public class Send
    {
        public class Command : IRequest<CommentViewModel>
        {
            public Guid ActivityId { get; set; }
            public string Username { get; set; }
            public string Body { get; set; }
        }

        public class Handler : IRequestHandler<Command, CommentViewModel>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IMapper _mapper;

            public Handler(DatabaseContext databaseContext, IMapper mapper)
            {
                _databaseContext = databaseContext;
                _mapper = mapper;
            }

            public async Task<CommentViewModel> Handle(Command command, CancellationToken cancellationToken)
            {
                var activity = await _databaseContext.Activities.FindAsync(command.ActivityId);

                if (activity == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { activity = "Not found" });

                var appUser = await _databaseContext.Users.SingleOrDefaultAsync(x => x.UserName == command.Username);
                var comment = new Comment
                {
                    AppUser = appUser,
                    Activity = activity,
                    Body = command.Body,
                    CreatedAt = DateTime.Now,
                };

                activity.Comments.Add(comment);

                var activitySuccessfullyCreated = await _databaseContext.SaveChangesAsync() > 0;

                if (activitySuccessfullyCreated) return _mapper.Map<CommentViewModel>(comment);

                throw new Exception("Problem");
            }
        }
    }
}