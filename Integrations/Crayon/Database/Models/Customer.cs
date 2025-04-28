using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;

namespace Integrations.Crayon.Database.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string Salt { get; set; }

    public string RefreshToken {  get; set; }
    
    public DateTime? RefreshTokenExpiry {  get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
