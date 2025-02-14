using System;
using System.Collections.Generic;

namespace mat_modelirovanije.Models;

public partial class EducationCalendar
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();
}
