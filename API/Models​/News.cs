﻿using System;
using System.Collections.Generic;

namespace API.Models​;

public partial class News
{
    public int NewId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public DateTime? Createdate { get; set; }
}
