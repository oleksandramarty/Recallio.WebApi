using Recallio.Models.Responses.Users;
using MediatR;

namespace Recallio.Mediatr.Users.Requests;

public class CurrentUserQuery: IRequest<CurrentUserResponse>
{
    public CurrentUserQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; set; }
}