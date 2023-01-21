using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Account;

public record RequiredEmailRequest(
    [Required, EmailAddress] string Email
);