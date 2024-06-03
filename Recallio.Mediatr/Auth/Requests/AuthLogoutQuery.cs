using MediatR;

namespace Recallio.Mediatr.Auth.Queries;

public class AuthLogoutQuery: IRequest
{
    public AuthLogoutQuery(Guid userId)
    {
        UserId = userId;
    }
    public Guid UserId { get; set; }
}