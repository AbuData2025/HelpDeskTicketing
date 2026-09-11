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

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public TicketStatus? StatusFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public TicketPriority? PriorityFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "newest";

        public async Task OnGetAsync()
        {
            var query = _context.Tickets
                .Include(t => t.SubmittedByUser)
                .Include(t => t.AssignedToUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(t =>
                    t.Title.Contains(SearchTerm) ||
                    t.Description.Contains(SearchTerm));
            }

            if (StatusFilter.HasValue)
            {
                query = query.Where(t => t.Status == StatusFilter.Value);
            }

            if (PriorityFilter.HasValue)
            {
                query = query.Where(t => t.Priority == PriorityFilter.Value);
            }

            query = SortBy switch
            {
                "oldest" => query.OrderBy(t => t.CreatedAt),
                "priority" => query.OrderByDescending(t => t.Priority),
                _ => query.OrderByDescending(t => t.CreatedAt)
            };

            Tickets = await query.ToListAsync();
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

            _context.TicketActivities.Add(new TicketActivity
            {
                TicketId = ticket.Id,
                ActorUserId = user.Id,
                Description = $"Assigned to {user.FullName}",
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return RedirectToPage(new { SearchTerm, StatusFilter, PriorityFilter, SortBy });
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int ticketId, TicketStatus newStatus)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return NotFound();

            var oldStatus = ticket.Status;
            ticket.Status = newStatus;
            ticket.UpdatedAt = DateTime.UtcNow;

            _context.TicketActivities.Add(new TicketActivity
            {
                TicketId = ticket.Id,
                ActorUserId = user.Id,
                Description = $"Status changed from {oldStatus} to {newStatus}",
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return RedirectToPage(new { SearchTerm, StatusFilter, PriorityFilter, SortBy });
        }
    
    }
}