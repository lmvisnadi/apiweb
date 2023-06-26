using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public static class SQLiteContextFactory
    {
        public static SharedContext Create(Guid userId, Guid? contaId = null, string? databaseName = null, Guid? participanteLogadoId = null, Guid[]? participanteAcessoIds = null)
        {
            databaseName = databaseName ?? Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<BaseContext>()
                     .UseSqlite($"Data Source={databaseName}.db")
                     .Options;

            var context = new SharedContext(options, userId);

            context.Database.EnsureCreated();

            return context;
        }
    }
}
