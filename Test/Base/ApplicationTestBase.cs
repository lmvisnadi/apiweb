using Data.Context;
using Xunit;

namespace Test.Base
{
    public abstract class ApplicationTestBase : IAsyncLifetime
    {
        protected SharedContext _context = null!;

        public ApplicationTestBase()
        {
        }

        public async Task DisposeAsync()
        {
            await Task.Run(() => _context.Database.EnsureDeleted());
        }

        public Task InitializeAsync()
            => Task.CompletedTask;
    }
}
