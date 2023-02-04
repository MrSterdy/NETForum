using System.ComponentModel.DataAnnotations;

using NETForum.Infrastructure.Database.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace NETForum.Models.Requests.RequestValidation;

public class UniqueTagNameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var repository = validationContext.GetRequiredService<ITagRepository>();

        return repository.Exists((value as string)!).GetAwaiter().GetResult() ?
            new ValidationResult("Tag Name is already taken") :
            ValidationResult.Success;
    }
}