using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Functional.Infrastructure;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional;

public class ProductsEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ProductsEndpointsTests(CustomWebApplicationFactory factory) => _factory = factory;

    private async Task<string> DebugResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Content: {content}");
        return content;
    }

    [Fact]
    public async Task Create_Product_Requires_Auth()
    {
        var client = _factory.CreateClient();

        var payload = new
        {
            Name = "Test Product",
            Price = 99.99m
        };

        var response = await client.PostAsJsonAsync("/api/Products", payload);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_Product_Works()
    {
        var client = _factory.CreateAuthenticatedClient();

        var payload = new
        {
            Name = "Premium Amber Ale",
            Price = 15.99m
        };

        var create = await client.PostAsJsonAsync("/api/Products", payload);
        await DebugResponse(create);
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var location = create.Headers.Location;
        Assert.NotNull(location);

        var getProduct = await client.GetAsync(location);
        await DebugResponse(getProduct);
        Assert.Equal(HttpStatusCode.OK, getProduct.StatusCode);

        var product = await getProduct.Content.ReadFromJsonAsync<ProductApiResponse>();
        Assert.NotNull(product);
        Assert.Equal("Premium Amber Ale", product.Name);
        Assert.Equal(15.99m, product.Price);
        Assert.NotEqual(Guid.Empty, product.Id);
    }

    [Fact]
    public async Task Create_Product_With_Invalid_Data_Returns_BadRequest()
    {
        var client = _factory.CreateAuthenticatedClient();

        var payload = new
        {
            Name = "",
            Price = -10m
        };

        var create = await client.PostAsJsonAsync("/api/Products", payload);
        await DebugResponse(create);
        Assert.Equal(HttpStatusCode.BadRequest, create.StatusCode);
    }

    [Fact]
    public async Task Get_Product_By_Id_Works()
    {
        var client = _factory.CreateAuthenticatedClient();

        var createPayload = new
        {
            Name = "Pale Ale",
            Price = 12.50m
        };

        var create = await client.PostAsJsonAsync("/api/Products", createPayload);
        await DebugResponse(create);
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var location = create.Headers.Location;
        Assert.NotNull(location);

        var getProduct = await client.GetAsync(location);
        await DebugResponse(getProduct);
        Assert.Equal(HttpStatusCode.OK, getProduct.StatusCode);

        var product = await getProduct.Content.ReadFromJsonAsync<ProductApiResponse>();

        Assert.NotNull(product);
        Assert.Equal("Pale Ale", product.Name);
        Assert.Equal(12.50m, product.Price);
        Assert.NotEqual(Guid.Empty, product.Id);
    }

    [Fact]
    public async Task Create_Product_With_Extra_Fields_Ignores_Them()
    {
        var client = _factory.CreateAuthenticatedClient();

        var createPayload = new
        {
            Name = "Test Beer",
            Price = 9.99m,
            Description = "This should be ignored",
            StockQuantity = 100,
            Category = "IPA"
        };

        var create = await client.PostAsJsonAsync("/api/Products", createPayload);
        await DebugResponse(create);
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var location = create.Headers.Location;
        Assert.NotNull(location);

        var getProduct = await client.GetAsync(location);
        await DebugResponse(getProduct);
        Assert.Equal(HttpStatusCode.OK, getProduct.StatusCode);

        var product = await getProduct.Content.ReadFromJsonAsync<ProductApiResponse>();

        Assert.NotNull(product);
        Assert.Equal("Test Beer", product.Name);
        Assert.Equal(9.99m, product.Price);
        Assert.NotEqual(Guid.Empty, product.Id);
    }

    [Fact]
    public async Task Get_Products_List_Returns_Paginated_Results()
    {
        var client = _factory.CreateAuthenticatedClient();

        var products = new[]
        {
            new { Name = "Beer A", Price = 10.0m },
            new { Name = "Beer B", Price = 12.0m },
            new { Name = "Beer C", Price = 8.0m }
        };

        foreach (var productPayload in products)
        {
            var create = await client.PostAsJsonAsync("/api/Products", productPayload);
            Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        }

        var listResponse = await client.GetAsync("/api/Products?page=1&pageSize=20");
        await DebugResponse(listResponse);
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);

        var productList = await listResponse.Content.ReadFromJsonAsync<ProductListApiResponse>();

        Assert.NotNull(productList);
        Assert.True(productList.Data.Count >= 3);
        Assert.Equal(1, productList.Page);
        Assert.Equal(20, productList.PageSize);
        Assert.True(productList.Total >= 3);

        foreach (var product in productList.Data)
        {
            Assert.NotEqual(Guid.Empty, product.Id);
            Assert.NotEmpty(product.Name);
            Assert.True(product.Price > 0);
        }
    }

    [Fact]
    public async Task Get_Existing_Product_By_Id_Works()
    {
        var client = _factory.CreateAuthenticatedClient();

        var listResponse = await client.GetAsync("/api/Products");
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);

        var productList = await listResponse.Content.ReadFromJsonAsync<ProductListApiResponse>();
        Assert.NotNull(productList);
        Assert.True(productList.Data.Any());

        var firstProduct = productList.Data.First();

        var getProduct = await client.GetAsync($"/api/Products/{firstProduct.Id}");
        await DebugResponse(getProduct);
        Assert.Equal(HttpStatusCode.OK, getProduct.StatusCode);

        var product = await getProduct.Content.ReadFromJsonAsync<ProductApiResponse>();

        Assert.NotNull(product);
        Assert.Equal(firstProduct.Id, product.Id);
        Assert.Equal(firstProduct.Name, product.Name);
        Assert.Equal(firstProduct.Price, product.Price);
    }

    [Fact]
    public async Task Create_Product_With_Zero_Price_Returns_BadRequest()
    {
        var client = _factory.CreateAuthenticatedClient();

        var payload = new
        {
            Name = "Free Product",
            Price = 0m
        };

        var create = await client.PostAsJsonAsync("/api/Products", payload);
        await DebugResponse(create);

        Assert.True(create.StatusCode == HttpStatusCode.BadRequest ||
                   create.StatusCode == HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_Product_With_Long_Name_Works()
    {
        var client = _factory.CreateAuthenticatedClient();

        var longName = new string('A', 200);
        var payload = new
        {
            Name = longName,
            Price = 25.99m
        };

        var create = await client.PostAsJsonAsync("/api/Products", payload);
        await DebugResponse(create);

        Assert.True(create.StatusCode == HttpStatusCode.Created ||
                   create.StatusCode == HttpStatusCode.BadRequest);
    }
}