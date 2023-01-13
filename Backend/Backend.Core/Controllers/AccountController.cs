using Backend.Core.Identity;
using Backend.Core.Models.User;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(UserManager<ApplicationUser> userManager) =>
        _userManager = userManager;

    [HttpGet]
    public async Task<AccountResponse> GetAsync()
    {
        var found = await _userManager.GetUserAsync(User);

        return new AccountResponse(found!.Id, found.Email!, found.EmailConfirmed, found.UserName!);
    }
}