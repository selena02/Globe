namespace Application.Common.Interfaces;

public interface IUserService
{
    Task DeleteUserAsync(int userId, CancellationToken cancellationToken);
}