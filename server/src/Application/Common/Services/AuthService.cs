using System.Security.Claims;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationDbContext _context;

        public AuthService(IHttpContextAccessor httpContextAccessor, IApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public int GetCurrentUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdString, out var userId))
            {
                throw new UnauthorizedException("User not authenticated");
            }
            return userId;
        }

        public int? GetCurrentUserIdOrNull()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            return userIdString is null ? null : int.Parse(userIdString);
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId(); 
            return await _context.Users.FindAsync(userId);
        }

        public List<string> GetUserRoles()
        {
            var roles = _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)
                .Select(claim => claim.Value)
                .ToList();

            return roles ?? [];
        }

        public void EnsureRoleAccess(string requiredRole)
        {
            var roles = GetUserRoles();
            if (!roles.Contains(requiredRole))
            {
                throw new ForbiddenAccessException($"This action requires the '{requiredRole}' role");
            }
        }
    }
}