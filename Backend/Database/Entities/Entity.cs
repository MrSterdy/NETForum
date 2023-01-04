using System.ComponentModel.DataAnnotations;

namespace Backend.Database.Entities;

public abstract class Entity
{
    [Key]
    public long Id { get; private set; }
}