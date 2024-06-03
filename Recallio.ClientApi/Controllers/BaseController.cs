﻿using Recallio.Mediatr.Base.Requests;
using Recallio.Kernel.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Recallio.Kernel.Extensions.Controllers;

[Route("api-Recallio/[controller]")]
[ApiController]
[ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status409Conflict)]
[ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status417ExpectationFailed)]
[ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status500InternalServerError)]
public class BaseController: ControllerBase
{
    private IConfiguration _configuration;
    private readonly IMediator _mediatr;

    public BaseController(
        IConfiguration configuration,
        IMediator mediator
    )
    {
        _configuration = configuration;
        _mediatr = mediator;
    }
    
    protected async Task<Guid> GetUserIdAsync(CancellationToken cancellationToken)
    {
        return await this._mediatr.Send(new GetUserIdQuery(User), cancellationToken);
    }

}