using System;
using System.Collections.Generic;

namespace mat_modelirovanije.Models;

public partial class Job
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
