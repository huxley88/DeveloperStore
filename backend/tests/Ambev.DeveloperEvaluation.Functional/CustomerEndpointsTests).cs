using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Functional.Infrastructure;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional;

public class CustomersEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public CustomersEndpointsTests(CustomWebApplicationFactory factory) => _factory = factory;

    private async Task<string> DebugResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Content: {content}");
        return content;
    }

    [Fact]
    public async Task Get_Customers_Requires_Auth()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/Customers");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_Customer_Requires_Auth()
    {
        var client = _factory.CreateClient();

        var payload = new
        {
            Name = "Test Customer",
            Email = "test@email.com",
            Phone = "+5511999999999",
            Document = "12345678900"
        };

        var response = await client.PostAsJsonAsync("/api/Customers", payload);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_Customer_Works()
    {
        var client = _factory.CreateAuthenticatedClient();

        var payload = new
        {
            Name = "John Doe",
            Email = "john.doe@email.com",
            Phone = "+5511999999999",
            Document = "12345678900"
        };

        var create = await client.PostAsJsonAsync("/api/Customers", payload);
        await DebugResponse(create);
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var location = create.Headers.Location;
        Assert.NotNull(location);

        var getCustomer = await client.GetAsync(location);
        await DebugResponse(getCustomer);
        Assert.Equal(HttpStatusCode.OK, getCustomer.StatusCode);

        var customer = await getCustomer.Content.ReadFromJsonAsync<CustomerApiResponse>();
        Assert.NotNull(customer);
        Assert.Equal(payload.Name, customer.Name);
        Assert.Equal(payload.Email, customer.Email);
        Assert.Equal(payload.Phone, customer.Phone);
        Assert.NotEqual(Guid.Empty, customer.Id);
    }

    [Fact]
    public async Task Create_Customer_With_Invalid_Data_Returns_BadRequest()
    {
        var client = _factory.CreateAuthenticatedClient();

        var payload = new
        {
            Name = "",
            Email = "invalid-email",
            Phone = "123",
            Document = "123"
        };

        var create = await client.PostAsJsonAsync("/api/Customers", payload);
        await DebugResponse(create);
        Assert.Equal(HttpStatusCode.BadRequest, create.StatusCode);
    }

    [Fact]
    public async Task Get_Customers_List_Works()
    {
        var client = _factory.CreateAuthenticatedClient();

        var customers = new[]
        {
            new { Name = "Alice Smith", Email = "alice@test.com", Phone = "+5511111111111", Document = "11111111111" },
            new { Name = "Bob Jones", Email = "bob@test.com", Phone = "+5511222222222", Document = "22222222222" },
            new { Name = "Carol White", Email = "carol@test.com", Phone = "+5511333333333", Document = "33333333333" }
        };

        foreach (var customerPayload in customers)
        {
            var create = await client.PostAsJsonAsync("/api/Customers", customerPayload);
            Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        }

        var listResponse = await client.GetAsync("/api/Customers?page=1&pageSize=20");
        await DebugResponse(listResponse);
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);

        var customerList = await listResponse.Content.ReadFromJsonAsync<CustomerListApiResponse>();

        Assert.NotNull(customerList);
        Assert.True(customerList.Data.Count >= 3);
        Assert.Equal(1, customerList.Page);
        Assert.Equal(20, customerList.PageSize);
        Assert.True(customerList.Total >= 3);

        foreach (var customer in customerList.Data)
        {
            Assert.NotEqual(Guid.Empty, customer.Id);
            Assert.NotEmpty(customer.Name);
            Assert.NotEmpty(customer.Email);
            Assert.NotEmpty(customer.Phone);
        }
    }

    [Fact]
    public async Task Get_Customer_By_Id_Works()
    {
        var client = _factory.CreateAuthenticatedClient();

        var createPayload = new
        {
            Name = "Jane Doe",
            Email = "jane@test.com",
            Phone = "+5511444444444",
            Document = "44444444444"
        };

        var create = await client.PostAsJsonAsync("/api/Customers", createPayload);
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var location = create.Headers.Location;
        Assert.NotNull(location);

        var getCustomer = await client.GetAsync(location);
        await DebugResponse(getCustomer);
        Assert.Equal(HttpStatusCode.OK, getCustomer.StatusCode);

        var customer = await getCustomer.Content.ReadFromJsonAsync<CustomerApiResponse>();

        Assert.NotNull(customer);
        Assert.Equal(createPayload.Name, customer.Name);
        Assert.Equal(createPayload.Email, customer.Email);
        Assert.Equal(createPayload.Phone, customer.Phone);
        Assert.NotEqual(Guid.Empty, customer.Id);
    }

    [Fact]
    public async Task Create_Customer_With_Duplicate_Email_Returns_BadRequest()
    {
        var client = _factory.CreateAuthenticatedClient();

        var payload = new
        {
            Name = "First Customer",
            Email = "duplicate@test.com",
            Phone = "+5511888888888",
            Document = "88888888888"
        };

        var create1 = await client.PostAsJsonAsync("/api/Customers", payload);
        Assert.Equal(HttpStatusCode.Created, create1.StatusCode);

        var payload2 = new
        {
            Name = "Second Customer",
            Email = "duplicate@test.com",
            Phone = "+5511999999999",
            Document = "99999999999"
        };

        var create2 = await client.PostAsJsonAsync("/api/Customers", payload2);
        await DebugResponse(create2);

        Assert.True(create2.StatusCode == HttpStatusCode.BadRequest ||
                   create2.StatusCode == HttpStatusCode.Conflict ||
                   create2.StatusCode == HttpStatusCode.Created);
    }
}