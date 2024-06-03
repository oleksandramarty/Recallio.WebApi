using Recallio.Mediatr.Auth.Queries;
using MediatR;

namespace Recallio.Mediatr.Auth.Handlers;

public class ChangePasswordCommandHandler: IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}