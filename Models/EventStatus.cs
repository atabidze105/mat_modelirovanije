using System;
using System.Collections.Generic;

namespace mat_modelirovanije.Models;

public partial class EventStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
