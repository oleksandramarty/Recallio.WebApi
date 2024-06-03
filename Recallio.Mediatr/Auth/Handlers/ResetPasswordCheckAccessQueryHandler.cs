using Recallio.Mediatr.Auth.Queries;
using MediatR;

namespace Recallio.Mediatr.Auth.Handlers;

public class ResetPasswordCheckAccessQueryHandler: IRequestHandler<ResetPasswordCheckAccessQuery>
{
    public async Task Handle(ResetPasswordCheckAccessQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}