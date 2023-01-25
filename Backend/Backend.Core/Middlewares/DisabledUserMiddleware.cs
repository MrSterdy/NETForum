using Backend.Core.Identity;

using Microsoft.AspNetCore.Identity;

namespace Backend.Core.Middlewares;

public class DisabledUserMiddleware
{
    private readonly RequestDelegate _next;

    public DisabledUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext context, 
        UserManager<ApplicationUser> userManager, 
        SignInManager signInManager
    )
    {
        if (context.User.Identity!.IsAuthenticated)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (!user!.Enabled)
                await signInManager.SignOutAsync();
        }

        await _next(context);
    }
}