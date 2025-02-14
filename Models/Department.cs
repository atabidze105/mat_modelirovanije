using System;
using System.Collections.Generic;

namespace mat_modelirovanije.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? DepartmentDirector { get; set; }

    public virtual Employee? DepartmentDirectorNavigation { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Department> LowerDeps { get; set; } = new List<Department>();

    public virtual ICollection<Department> MainDeps { get; set; } = new List<Department>();
}
