using System;
using System.Collections.Generic;

namespace mat_modelirovanije.Models;

public partial class EducationType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();
}
