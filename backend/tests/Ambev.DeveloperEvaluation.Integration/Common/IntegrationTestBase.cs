
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Integration.Common;

public abstract class IntegrationTestBase : IDisposable
{
    protected readonly DefaultContext Db;

    protected IntegrationTestBase()
    {
        var services = new ServiceCollection();
        services.AddDbContext<DefaultContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        var sp = services.BuildServiceProvider();
        Db = sp.GetRequiredService<DefaultContext>();
        Db.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Db.Database.EnsureDeleted();
        Db.Dispose();
    }
}
