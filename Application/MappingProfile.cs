using Application.DTOs;
using AutoMapper;
using Domain.Model;

namespace Application;

/// <summary>
/// Represents a configuration for maps.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes configuration for maps.
    /// </summary>
    public MappingProfile()
    {
        _ = this.CreateMap<User, UserDto>();
    }
}
