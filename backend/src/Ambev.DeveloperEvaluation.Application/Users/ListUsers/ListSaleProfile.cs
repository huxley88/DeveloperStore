using AutoMapper;
using Ambev.DeveloperEvaluation.Application.User.List;

namespace Ambev.DeveloperEvaluation.Application.Users.List;

public class ListUsersProfile : Profile
{
    public ListUsersProfile()
    {
        CreateMap<Domain.Entities.User, ListUsersItem>();
    }
}