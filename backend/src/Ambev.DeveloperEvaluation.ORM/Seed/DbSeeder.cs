using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.ORM.Seed;

public static class DbSeeder
{
    public static void EnsureSeeded(DefaultContext ctx)
    {
        ctx.Database.EnsureCreated();

        if (ctx.Database.ProviderName?.Contains("InMemory") == true)
            return;

        if (!ctx.Customers.Any())
        {
            ctx.Customers.AddRange(new[]
            {
                new Customer { Name = "Alice Doe", Email = "alice@dev.com", Phone = "19999999999" },
                new Customer { Name = "Bob Smith", Email = "bob@dev.com", Phone = "19999999999" },
                new Customer { Name = "Charlie Brown", Email = "charlie@dev.com", Phone = "19999999999" },
                new Customer { Name = "David Johnson", Email = "david@dev.com", Phone = "19999999999" },
                new Customer { Name = "Eve White", Email = "eve@dev.com", Phone = "19999999999" },
                new Customer { Name = "Frank Green", Email = "frank@dev.com", Phone = "19999999999" },
                new Customer { Name = "Grace Lee", Email = "grace@dev.com", Phone = "19999999999" },
                new Customer { Name = "Hank Miller", Email = "hank@dev.com", Phone = "19999999999"},
                new Customer { Name = "Ivy Clark", Email = "ivy@dev.com", Phone = "19999999999" },
                new Customer { Name = "Jack King", Email = "jack@dev.com", Phone = "19999999999" }
            });

            ctx.SaveChanges();
        }

        if (!ctx.Products.Any())
        {
            ctx.Products.AddRange(new[]
            {
                new Product { Name = "Lager 350ml", Price = 6.50m },
                new Product { Name = "IPA 500ml", Price = 12.00m },
                new Product { Name = "Stout 330ml", Price = 7.80m },
                new Product { Name = "Pilsner 500ml", Price = 5.40m },
                new Product { Name = "Wheat Beer 400ml", Price = 8.20m },
                new Product { Name = "Pale Ale 330ml", Price = 9.50m },
                new Product { Name = "Porter 500ml", Price = 11.00m },
                new Product { Name = "Saison 375ml", Price = 10.50m },
                new Product { Name = "Amber Ale 500ml", Price = 7.00m },
                new Product { Name = "Double IPA 330ml", Price = 14.00m }
            });

            ctx.SaveChanges();
        }

        if (!ctx.Sales.Any())
        {
            var customers = ctx.Customers.ToList();
            var products = ctx.Products.ToList();
            var branchId = Guid.NewGuid().ToString();

            var sale1 = new Sale
            {
                SaleNumber = "S-0001",
                CustomerId = customers[0].Id,
                CustomerName = customers[0].Name,
                BranchId = branchId,
                BranchName = "Branch - Rio de Janeiro",
                Date = DateTime.UtcNow
            };
            var item1 = new SaleItem
            {
                ProductId = products[0].Id,
                ProductName = products[0].Name,
                Quantity = 4,
                UnitPrice = products[0].Price
            };
            item1.ApplyBusinessRules();
            sale1.AddItem(item1);
            sale1.RecalculateTotal();
            ctx.Sales.Add(sale1);

            var sale2 = new Sale
            {
                SaleNumber = "S-0002",
                CustomerId = customers[1].Id,
                CustomerName = customers[1].Name,
                BranchId = branchId,
                BranchName = "Main Branch",
                Date = DateTime.UtcNow.AddMinutes(10)
            };
            var item2 = new SaleItem
            {
                ProductId = products[1].Id,
                ProductName = products[1].Name,
                Quantity = 3,
                UnitPrice = products[1].Price
            };
            item2.ApplyBusinessRules();
            sale2.AddItem(item2);
            sale2.RecalculateTotal();
            ctx.Sales.Add(sale2);

            var sale3 = new Sale
            {
                SaleNumber = "S-0003",
                CustomerId = customers[2].Id,
                CustomerName = customers[2].Name,
                BranchId = branchId,
                BranchName = "Main Branch",
                Date = DateTime.UtcNow.AddMinutes(20)
            };
            var item3 = new SaleItem
            {
                ProductId = products[2].Id,
                ProductName = products[2].Name,
                Quantity = 5,
                UnitPrice = products[2].Price
            };
            item3.ApplyBusinessRules();
            sale3.AddItem(item3);
            sale3.RecalculateTotal();
            ctx.Sales.Add(sale3);

            var sale4 = new Sale
            {
                SaleNumber = "S-0004",
                CustomerId = customers[3].Id,
                CustomerName = customers[3].Name,
                BranchId = branchId,
                BranchName = "Main Branch",
                Date = DateTime.UtcNow.AddMinutes(30)
            };
            var item4 = new SaleItem
            {
                ProductId = products[3].Id,
                ProductName = products[3].Name,
                Quantity = 2,
                UnitPrice = products[3].Price
            };
            item4.ApplyBusinessRules();
            sale4.AddItem(item4);
            sale4.RecalculateTotal();
            ctx.Sales.Add(sale4);

            var sale5 = new Sale
            {
                SaleNumber = "S-0005",
                CustomerId = customers[4].Id,
                CustomerName = customers[4].Name,
                BranchId = branchId,
                BranchName = "Main Branch",
                Date = DateTime.UtcNow.AddMinutes(40)
            };
            var item5 = new SaleItem
            {
                ProductId = products[4].Id,
                ProductName = products[4].Name,
                Quantity = 6,
                UnitPrice = products[4].Price
            };
            item5.ApplyBusinessRules();
            sale5.AddItem(item5);
            sale5.RecalculateTotal();
            ctx.Sales.Add(sale5);

            ctx.SaveChanges();
        }


        if (!ctx.Users.Any())
        {
            var user1 = new User
            {
                Username = "Huxley",
                Password = BCrypt.Net.BCrypt.HashPassword("123"),
                Email = "huxley@gmail.com",
                Phone = "0000000000",
                Status = UserStatus.Active,
                Role = UserRole.None
            };

            var user2 = new User
            {
                Username = "Teste",
                Password = BCrypt.Net.BCrypt.HashPassword("abc"),
                Email = "teste@gmail.com",
                Phone = "0000000000",
                Status = UserStatus.Active,
                Role = UserRole.None
            };

            ctx.Users.AddRange(user1, user2);
            ctx.SaveChanges();
        }
    }
}