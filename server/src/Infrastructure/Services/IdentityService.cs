using Application.Authentication.Commands.RegisterUser;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;

    public IdentityService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<IdentityResult> CreateAsync(RegisterUserCommand user, string password)
    {
        var newUser = new User
        {
            UserName = user.UserName,
            Email = user.Email,
            FullName = user.FullName
        };
        
        return await _userManager.CreateAsync(newUser, password);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<bool> IsInRoleAsync(User user, string role)
    {
        return await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<IList<string>> GetRolesAsync(User user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> AddToRoleAsync(User user, string role)
    {
        return await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<IdentityResult> RemoveFromRoleAsync(User user, string role)
    {
        return await _userManager.RemoveFromRoleAsync(user, role);
    }
}