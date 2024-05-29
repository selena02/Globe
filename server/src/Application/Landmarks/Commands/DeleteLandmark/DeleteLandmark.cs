using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Landmarks.Commands.DeleteLandmark;

public record DeleteLandmarkCommand(int LandmarkId) : ICommand<DeleteLandmarkResponse>;

public record DeleteLandmarkResponse(bool IsDeleted);

public class DeleteLandmarkCommandHandler : ICommandHandler<DeleteLandmarkCommand, DeleteLandmarkResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public DeleteLandmarkCommandHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<DeleteLandmarkResponse> Handle(DeleteLandmarkCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();

        var landmark = await _context.Landmarks.FindAsync(new object[] { request.LandmarkId }, cancellationToken);

        if (landmark is null)
        {
            throw new NotFoundException("Landmark not found");
        }

        var isGuide = _authService.GetUserRoles().Contains(Roles.Guide.ToString());

        if (landmark.UserId != currentUserId && !isGuide)
        {
            throw new ForbiddenAccessException("User is not authorized to delete this landmark");
        }

        _context.Landmarks.Remove(landmark);

        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteLandmarkResponse(true);
    }
}