﻿using System.Security.Claims;
using Recallio.Domain.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Recallio.Interfaces;

public interface IAuthService
{
    Task<string> GetAuthenticationTokenAsync(User user, string loginProvider, string tokenName);
    Task<User> FindUserByIdAsync(string userId);
    Task UpdateUserLoginsAsync(Guid userId, IList<IdentityUserLogin<Guid>> entity, CancellationToken cancellationToken);
    Task LogOutUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<Guid> GetUserIdAsync(ClaimsPrincipal principal, CancellationToken cancellationToken);
    Task<User> FindUserByLoginAsync(string loginProvider, string providerKey);
    Task<IList<string>> GetRolesAsync(User user);
    Task<bool> CheckPasswordAsync(User user, string password);

    Task<IdentityResult> SetAuthenticationTokenAsync(User user, string loginProvider,
        string tokenName, string tokenValue);
}
