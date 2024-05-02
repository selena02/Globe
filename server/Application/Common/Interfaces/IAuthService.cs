using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAuthService
{
    int? GetCurrentUserId();
    Task<User?> GetCurrentUserAsync();
}