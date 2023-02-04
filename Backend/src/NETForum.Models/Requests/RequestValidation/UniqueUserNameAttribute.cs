using System.ComponentModel.DataAnnotations;

using NETForum.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace NETForum.Models.Requests.RequestValidation;

public class UniqueUserNameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var manager = validationContext.GetRequiredService<UserManager<ApplicationUser>>();

        var user = manager.FindByNameAsync((value as string)!).GetAwaiter().GetResult();

        return user is null ? 
            ValidationResult.Success : 
            new ValidationResult("UserName is already taken");
    }
}