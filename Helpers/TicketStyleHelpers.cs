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

        public static string SlaBadge(Ticket ticket)
        {
            if (ticket.DueBy == null) return "";

            if (ticket.Status == TicketStatus.Resolved || ticket.Status == TicketStatus.Closed)
            {
                return "<span class=\"badge-pill badge-sla-ok\">Completed</span>";
            }

            if (DateTime.UtcNow > ticket.DueBy)
            {
                return "<span class=\"badge-pill badge-sla-breached\">Overdue</span>";
            }

            var hoursLeft = (ticket.DueBy.Value - DateTime.UtcNow).TotalHours;
            if (hoursLeft <= 24)
            {
                return "<span class=\"badge-pill badge-sla-warning\">Due soon</span>";
            }

            return "<span class=\"badge-pill badge-sla-ok\">On track</span>";
        }
    }
}