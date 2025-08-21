using MediatR;
using Ambev.DeveloperEvaluation.Application.Common;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Customers.List
{
    public class ListCustomersQuery : IRequest<PagedResult<ListCustomersItem>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class ListCustomersItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public ListCustomersItem() { }

        public ListCustomersItem(string id, string name, string? email, string? phone)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
        }
    }
}
