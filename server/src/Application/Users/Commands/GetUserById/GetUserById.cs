using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Users.DTOs;
using Domain.Exceptions;

namespace Application.Users.Commands.GetUserById;

public record GetUserByIdCommand(int UserId) : ICommand<UserDto>;

public class GetUserById
{
    
    
}