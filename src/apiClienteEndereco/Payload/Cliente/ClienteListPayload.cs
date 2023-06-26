using Application.QuerySide.ListCliente;
using Infraestructure.Pagination;

namespace apiClienteEndereco.Payload.Cliente
{
    public class ClienteListPayload
    {

        public ClienteListPayload()
        {
        }
        public ListClienteQuery AsQuery(PaginationOptions? paginationOptions)
            => new(paginationOptions);
    }
}
