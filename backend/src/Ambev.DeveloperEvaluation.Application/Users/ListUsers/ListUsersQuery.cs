using MediatR;
using Ambev.DeveloperEvaluation.Application.Common;
using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.User.List;

public class ListUsersQuery : IRequest<PagedResult<ListUsersItem>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class ListUsersItem
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    public ListUsersItem() { }

    public ListUsersItem(Guid id, string userName, string email)
    {
        Id = id;
        UserName = userName;
        Email = email;
    }
}