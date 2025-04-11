using Application.DTOs;
using Application.DTOs.Message;
using Application.DTOs.UserLocation;
using AutoMapper;
using Domain.Model;

namespace Application;

/// <summary>
/// Represents the AutoMapper configuration for mapping between domain entities and data transfer objects.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        _ = this.CreateMap<User, UserDto>();
        _ = this.CreateMap<Group, GroupDto>();
        _ = this.CreateMap<UserConfiguration, UserConfigurationDto>();
        _ = this.CreateMap<UserLocation, UserLocationDto>();
        _ = this.CreateMap<Message, MessageDto>();
        _ = this.CreateMap<FriendInvitation, FriendInvitationDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InvitationId));
    }
}
