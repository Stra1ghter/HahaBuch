using System.Security.Claims;
using HahaBuch.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HahaBuch;

public class VaultAccessor
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VaultAccessor(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Guid> GetUsersVaultId()
    {
        var claimsPrincipal = _httpContextAccessor.HttpContext!.User;
        if (claimsPrincipal == null || !claimsPrincipal.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }
        
        var userId = Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);
        ApplicationUserEntity userEntity = (await _context.Users.FindAsync(userId))!;
        return userEntity.VaultEntityId
            ?? throw new InvalidOperationException("User does not have a vault.");
    }
}