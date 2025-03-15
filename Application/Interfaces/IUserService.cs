using Application.DTOs;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(int id);
}
