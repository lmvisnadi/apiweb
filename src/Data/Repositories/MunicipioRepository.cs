using Data.Context;
using dominio.Entities;
using dominio.Repositories;
using Infraestructure.Repository;

namespace Data.Repositories
{
    public class MunicipioRepository : EntityBaseRepository<Municipio>, IMunicipioRepository
    {
        public MunicipioRepository(SharedContext context) : base(context)
        {
        }
    }
}
