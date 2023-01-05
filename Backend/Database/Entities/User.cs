using System.ComponentModel.DataAnnotations;

namespace Backend.Database.Entities;

public class User : Entity
{
    [Required, MinLength(4), MaxLength(16)]
    public string Username { get; set; }

    public User(string username) =>
        Username = username;
}