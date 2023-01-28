using AutoMapper;

using Backend.Core.Identity;
using Backend.Core.Models.User;

using Microsoft.AspNetCore.Identity;

namespace Backend.Core.Mapping;

public class AdminResolver<Destination> : IValueResolver<ApplicationUser, Destination, bool>
    where Destination : UserResponse
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminResolver(UserManager<ApplicationUser> userManager) =>
        _userManager = userManager;

    public bool Resolve(ApplicationUser source, Destination destination, bool destMember, ResolutionContext context) =>
        _userManager.IsInRoleAsync(source, "Admin").GetAwaiter().GetResult();
}