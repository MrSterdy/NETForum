using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Database;

public static class DbInitializer
{
    public static void InitializeDatabase(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();

        using var context = scope.ServiceProvider.GetRequiredService<Context>();

        context.Database.Migrate();

        if (context.Roles.Any(role => role.Name == "Admin"))
            return;
        
        context.Roles.Add(new IdentityRole<int>("Admin")
        {
            NormalizedName = "ADMIN"
        });

        context.SaveChanges();
    }
}