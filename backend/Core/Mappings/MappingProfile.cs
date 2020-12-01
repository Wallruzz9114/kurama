using System.Linq;
using AutoMapper;
using Data.ViewModels;
using Models;

namespace Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivityViewModel>();

            CreateMap<ActivityAttendee, ActivityAttendeeViewModel>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.PictureURL, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).URL));

            CreateMap<Comment, CommentViewModel>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.PictureURL, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).URL));
        }
    }
}