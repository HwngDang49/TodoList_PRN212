using System;
using System.Collections.Generic;

namespace Todolist_GroupY.DAL.Entities;

public partial class Todo
{
    public int TodoId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? ReminderTime { get; set; }

    public bool? IsCompleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
