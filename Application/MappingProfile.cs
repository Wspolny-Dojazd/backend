using Application.DTOs;
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
    }
}
