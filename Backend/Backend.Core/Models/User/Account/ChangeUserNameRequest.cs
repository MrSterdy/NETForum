using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Account;

public record ChangeUserNameRequest(
    [Required, MinLength(4), MaxLength(16)] string UserName
);