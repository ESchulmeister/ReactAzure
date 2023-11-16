using reactAzure.Data;
using reactAzure.Models;
using AutoMapper;

namespace reactAzure.Profiles
{
    public class RoleProfile :Profile
    {

        public RoleProfile()
        {

            CreateMap<Role, RoleModel>()
                .ForMember(m => m.ID, o => o.MapFrom(d => d.RId))
                .ForMember(m => m.Name, o => o.MapFrom(d => d.RName))
                .ForMember(m => m.IsSupervisor, o => o.MapFrom(d => d.RSupervisor))
                .ForMember(m => m.IsAdministrator, o => o.MapFrom(d => d.RAdministrator))
                .ForMember(m => m.IsActive, o => o.MapFrom(d => d.RActive.HasValue))

                .ReverseMap();

        }
    }
}
