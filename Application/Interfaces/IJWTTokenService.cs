using Domain.Model;

namespace Application.Interfaces;

public interface IJWTTokenService
{
    string GenerateToken(User user);
}
