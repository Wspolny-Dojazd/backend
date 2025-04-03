using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;

/// <inheritdoc/>
public class UserLocationService(IUserLocationRepository userLocationRepository, IMapper mapper) : IUserLocationService
{
    /// <inheritdoc/>
    public async Task<UserLocationDto> GetByUserIdAsync(Guid userId)
    {
        var userLocation = await userLocationRepository.GetByUserIdAsync(userId)
            ?? throw new UserLocationNotFoundException(userId);

        return mapper.Map<UserLocationDto>(userLocation);
    }

    /// <inheritdoc/>
    public async Task<UserLocationDto> UpdateAsync(Guid userId, UserLocationDto userLocation)
    {
        var userLocationEntity = new UserLocation
        {
            UserId = userId,
            Latitude = userLocation.Latitude,
            Longitude = userLocation.Longitude,
        };
        await userLocationRepository.UpdateAsync(userLocationEntity);
        return userLocation;
    }
}
