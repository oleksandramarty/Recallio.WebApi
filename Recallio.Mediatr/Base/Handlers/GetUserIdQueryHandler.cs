using Recallio.Interfaces;
using Recallio.Mediatr.Base.Requests;
using MediatR;

namespace Recallio.Mediatr.Base.Handlers;

public class GetUserIdQueryHandler: IRequestHandler<GetUserIdQuery, Guid>
{
    private readonly IAuthService _authService;

    public GetUserIdQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Guid> Handle(GetUserIdQuery query, CancellationToken cancellationToken)
    {
        return await this._authService.GetUserIdAsync(query.User, cancellationToken);
    }
}