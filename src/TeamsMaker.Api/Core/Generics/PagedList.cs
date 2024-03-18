namespace Core.Generics;

public class PagedList<T> : List<T> where T : class
{
    public int CurrentPage { get; private init; }
    public int TotalPages { get; private init; }
    public int PageSize { get; private init; }
    public int TotalCount { get; private init; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        CurrentPage = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip(Math.Max(0, (pageNumber - 1)) * pageSize).Take(pageSize).ToList();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }

    public static async Task<PagedList<T>> ToPagedListAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken ct)
    {
        var count = source.Count();
        var items = await source.Skip(Math.Max(0, (pageNumber - 1)) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
