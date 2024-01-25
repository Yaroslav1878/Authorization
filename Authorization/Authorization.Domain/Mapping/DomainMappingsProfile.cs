using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Models.Entities;
using AutoMapper;

namespace Authorization.Domain.Mapping;

public class DomainMappingsProfile : Profile
{
    public DomainMappingsProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<Scope, ScopeDto>();
        CreateMap<Role, RoleDto>();
        CreateMap<RoleScope, RoleScopeDto>();
        CreateMap<RefreshToken, RefreshTokenDto>();
        CreateMap<ConfirmationRequest, ConfirmationRequestDto>();

        CreateMap<UserDto, User>();
        CreateMap<ScopeDto, Scope>();
        CreateMap<RoleDto, Role>();
        CreateMap<RoleScopeDto, RoleScope>();
        CreateMap<RefreshTokenDto, RefreshToken>();
        CreateMap<ClientDto, Client>();
        CreateMap<ConfirmationRequestDto, ConfirmationRequest>();
    }
}
