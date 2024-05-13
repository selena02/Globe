using Application.Common.Abstractions;
using Application.Common.Interfaces;

namespace Application.Users.Queries.GetUsersSearchBar;

public record GetUsersSearchBarQuery() : IQuery<GetUsersSearchBarResponse>;

public record SearchBarUser(int UserId, string Username, string FullName, string? ProfilePicture);

public class GetUsersSearchBarResponse
{ 
    public List<SearchBarUser> Users { get; set; }
}

public class GetUsersSearchBarQueryHandler : IQueryHandler<GetUsersSearchBarQuery, GetUsersSearchBarResponse>
{
    private readonly IApplicationDbContext _context;
    
    public GetUsersSearchBarQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetUsersSearchBarResponse> Handle(GetUsersSearchBarQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .AsNoTracking()
            .Select(u => new SearchBarUser(
                u.Id,
                u.UserName,
                u.FullName,
                u.PicturePublicId
            ))
            .ToListAsync(cancellationToken);
        
        return new GetUsersSearchBarResponse { Users = users };
    }
}