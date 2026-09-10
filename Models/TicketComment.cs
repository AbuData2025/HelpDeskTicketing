using System.ComponentModel.DataAnnotations;

namespace HelpDeskTicketing.Models
{
    public class TicketComment
    {
        public int Id { get; set; }

        [Required, StringLength(2000)]
        public string Body { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        [Required]
        public string AuthorUserId { get; set; } = string.Empty;
        public ApplicationUser? AuthorUser { get; set; }
    }
}