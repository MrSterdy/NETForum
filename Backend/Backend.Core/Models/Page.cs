namespace Backend.Core.Models;

public record Page<T>(IEnumerable<T> Items, bool IsLast);