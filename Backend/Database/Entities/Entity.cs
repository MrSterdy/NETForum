using System.ComponentModel.DataAnnotations;

namespace Backend.Database.Entities;

public abstract class Entity
{
    [Key]
    public int Id { get; private set; }
}