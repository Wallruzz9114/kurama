using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.Security;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Actions.SocialLinks
{
    public class GetFavourites
    {
        public class Query : IRequest<List<ProfileViewModel>>
        {
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<ProfileViewModel>>
        {
            private readonly DataContext _dataContext;
            private readonly IAppUserService _appUserService;

            public Handler(DataContext dataContext, IAppUserService appUserService)
            {
                _dataContext = dataContext;
                _appUserService = appUserService;
            }

            public async Task<List<ProfileViewModel>> Handle(Query query, CancellationToken cancellationToken)
            {
                var queryable = _dataContext.SocialLinks.AsQueryable();
                var appUserLinks = new List<SocialLink>();
                var favourites = new List<ProfileViewModel>();

                appUserLinks = await queryable
                    .Where(x => x.SourceUser.UserName == query.Username).ToListAsync();

                foreach (var link in appUserLinks)
                    favourites.Add(await _appUserService.GetProfile(link.TargetUser.UserName));

                return favourites;
            }
        }
    }
}