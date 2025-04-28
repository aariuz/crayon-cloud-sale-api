using System;
using System.Collections.Generic;

namespace Integrations.Crayon.Database.Models;

public partial class Subscription
{
    public int Id { get; set; }

    public int SoftwareId { get; set; }

    public string SoftwareName { get; set; }

    public string SoftwareDescription { get; set; }

    public bool Active { get; set; }

    public DateTime ValidUntil { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; }

    public virtual ICollection<License> Licenses { get; set; } = new List<License>();
}
