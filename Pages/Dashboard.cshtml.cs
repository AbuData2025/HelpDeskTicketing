using HelpDeskTicketing.Data;
using HelpDeskTicketing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskTicketing.Pages
{
    [Authorize(Roles = "ITAgent,Admin")]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int OpenCount { get; set; }
        public int InProgressCount { get; set; }
        public int ResolvedCount { get; set; }
        public int ClosedCount { get; set; }
        public int TotalCount { get; set; }

        public async Task OnGetAsync()
        {
            OpenCount = await _context.Tickets.CountAsync(t => t.Status == TicketStatus.Open);
            InProgressCount = await _context.Tickets.CountAsync(t => t.Status == TicketStatus.InProgress);
            ResolvedCount = await _context.Tickets.CountAsync(t => t.Status == TicketStatus.Resolved);
            ClosedCount = await _context.Tickets.CountAsync(t => t.Status == TicketStatus.Closed);
            TotalCount = OpenCount + InProgressCount + ResolvedCount + ClosedCount;
        }
    }
}