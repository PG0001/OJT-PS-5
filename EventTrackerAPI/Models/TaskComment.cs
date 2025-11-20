using System;
using System.Collections.Generic;

namespace EventTrackerAPI.Models;

public partial class TaskComment
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string CommentText { get; set; } = null!;

    public DateTime CreatedAt { get; set; }


}
