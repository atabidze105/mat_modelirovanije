using System;
using System.Collections.Generic;

namespace mat_modelirovanije.Models;

public partial class Event
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DatetimeStart { get; set; }

    public DateTime DatetimeEnd { get; set; }

    public string? Description { get; set; }

    public int IdType { get; set; }

    public int IdStatus { get; set; }

    public virtual EventStatus IdStatusNavigation { get; set; } = null!;

    public virtual EventType IdTypeNavigation { get; set; } = null!;

    public virtual ICollection<Employee> IdOrganisators { get; set; } = new List<Employee>();
}
