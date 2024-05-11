using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Users.DTOs;
using Domain.Exceptions;

namespace Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(int Id) : IQuery<UserByIdResponse>;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserByIdResponse>
{
    private readonly IApplicationDbContext _context;
    
    public GetUserByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserByIdResponse> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync( new object[] { query.Id }, cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        return new UserByIdResponse(
            user.Id,
            user.UserName,
            user.FullName,
            user.Email,
            user.ProfilePictureUrl,
            user.Location,
            user.Bio,
            user.FollowersCount,
            user.FollowingCount
        );
    }
}