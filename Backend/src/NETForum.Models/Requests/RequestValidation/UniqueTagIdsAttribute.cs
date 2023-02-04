using System.ComponentModel.DataAnnotations;

using NETForum.Infrastructure.Database;

using Microsoft.Extensions.DependencyInjection;

namespace NETForum.Models.Requests.RequestValidation;

public class UniqueTagIdsAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var tagIds = value as int[];
        
        if (tagIds!.Length == 0)
            return ValidationResult.Success;

        if (tagIds.Distinct().Count() != tagIds.Length)
            return new ValidationResult("Tag Ids must contain unique values");
        
        var context = validationContext.GetRequiredService<ApplicationDbContext>();

        return context.Tags.Any(t => tagIds.All(id => t.Id != id)) ?
            new ValidationResult("Tag Ids contains invalid value") : 
            ValidationResult.Success;
    }
}