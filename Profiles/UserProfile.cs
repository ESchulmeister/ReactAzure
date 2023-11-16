using AutoMapper;
using reactAzure.Data;
using reactAzure.Models;

namespace reactAzure
{
    namespace reactSample
    {
        public class UserProfile : Profile
        {
            public UserProfile()
            {
                this.CreateMap<User, UserModel>()
                    .ForMember(m => m.ID, o => o.MapFrom(d => d.UsrId))
                    .ForMember(m => m.FirstName, o => o.MapFrom(d => d.UsrFirst))
                    .ForMember(m => m.LastName, o => o.MapFrom(d => d.UsrLast))
                    .ForMember(m => m.Login, o => o.MapFrom(d => d.UsrLogin))
                    .ForMember(m => m.IsActive, o => o.MapFrom(d => d.UsrActive.HasValue))
                    .ForMember(m => m.FTE, o => o.MapFrom(d => d.UsrFte))
                    .ForMember(m => m.RoleID, o => o.MapFrom(d => d.UsrDefaultRole))
                    .ForMember(m => m.Clock, o => o.MapFrom(d => d.UsrClock))
                    .ForMember(m => m.Role, o => o.MapFrom(d => d.UsrDefaultRoleNavigation))    //role property mapping
                    .ReverseMap();

            }


        }
    }
}
