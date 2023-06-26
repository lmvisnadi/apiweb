namespace Infraestructure.Pagination
{
    public static class Extension
    {
        public static Page<T> Paginate<T>(
            this IQueryable<T> entities,
            PaginationOptions? paginationOptions = null
        )
            => Paginator<T>.DefaultPaginator.Paginate(entities, paginationOptions);

        public static async Task<Page<T>> PaginateAsync<T>(
            this IQueryable<T> entities,
            PaginationOptions? paginationOptions = null,
            CancellationToken? cancellationToken = null
        )
            => await Paginator<T>.DefaultPaginator.PaginateAsync(entities, paginationOptions, cancellationToken);
    }
}
