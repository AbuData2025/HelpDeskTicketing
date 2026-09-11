namespace HelpDeskTicketing.Models
{
    public class TicketActivity
    {
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public string ActorUserId { get; set; } = string.Empty;
        public ApplicationUser? ActorUser { get; set; }
    }
}