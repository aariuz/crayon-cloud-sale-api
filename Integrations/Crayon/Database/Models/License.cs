using System;
using System.Collections.Generic;

namespace Integrations.Crayon.Database.Models;

public partial class License
{
    public int Id { get; set; }

    public string Key { get; set; }

    public int SubscriptionId { get; set; }

    public virtual Subscription Subscription { get; set; }
}
