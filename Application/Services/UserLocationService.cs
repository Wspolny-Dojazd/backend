﻿using Application.DTOs.UserLocation;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Shared.Enums.ErrorCodes;

namespace Application.Services;

/// <summary>
/// Provides logic related to user location.
/// </summary>
/// <param name="userLocationRepository">The repository for accessing user location data.</param>
/// <param name="mapper">The object mapper.</param>
public class UserLocationService(IUserLocationRepository userLocationRepository, IMapper mapper) : IUserLocationService
{
    /// <inheritdoc/>
    public async Task<UserLocationDto> GetByUserIdAsync(Guid userId)
    {
        var userLocation = await userLocationRepository.GetByUserIdAsync(userId)
            ?? throw new AppException(404, UserLocationErrorCode.LOCATION_NOT_FOUND);

        return mapper.Map<UserLocationDto>(userLocation);
    }

    /// <inheritdoc/>
    public async Task<UserLocationDto> UpdateAsync(Guid userId, UserLocationRequestDto dto)
    {
        var newLocation = await userLocationRepository.GetByUserIdAsync(userId)
            ?? new UserLocation
            {
                UserId = userId,
            };

        newLocation.Latitude = dto.Latitude;
        newLocation.Longitude = dto.Longitude;
        newLocation.UpdatedAt = DateTime.UtcNow;
        await userLocationRepository.UpdateAsync(newLocation);

        return mapper.Map<UserLocationDto>(newLocation);
    }
}
