using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Pagination
{
    public class Paginator<T> : IPaginator<T>
    {
        public static Paginator<T> DefaultPaginator = new Paginator<T>();
        public readonly int DefaultAmountPerPage = 10;

        public Page<T> PaginateIEnumerable(
            IEnumerable<T> entities,
            PaginationOptions? paginationOptions = null
        ) => Paginate(
            entities.AsQueryable(),
            paginationOptions
        );

        public Page<T> Paginate(
            IQueryable<T> entities,
            PaginationOptions? paginationOptions = null
        )
        {
            var totalPages = 1;
            var totalItems = entities.Count();

            if (paginationOptions != null)
            {
                var amountPerPage = paginationOptions.AmountPerPage ?? DefaultAmountPerPage;
                var begin = paginationOptions.Page == 1 ? 0 : ((paginationOptions.Page - 1) * amountPerPage);

                totalPages = (int)Math.Ceiling(totalItems / (double)amountPerPage);
                entities = entities.Skip(begin).Take(amountPerPage);
            }

            var page = new Page<T>()
            {
                TotalPages = totalPages,
                Items = entities,
                TotalItems = totalItems
            };

            return page;
        }

        public async Task<Page<T>> PaginateAsync(
            IQueryable<T> entities,
            PaginationOptions? paginationOptions = null,
            CancellationToken? cancellationToken = null
        )
        {
            var totalPages = 1;
            var totalItems = await entities.CountAsync(cancellationToken ?? CancellationToken.None);

            if (paginationOptions != null)
            {
                var amountPerPage = paginationOptions.AmountPerPage ?? DefaultAmountPerPage;
                var begin = paginationOptions.Page == 1 ? 0 : ((paginationOptions.Page - 1) * amountPerPage);

                totalPages = (int)Math.Ceiling(totalItems / (double)amountPerPage);
                entities = entities.Skip(begin).Take(amountPerPage);
            }

            var page = new Page<T>()
            {
                TotalPages = totalPages,
                Items = await entities.ToListAsync(cancellationToken ?? CancellationToken.None),
                TotalItems = totalItems
            };

            return page;
        }
    }
}
