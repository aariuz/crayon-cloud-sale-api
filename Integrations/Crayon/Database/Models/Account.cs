using System;
using System.Collections.Generic;

namespace Integrations.Crayon.Database.Models;

public partial class Account
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
