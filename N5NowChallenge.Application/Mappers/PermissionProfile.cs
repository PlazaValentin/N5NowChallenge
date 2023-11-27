using AutoMapper;
using N5NowChallenge.Application.Commands.CreatePermission;
using N5NowChallenge.Application.Commands.UpdatePermission;
using N5NowChallenge.Application.DTOs;
using N5NowChallenge.Domain.Entities;

namespace N5NowChallenge.Application.Mappers;

public class PermissionProfile : Profile
{
    public PermissionProfile()
    {
        CreateMap<Permission, PermissionDTO>().ReverseMap();
        CreateMap<Permission, CreatePermissionCommand>().ReverseMap();
        CreateMap<Permission, UpdatePermissionCommand>()
            .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src => src.Id))
            .ReverseMap();
    }
}
