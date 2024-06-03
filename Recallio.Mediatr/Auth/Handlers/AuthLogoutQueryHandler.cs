using Recallio.Interfaces;
using Recallio.Mediatr.Auth.Queries;
using MediatR;

namespace Recallio.Mediatr.Auth.Handlers;

public class AuthLogoutQueryHandler: IRequestHandler<AuthLogoutQuery>
{
    private readonly IAuthService _authService;

    public AuthLogoutQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task Handle(AuthLogoutQuery query, CancellationToken cancellationToken)
    {
        // query.UserId = Get User Id
        await this._authService.LogOutUserAsync(query.UserId, cancellationToken);
    }
}