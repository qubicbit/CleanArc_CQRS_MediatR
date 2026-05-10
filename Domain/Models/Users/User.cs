using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Users;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = [];

    public string Role { get; set; } = "User"; // får user som default vid reg.


}
