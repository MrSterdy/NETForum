using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Account;

public record ChangeEmailRequest(
    [Required, EmailAddress] string Email
);