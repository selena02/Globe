namespace Application.Common.Utils;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; set; }
    public int TotalPages { get;  set; }
    public int ItemTotal{ get; set; }
    public int PageSize { get; set; }
    
    

    private PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        ItemTotal = count;
        PageSize = pageSize;

        AddRange(items);
    }
    
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync(); 
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        var pagedList = new PaginatedList<T>(items, count, pageIndex, pageSize);
        
        return pagedList;
    }
}