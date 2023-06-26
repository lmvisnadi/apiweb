using Data.Context;
using dominio.Entities;
using dominio.Repositories;
using Infraestructure.Repository;

namespace Data.Repositories
{
    public class ClienteRepository : EntityBaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(SharedContext context) : base(context)
        {
        }
    }
}
