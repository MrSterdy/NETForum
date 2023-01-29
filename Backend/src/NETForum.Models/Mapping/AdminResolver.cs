using AutoMapper;

using NETForum.Models.Responses;
using NETForum.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;

namespace NETForum.Models.Mapping;

public class AdminResolver<TDestination> : IValueResolver<ApplicationUser, TDestination, bool>
    where TDestination : UserResponse
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminResolver(UserManager<ApplicationUser> userManager) =>
        _userManager = userManager;

    public bool Resolve(ApplicationUser source, TDestination destination, bool destMember, ResolutionContext context) =>
        _userManager.IsInRoleAsync(source, "Admin").GetAwaiter().GetResult();
}