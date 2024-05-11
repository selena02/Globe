using Application.Authentication.Commands.RegisterUser;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<IdentityResult> CreateAsync(RegisterUserCommand user, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> IsInRoleAsync(User user, string role);
        Task<IList<string>> GetRolesAsync(User user);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        Task<IdentityResult> RemoveFromRoleAsync(User user, string role);
    }
}