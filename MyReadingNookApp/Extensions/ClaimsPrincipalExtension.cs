using System.Security.Claims;

namespace MyReadingNookApp.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) throw new UnauthorizedAccessException("User ID claim is missing.");
        return int.Parse(claim.Value);
    }
}