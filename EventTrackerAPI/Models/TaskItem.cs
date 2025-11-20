using System;
using System.Collections.Generic;

namespace EventTrackerAPI.Models;

public partial class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Priority { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int? AssignedTo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DueDate { get; set; }


}
