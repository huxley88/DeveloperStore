using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.User.List;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.List
{
    public class ListUsersHandler : IRequestHandler<ListUsersQuery, PagedResult<ListUsersItem>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ListUsersHandler> _logger;

        public ListUsersHandler(
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<ListUsersHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResult<ListUsersItem>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.ListAsync(request.Page, request.PageSize, cancellationToken);
            var total = await _userRepository.CountAsync(cancellationToken);

            var mapped = _mapper.Map<List<ListUsersItem>>(users);

            return new PagedResult<ListUsersItem>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total,
                Data = mapped
            };
        }
    }
}
