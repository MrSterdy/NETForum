namespace Backend.Database.Entities;

public class User : Entity
{
    public string Username { get; set; }

    public User(string username) =>
        Username = username;
}