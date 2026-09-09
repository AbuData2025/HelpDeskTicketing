using HelpDeskTicketing.Data;
using HelpDeskTicketing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskTicketing.Pages.Agent
{
    [Authorize(Roles = "ITAgent,Admin")]
    public class AllTicketsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AllTicketsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Ticket> Tickets { get; set; } = new List<Ticket>();

        public async Task OnGetAsync()
        {
            Tickets = await _context.Tickets
                .Include(t => t.SubmittedByUser)
                .Include(t => t.AssignedToUser)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAssignToMeAsync(int ticketId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return NotFound();

            ticket.AssignedToUserId = user.Id;
            ticket.Status = TicketStatus.InProgress;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int ticketId, TicketStatus newStatus)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return NotFound();

            ticket.Status = newStatus;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}