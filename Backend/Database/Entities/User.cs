using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Entities;

public class User : Entity
{
    public string Username { get; set; }

    [ForeignKey("UserId")]
    public IEnumerable<Thread> Threads => new List<Thread>();

    public User(string username) =>
        Username = username;
}