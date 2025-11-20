namespace EventTrackerAPI.Models.Dtos
{
    public class TaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Priority { get; set; } = null!;
        public DateTime? DueDate { get; set; }
    }

    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? AssignedTo { get; set; }
    }
}
