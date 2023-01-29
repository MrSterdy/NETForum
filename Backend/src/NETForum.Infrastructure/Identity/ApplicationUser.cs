using Microsoft.AspNetCore.Identity;

namespace NETForum.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<int>
{
    [ProtectedPersonalData]
    public string? NewEmail { get; set; }

    public bool Banned { get; set; }
}