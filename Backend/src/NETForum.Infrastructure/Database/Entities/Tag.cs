using System.ComponentModel.DataAnnotations;

namespace NETForum.Infrastructure.Database.Entities;

public class Tag
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(sbyte.MaxValue)]
    public string Name { get; set; } = default!;
}