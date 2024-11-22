namespace Shared.Pagination.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginatedRequest request)
    {
        return query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
    }
}
