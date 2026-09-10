using HelpDeskTicketing.Models;

namespace HelpDeskTicketing.Helpers
{
    public static class TicketStyleHelpers
    {
        public static string StatusBadgeClass(TicketStatus status) => status switch
        {
            TicketStatus.Open => "badge-status-open",
            TicketStatus.InProgress => "badge-status-inprogress",
            TicketStatus.Resolved => "badge-status-resolved",
            TicketStatus.Closed => "badge-status-closed",
            _ => "badge-status-open"
        };

        public static string PriorityBadgeClass(TicketPriority priority) => priority switch
        {
            TicketPriority.Low => "badge-priority-low",
            TicketPriority.Medium => "badge-priority-medium",
            TicketPriority.High => "badge-priority-high",
            TicketPriority.Critical => "badge-priority-critical",
            _ => "badge-priority-low"
        };
    }
}