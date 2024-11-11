using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class News
{
    public int NewsId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateOnly PublicationDate { get; set; }

    public int? AuthorId { get; set; }

    public int? HomestayId { get; set; }

    public int? TourId { get; set; }

    public int? ProviderId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Admin? Author { get; set; }

    public virtual Homestay? Homestay { get; set; }

    public virtual Provider? Provider { get; set; }

    public virtual Tour? Tour { get; set; }
}
