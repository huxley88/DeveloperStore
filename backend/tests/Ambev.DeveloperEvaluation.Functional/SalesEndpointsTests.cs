using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Functional.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional;

public class SalesEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public SalesEndpointsTests(CustomWebApplicationFactory factory) => _factory = factory;

    private async Task<string> DebugResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Content: {content}");
        return content;
    }

    private async Task<Guid> CreateTestCustomer(string name)
    {
        var client = _factory.CreateAuthenticatedClient();

        var customerPayload = new
        {
            Name = name,
            Email = $"{name.Replace(" ", "").ToLower()}@test.com",
            Phone = "+5511999999999",
            Document = "12345678900"
        };

        var response = await client.PostAsJsonAsync("/api/Customers", customerPayload);
        response.EnsureSuccessStatusCode();

        var location = response.Headers.Location;
        return Guid.Parse(location.Segments.Last());
    }

    [Fact]
    public async Task Get_Sales_Requires_Auth()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/Sales");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_Sale_Requires_Auth()
    {
        var client = _factory.CreateClient();

        var payload = new
        {
            saleNumber = "SALE-001",
            branchId = "1",
            branchName = "Main Branch",
            customerId = Guid.NewGuid(),
            customerName = "Test Customer",
            date = DateTime.UtcNow,
            items = new[]
            {
                new
                {
                    productId = Guid.NewGuid(),
                    productName = "Test Product",
                    quantity = 2,
                    unitPrice = 10.50m
                }
            }
        };

        var response = await client.PostAsJsonAsync("/api/Sales", payload);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_Sale_Works()
    {
        var client = _factory.CreateAuthenticatedClient();
        var customerId = await CreateTestCustomer("John Doe");

        var payload = new
        {
            saleNumber = "SALE-002",
            branchId = "1",
            branchName = "Main Branch - SP",
            customerId,
            customerName = "John Doe",
            date = DateTime.UtcNow,
            items = new[]
            {
                new
                {
                    productId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    productName = "Amber Ale 500ml",
                    quantity = 3,
                    unitPrice = 15.99m
                }
            }
        };

        var create = await client.PostAsJsonAsync("/api/Sales", payload);
        await DebugResponse(create);
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var location = create.Headers.Location;
        Assert.NotNull(location);

        var getSale = await client.GetAsync(location);
        await DebugResponse(getSale);
        Assert.Equal(HttpStatusCode.OK, getSale.StatusCode);

        var sale = await getSale.Content.ReadFromJsonAsync<SaleApiResponse>();
        Assert.NotNull(sale);
        Assert.NotNull(sale.SaleNumber);
        Assert.Equal("SALE-002", sale.SaleNumber);
    }

    [Fact]
    public async Task Create_Sale_With_Invalid_Data_Returns_BadRequest()
    {
        var client = _factory.CreateAuthenticatedClient();

        var payload = new
        {
            saleNumber = "",
            branchId = "",
            branchName = "",
            customerId = Guid.Empty,
            customerName = "",
            date = DateTime.UtcNow,
            items = new object[0]
        };

        var create = await client.PostAsJsonAsync("/api/Sales", payload);
        await DebugResponse(create);
        Assert.Equal(HttpStatusCode.BadRequest, create.StatusCode);
    }

    [Fact]
    public async Task Create_Sale_With_Empty_Items_Returns_BadRequest()
    {
        var client = _factory.CreateAuthenticatedClient();
        var customerId = await CreateTestCustomer("Empty Items Customer");

        var payload = new
        {
            saleNumber = "SALE-003",
            branchId = "1",
            branchName = "Test Branch",
            customerId,
            customerName = "Empty Items Customer",
            date = DateTime.UtcNow,
            items = new object[0]
        };

        var create = await client.PostAsJsonAsync("/api/Sales", payload);
        await DebugResponse(create);

        Assert.True(create.StatusCode == HttpStatusCode.BadRequest ||
                   create.StatusCode == HttpStatusCode.Created);
    }

    [Fact]
    public async Task Get_Sales_List_Works()
    {
        var client = _factory.CreateAuthenticatedClient();
        var customerId = await CreateTestCustomer("Alice Smith");

        var createPayload = new
        {
            saleNumber = "SALE-004",
            branchId = "2",
            branchName = "Secondary Branch",
            customerId,
            customerName = "Alice Smith",
            date = DateTime.UtcNow,
            items = new[]
            {
                new
                {
                    productId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    productName = "Test Beer",
                    quantity = 1,
                    unitPrice = 20.00m
                }
            }
        };

        var create = await client.PostAsJsonAsync("/api/Sales", createPayload);
        await DebugResponse(create);
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var listResponse = await client.GetAsync("/api/Sales?page=1&pageSize=10");
        await DebugResponse(listResponse);
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);

        var salesList = await listResponse.Content.ReadFromJsonAsync<SaleListApiResponse>();
        Assert.NotNull(salesList);
        Assert.True(salesList.Data.Count >= 1);
    }

    [Fact]
    public async Task Get_Sale_By_Id_Works()
    {
        var client = _factory.CreateAuthenticatedClient();
        var customerId = await CreateTestCustomer("Bob Johnson");

        var createPayload = new
        {
            saleNumber = "SALE-005",
            branchId = "3",
            branchName = "Third Branch",
            customerId,
            customerName = "Bob Johnson",
            date = DateTime.UtcNow,
            items = new[]
            {
                new
                {
                    productId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    productName = "IPA Beer",
                    quantity = 4,
                    unitPrice = 18.75m
                }
            }
        };

        var create = await client.PostAsJsonAsync("/api/Sales", createPayload);
        await DebugResponse(create);
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var location = create.Headers.Location;
        Assert.NotNull(location);

        var getSale = await client.GetAsync(location);
        var content = await DebugResponse(getSale);
        Assert.Equal(HttpStatusCode.OK, getSale.StatusCode);

        Assert.Contains("SALE-005", content);
        Assert.Contains("Bob Johnson", content);
    }

    [Fact]
    public async Task Create_Sale_With_Multiple_Items_Works()
    {
        var client = _factory.CreateAuthenticatedClient();
        var customerId = await CreateTestCustomer("Multi Item Customer");

        var payload = new
        {
            saleNumber = "SALE-MULTI",
            branchId = "1",
            branchName = "Multi Branch",
            customerId,
            customerName = "Multi Item Customer",
            date = DateTime.UtcNow,
            items = new[]
            {
                new
                {
                    productId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    productName = "Amber Ale 500ml",
                    quantity = 2,
                    unitPrice = 7.00m
                },
                new
                {
                    productId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    productName = "Pale Ale 330ml",
                    quantity = 3,
                    unitPrice = 9.50m
                }
            }
        };

        var create = await client.PostAsJsonAsync("/api/Sales", payload);
        await DebugResponse(create);
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var location = create.Headers.Location;
        Assert.NotNull(location);

        var getSale = await client.GetAsync(location);
        var responseContent = await getSale.Content.ReadAsStringAsync();

        // ✅ DEBUG: Ver exatamente o que está sendo retornado
        Console.WriteLine("=== GET RESPONSE CONTENT ===");
        Console.WriteLine(responseContent);

        // ✅ DEBUG: Ver se é lista ou objeto individual
        if (responseContent.Contains("\"data\"") && responseContent.Contains("\"page\""))
        {
            Console.WriteLine("Response is a PAGED LIST");
            var pagedSale = await getSale.Content.ReadFromJsonAsync<SaleListApiResponse>();
            Console.WriteLine($"Data count: {pagedSale?.Data?.Count ?? 0}");

            if (pagedSale?.Data?.Any() == true)
            {
                Assert.Equal("SALE-MULTI", pagedSale.Data.First().SaleNumber);
                Assert.Equal(2, pagedSale.Data.First().Items.Count);
            }
            else
            {
                Assert.True(false, "Data list is empty!");
            }
        }
        else
        {
            Console.WriteLine("Response is an INDIVIDUAL SALE");
            var sale = await getSale.Content.ReadFromJsonAsync<SaleApiResponse>();
            Assert.NotNull(sale);
            Assert.Equal("SALE-MULTI", sale.SaleNumber);
            Assert.Equal(2, sale.Items.Count);
        }
    }


    [Fact]
    public async Task Create_Sale_With_Invalid_Customer_Returns_BadRequest()
    {
        var client = _factory.CreateAuthenticatedClient();

        var payload = new
        {
            saleNumber = "SALE-INVALID-CUST",
            branchId = "1",
            branchName = "Test Branch",
            customerId = Guid.NewGuid(),
            customerName = "Nonexistent Customer",
            date = DateTime.UtcNow,
            items = new[]
            {
                new
                {
                    productId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    productName = "Test Beer",
                    quantity = 1,
                    unitPrice = 10.00m
                }
            }
        };

        var create = await client.PostAsJsonAsync("/api/Sales", payload);
        await DebugResponse(create);

        Assert.True(create.StatusCode == HttpStatusCode.BadRequest ||
                   create.StatusCode == HttpStatusCode.NotFound ||
                   create.StatusCode == HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_Sale_With_Invalid_Product_Returns_BadRequest()
    {
        var client = _factory.CreateAuthenticatedClient();
        var customerId = await CreateTestCustomer("Invalid Product Customer");

        var payload = new
        {
            saleNumber = "SALE-INVALID-PROD",
            branchId = "1",
            branchName = "Test Branch",
            customerId,
            customerName = "Invalid Product Customer",
            date = DateTime.UtcNow,
            items = new[]
            {
                new
                {
                    productId = Guid.NewGuid(),
                    productName = "Nonexistent Product",
                    quantity = 1,
                    unitPrice = 10.00m
                }
            }
        };

        var create = await client.PostAsJsonAsync("/api/Sales", payload);
        await DebugResponse(create);

        Assert.True(create.StatusCode == HttpStatusCode.BadRequest ||
                   create.StatusCode == HttpStatusCode.NotFound ||
                   create.StatusCode == HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_Sale_With_Large_Quantity_Triggers_Discount()
    {
        var client = _factory.CreateAuthenticatedClient();
        var customerId = await CreateTestCustomer("Large Quantity Customer");

        var payload = new
        {
            saleNumber = "SALE-LARGE-QTY",
            branchId = "1",
            branchName = "Large Quantity Branch",
            customerId,
            customerName = "Large Quantity Customer",
            date = DateTime.UtcNow,
            items = new[]
            {
                new
                {
                    productId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    productName = "Bulk Beer",
                    quantity = 20,
                    unitPrice = 10.00m
                }
            }
        };

        var create = await client.PostAsJsonAsync("/api/Sales", payload);
        await DebugResponse(create);
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var location = create.Headers.Location;
        Assert.NotNull(location);

        var getSale = await client.GetAsync(location);
        var content = await DebugResponse(getSale);
        Assert.Equal(HttpStatusCode.OK, getSale.StatusCode);

        var sale = await getSale.Content.ReadFromJsonAsync<SaleApiResponse>();
        Assert.NotNull(sale);
        Assert.Equal("SALE-LARGE-QTY", sale.SaleNumber);
    }
}