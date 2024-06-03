using AutoMapper;
using Recallio.Domain.Models.User;
using Recallio.Models.Responses.Users;
using Microsoft.AspNetCore.Identity;

namespace Recallio.Models.Mapping;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        this.CreateMap<User, CurrentUserResponse>();
        this.CreateMap<Role, CurrentUserResponse>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Name));
    }
}