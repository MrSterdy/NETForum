using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Backend.Core.Identity;

public class SignInManager : SignInManager<IdentityUser<int>>
{
    public SignInManager(UserManager<IdentityUser<int>> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<IdentityUser<int>> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<IdentityUser<int>>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<IdentityUser<int>> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }

    public override async Task SignOutAsync() =>
        await Context.SignOutAsync(IdentityConstants.ApplicationScheme);
}