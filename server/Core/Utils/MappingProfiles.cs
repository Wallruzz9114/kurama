using System.Linq;
using AutoMapper;
using Core.ViewModels;
using Data;

namespace Core.Utils
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, ActivityViewModel>();
            CreateMap<ActivityAttendee, ActivityAttendeeViewModel>()
                .ForMember(aavm => aavm.Username, e => e.MapFrom(aa => aa.AppUser.UserName))
                .ForMember(aavm => aavm.DisplayName, e => e.MapFrom(aa => aa.AppUser.DisplayName))
                .ForMember(
                    aavm => aavm.ProfilePictureURL,
                    e => e.MapFrom(aa => aa.AppUser.Photos.FirstOrDefault(photo => photo.IsProfilePicture).URL)
                );
        }
    }
}