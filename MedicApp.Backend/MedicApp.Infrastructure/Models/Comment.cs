using System;
using System.Collections.Generic;

namespace MedicApp.Infrastructure.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int? HelpRequestId { get; set; }

    public int? AuthorId { get; set; }

    public string CommentText { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? Adequacy { get; set; }

    public virtual Account? Author { get; set; }

    public virtual MedicalHelpRequest? HelpRequest { get; set; }
}
