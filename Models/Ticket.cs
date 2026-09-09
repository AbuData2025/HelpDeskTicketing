using System.ComponentModel.DataAnnotations;

namespace HelpDeskTicketing.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        public TicketCategory Category { get; set; }

        public TicketPriority Priority { get; set; }

        public TicketStatus Status { get; set; } = TicketStatus.Open;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // The person who submitted the ticket
        [Required]
        public string SubmittedByUserId { get; set; } = string.Empty;
        public ApplicationUser? SubmittedByUser { get; set; }

        // The IT agent assigned to it (nullable — may be unassigned)
        public string? AssignedToUserId { get; set; }
        public ApplicationUser? AssignedToUser { get; set; }
    }
}