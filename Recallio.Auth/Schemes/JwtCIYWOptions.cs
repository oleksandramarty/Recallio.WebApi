using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Recallio.Auth.Schemes;

public class JwtRecallioOptions : AuthenticationSchemeOptions
{
    public string Realm { get; set; }
}

public class JwtNbxPostConfigureOptions : IPostConfigureOptions<JwtRecallioOptions>
{
    public void PostConfigure(string name, JwtRecallioOptions options)
    {
        if (string.IsNullOrEmpty(options.Realm))
        {
            throw new InvalidOperationException("Realm must be provided in options");
        }
    }
}