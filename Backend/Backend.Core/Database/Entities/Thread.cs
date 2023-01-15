﻿using System.ComponentModel.DataAnnotations;

using Backend.Core.Identity;

namespace Backend.Core.Database.Entities;

public class Thread
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public ApplicationUser User { get; set; } = default!;

    [Required, MinLength(4), MaxLength(sbyte.MaxValue)]
    public string Title { get; set; } = default!;

    [Required, MinLength(4), MaxLength(short.MaxValue)]
    public string Content { get; set; } = default!;

    public IEnumerable<Comment> Comments = default!;
}