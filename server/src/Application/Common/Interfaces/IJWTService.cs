using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IJWTService
{
    Task<string> GenerateTokenAsync(User user);
}