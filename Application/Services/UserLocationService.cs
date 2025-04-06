using Application.DTOs.UserLocation;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;

/// <summary>
/// Provides logic related to user location.
/// </summary>
/// <param name="userLocationRepository">The repository for accessing user location data.</param>
/// <param name="mapper">The object mapper.</param>
public class UserLocationService(IUserLocationRepository userLocationRepository, IMapper mapper) : IUserLocationService
{
    /// <inheritdoc/>
    public async Task<UserLocationRequestDto> GetByUserIdAsync(Guid userId)
    {
        var userLocation = await userLocationRepository.GetByUserIdAsync(userId)
            ?? throw new UserLocationNotFoundException(userId);

        return mapper.Map<UserLocationRequestDto>(userLocation);
    }

    /// <inheritdoc/>
    public async Task<UserLocationRequestDto> UpdateAsync(Guid userId, UserLocationRequestDto dto)
    {
        var userLocation = new UserLocation
        {
            UserId = userId,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
        };
        var existingLocation = await userLocationRepository.GetByUserIdAsync(userId);

        if (existingLocation == null)
        {
            await userLocationRepository.AddAsync(userLocation);
        }
        else
        {
            await userLocationRepository.UpdateAsync(userLocation);
        }

        return mapper.Map<UserLocationRequestDto>(userLocation);
    }
}
