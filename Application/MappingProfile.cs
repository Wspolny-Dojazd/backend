using System.Text.Json;
using Application.DTOs;
using Application.DTOs.FriendInvitation;
using Application.DTOs.GroupInvitation;
using Application.DTOs.Message;
using Application.DTOs.Path;
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
        _ = this.CreateMap<User, GroupMemberDto>();
        _ = this.CreateMap<UserConfiguration, UserConfigurationDto>();
        _ = this.CreateMap<UserLocation, UserLocationDto>();
        _ = this.CreateMap<Message, MessageDto>();
        _ = this.CreateMap<FriendInvitation, FriendInvitationDto>();
        _ = this.CreateMap<GroupInvitation, GroupInvitationDto>();

        _ = this.CreateMap<Group, GroupDto>()
            .AfterMap((src, dest) =>
            {
                var creator = dest.GroupMembers.FirstOrDefault(m => m.Id == src.CreatorId);
                if (creator is not null)
                {
                    creator.IsCreator = true;
                }
            });

        _ = this.CreateMap<ProposedPath, ProposedPathDto>()
            .ForCtorParam("Paths", opt =>
                opt.MapFrom(src => DeserializeUserPathDtos(src.SerializedDto)));

        _ = this.CreateMap<GroupPath, GroupPathDto>()
            .ForCtorParam("Paths", opt =>
                opt.MapFrom(src => DeserializeUserPathDtos(src.SerializedDto)));
    }

    private static IEnumerable<UserPathDto> DeserializeUserPathDtos(string serializedDto)
    {
        return JsonSerializer.Deserialize<IEnumerable<UserPathDto>>(serializedDto)!;
    }
}
