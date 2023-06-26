using Data.Context;
using dominio.Entities;
using dominio.Repositories;
using Infraestructure.Repository;

namespace Data.Repositories
{
    public class UnidadeFederativaRepository : EntityBaseRepository<UnidadeFederativa>, IUnidadeFederativaRepository
    {
        public UnidadeFederativaRepository(SharedContext context) : base(context)
        {
        }
    }
}
