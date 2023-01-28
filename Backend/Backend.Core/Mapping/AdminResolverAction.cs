using AutoMapper;

using Backend.Core.Identity;
using Backend.Core.Models.User;

using Microsoft.AspNetCore.Identity;

namespace Backend.Core.Mapping;

public class AdminResolverAction<TDest> : IMappingAction<ApplicationUser, TDest> 
    where TDest : UserResponse
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminResolverAction(UserManager<ApplicationUser> userManager) =>
        _userManager = userManager;

    public void Process(ApplicationUser source, TDest destination, ResolutionContext context) =>
        destination.Admin = _userManager.IsInRoleAsync(source, "Admin").GetAwaiter().GetResult();
}