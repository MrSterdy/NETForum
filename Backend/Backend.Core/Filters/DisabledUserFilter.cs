using Backend.Core.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend.Core.Filters;

public class DisabledUserFilter : IAsyncActionFilter
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    private readonly SignInManager _signInManager;

    public DisabledUserFilter(UserManager<ApplicationUser> userManager, SignInManager signInManager)
    {
        _userManager = userManager;

        _signInManager = signInManager;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var user = await _userManager.GetUserAsync(context.HttpContext.User);
        
        if (user is not null && !user.Enabled)
            await _signInManager.SignOutAsync();

        await next();
    }
}