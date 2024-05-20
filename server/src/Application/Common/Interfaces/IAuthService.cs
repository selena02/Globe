using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAuthService
{
    int GetCurrentUserId();
    int? GetCurrentUserIdOrNull();
    Task<User?> GetCurrentUserAsync();
    List<string> GetUserRoles();
    void EnsureRoleAccess(string requiredRole);
}