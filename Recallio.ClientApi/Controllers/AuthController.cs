﻿using Recallio.Mediatr.Auth.Queries;
using Recallio.Models.Responses.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Recallio.Kernel.Extensions.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController: BaseController
{
    private IConfiguration _configuration;
    private readonly IMediator _mediatr;
    
    public AuthController(
        IConfiguration configuration, 
        IMediator mediatr) : base(configuration, mediatr)
    {
        _configuration = configuration;
        _mediatr = mediatr;
    }
    
    [HttpPost("SignIn")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> SignInAsync([FromBody]SignInCommand command, CancellationToken cancellationToken)
    {
        await this._mediatr.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("Login")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    public async Task<IActionResult> LoginAsync([FromBody]AuthLoginQuery query, CancellationToken cancellationToken)
    {
        TokenResponse result = await this._mediatr.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("Logout")]
    [ProducesResponseType(typeof(void), 200)]
    [Authorize]
    public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
    {
        Guid userId = await this.GetUserIdAsync(cancellationToken);
        await this._mediatr.Send(new AuthLogoutQuery(userId), cancellationToken);
        return Ok(true);
    }

    [HttpPut("ChangePassword")]
    [ProducesResponseType(typeof(void), 200)]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync([FromBody]ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        await this._mediatr.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("Password/Forgot")]
    [ProducesResponseType(typeof(void), 200)]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody]ForgotPasswordQuery query, CancellationToken cancellationToken)
    {
        await this._mediatr.Send(query, cancellationToken);
        return Ok();
    }
    
    [HttpGet("Restore/{url}")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPasswordCheckAccessAsync([FromRoute] string url,
        CancellationToken cancellationToken)
    {
        await this._mediatr.Send(new ResetPasswordCheckAccessQuery(url), cancellationToken);
        return Ok();
    }

    [HttpPost("restore")]
    [AllowAnonymous]
    public async Task<IActionResult> RestorePasswordAsync([FromBody] RestorePasswordCommand command, CancellationToken cancellationToken)
    {
        await this._mediatr.Send(command, cancellationToken);
        return Ok();
    }

    [HttpGet("temporary")]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize]
    public async Task<IActionResult> CheckTemporaryPasswordAsync(CancellationToken cancellationToken)
    {
        bool result = await this._mediatr.Send(new CheckTemporaryPasswordQuery(), cancellationToken);
        return Ok(true);
    }
}