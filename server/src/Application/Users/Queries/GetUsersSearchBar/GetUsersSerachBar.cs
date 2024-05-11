using Application.Common.Abstractions;
using Application.Common.Interfaces;

namespace Application.Users.Queries.GetUsersSearchBar;

public record GetUsersSearchBarQuery() : IQuery<GetUsersSearchBarResponse>;

public class SearchBarUser
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string? ProfilePictureUrl { get; set; }
}

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
            .Select(u => new SearchBarUser
            {
                Id = u.Id,
                UserName = u.UserName,
                FullName = u.FullName,
                ProfilePictureUrl = u.ProfilePictureUrl
            })
            .ToListAsync(cancellationToken);
        
        return new GetUsersSearchBarResponse { Users = users };
    }
}