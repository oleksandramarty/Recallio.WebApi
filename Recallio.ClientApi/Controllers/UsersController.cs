﻿using Recallio.Mediatr.Users.Requests;
using Recallio.Models.Responses.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Recallio.Kernel.Extensions.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class UsersController: BaseController
{
    private IConfiguration _configuration;
    private readonly IMediator _mediatr;
    
    public UsersController(
        IConfiguration configuration, 
        IMediator mediatr) : base(configuration, mediatr)
    {
        _configuration = configuration;
        _mediatr = mediatr;
    }
    
    [HttpGet("Current")]
    [ProducesResponseType(typeof(CurrentUserResponse), 200)]
    [Authorize]
    public async Task<IActionResult> CurrentUserAsync(CancellationToken cancellationToken)
    {
        Guid userId = await this.GetUserIdAsync(cancellationToken);
        CurrentUserResponse result = await this._mediatr.Send(new CurrentUserQuery(userId), cancellationToken);
        return Ok(result);
    }
    
}