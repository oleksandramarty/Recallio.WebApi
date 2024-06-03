﻿using System.Security.Claims;
using Recallio.Const.Enum;
using Recallio.Const.Errors;
using Recallio.Const.Providers;
using Recallio.Domain;
using Recallio.Domain.Models.User;
using Recallio.Interfaces;
using Recallio.Kernel.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Recallio.Services;

public class AuthService: IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly DataContext _context;

    public AuthService(UserManager<User> userManager, DataContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    
    public async Task<string> GetAuthenticationTokenAsync(User user, string loginProvider, string tokenName)
    {
        return await _userManager.GetAuthenticationTokenAsync(user, loginProvider, tokenName);
    }
    
    public async Task<User> FindUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }
    
    public async Task UpdateUserLoginsAsync(Guid userId, IList<IdentityUserLogin<Guid>> entity,
        CancellationToken cancellationToken)
    {
        using (var transaction = this._context.Database.BeginTransaction())
        {
            try
            {
                var oldLogins = await _context.UserLogins.Where(_ => _.UserId == userId)
                    .ToListAsync(cancellationToken);
                if (oldLogins != null)
                {
                    _context.UserLogins.RemoveRange(oldLogins);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                await _context.UserLogins.AddRangeAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                transaction.Commit();
                return;
            }
            catch (LoggerException e)
            {
                transaction.Rollback();
                throw;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
    
    public async Task LogOutUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(_ => _.Id == userId, cancellationToken);
        if (user == null)
        {
            throw new LoggerException(ErrorMessages.UserNotFound, 404, userId, EntityTypeEnum.User.ToString());
        }

        var logins = await _context.UserLogins.Where(_ => _.UserId == user.Id).ToListAsync(cancellationToken);

        foreach (var login in logins)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, login.LoginProvider, TokenNameProvider.RecallioAuth);
        }
    }
    
    public async Task<Guid> GetUserIdAsync(ClaimsPrincipal principal, CancellationToken cancellationToken)
    {
        User user = await this._userManager.GetUserAsync(principal);

        if (user == null)
        {
            throw new LoggerException(ErrorMessages.UserNotFound, 404, null, EntityTypeEnum.User.ToString());
        }
        
        return user.Id;
    }
    
    public async Task<User> FindUserByLoginAsync(string loginProvider, string providerKey)
    {
        User user = await _userManager.FindByLoginAsync(loginProvider, providerKey);
        return user;
    }
    
    public async Task<IList<string>> GetRolesAsync(User user)
    {
        return await _userManager.GetRolesAsync(user);
    }
    
    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }
    
    public async Task<IdentityResult> SetAuthenticationTokenAsync(User user, string loginProvider,
        string tokenName, string tokenValue)
    {
        return await _userManager.SetAuthenticationTokenAsync(user, loginProvider, tokenName,
            tokenValue);
    }
}