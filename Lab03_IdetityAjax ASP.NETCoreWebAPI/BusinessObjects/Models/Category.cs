﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Orchid> Orchids { get; set; } = new List<Orchid>();
}
