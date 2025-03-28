using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;

/// <summary>
/// Provides user-related business logic.
/// </summary>
/// <param name="repository">The repository for accessing user data.</param>
/// <param name="mapper">The object mapper.</param>
public class UserConfigurationService(IUserConfigurationRepository repository, IMapper mapper)
    : IUserConfigurationService
{
    /// <inheritdoc/>
    public async Task<UserConfigurationDto> GetByUserIdAsync(int userId)
    {
        var configuration = await repository.GetByUserIdAsync(userId)
            ?? throw new UserNotFoundException(userId);

        return mapper.Map<UserConfiguration, UserConfigurationDto>(configuration);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(int userId, UserConfigurationDto dto)
    {
        var conf = await repository.GetByUserIdAsync(userId)
            ?? throw new UserNotFoundException(userId); // TODO UserConfigurationEXCEPTION

        conf.DistanceUnit = dto.DistanceUnit;
        conf.Language = dto.Language;
        conf.Theme = dto.Theme;
        conf.TimeSystem = dto.TimeSystem;

        await repository.UpdateAsync(conf);
    }
}
