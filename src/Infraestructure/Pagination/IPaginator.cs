namespace Infraestructure.Pagination
{
    public interface IPaginator<T>
    {
        Page<T> Paginate(IQueryable<T> entities, PaginationOptions? paginationOptions = null);
        Page<T> PaginateIEnumerable(IEnumerable<T> entities, PaginationOptions? paginationOptions = null);
    }
}
