using MediatR;

namespace Recallio.Mediatr.Auth.Queries;

public class ResetPasswordCheckAccessQuery: IRequest
{
    public ResetPasswordCheckAccessQuery(string url)
    {
        Url = Url ?? throw new ArgumentNullException(nameof(Url));;
    }

    public string Url { get; set; }
}