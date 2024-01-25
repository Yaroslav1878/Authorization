using Authorization.Application.Models.Responses.User;
using Authorization.Domain.Models.Dtos;
using AutoMapper;

namespace Authorization.Application.Mapping;

public class ApplicationMappingsProfile : Profile
{
    public ApplicationMappingsProfile()
    {
        // request

        // response
        CreateMap<UserDto, UserResponseModel>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleId));
    }
}
