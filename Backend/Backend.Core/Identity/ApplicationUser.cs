using Microsoft.AspNetCore.Identity;

namespace Backend.Core.Identity;

public class ApplicationUser : IdentityUser<int>
{
    [ProtectedPersonalData]
    public string? NewEmail { get; set; }

    public bool Enabled { get; set; } = true;
}