using Recallio.Mediatr.Auth.Queries;
using MediatR;

namespace Recallio.Mediatr.Auth.Handlers;

public class CheckTemporaryPasswordQueryHandler: IRequestHandler<CheckTemporaryPasswordQuery, bool>
{
    public async Task<bool> Handle(CheckTemporaryPasswordQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}