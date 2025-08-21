using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Seed;

public static class DefaultSeed
{
    public static async Task RunAsync(DefaultContext ctx, CancellationToken cancellationToken = default)
    {
        await ctx.Database.MigrateAsync(cancellationToken);

        if (!ctx.Users.Any())
        {
            var admin = new User
            {
                Username = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("@dmin123"),
                Email = "admin@gmail.com",
                Phone = "0000000000",
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };

            ctx.Users.Add(admin);
            await ctx.SaveChangesAsync(cancellationToken);
        }
    }
}
