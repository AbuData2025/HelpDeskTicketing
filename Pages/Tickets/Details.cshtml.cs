using HelpDeskTicketing.Data;
using HelpDeskTicketing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskTicketing.Pages.Tickets
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Ticket Ticket { get; set; } = default!;
        public List<TicketComment> Comments { get; set; } = new();
        public List<TicketActivity> Activities { get; set; } = new();

        [BindProperty]
        public string NewCommentBody { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var ticket = await _context.Tickets
                .Include(t => t.SubmittedByUser)
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null) return NotFound();

            // Security check: an EndUser can only view their own ticket.
            // ITAgent/Admin can view any ticket.
            bool isStaff = User.IsInRole("ITAgent") || User.IsInRole("Admin");
            if (!isStaff && ticket.SubmittedByUserId != user.Id)
            {
                return Forbid();
            }

            Ticket = ticket;
            Comments = await _context.TicketComments
                .Include(c => c.AuthorUser)
                .Where(c => c.TicketId == id)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();

            Activities = await _context.TicketActivities
                .Include(a => a.ActorUser)
                .Where(a => a.TicketId == id)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddCommentAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            bool isStaff = User.IsInRole("ITAgent") || User.IsInRole("Admin");
            if (!isStaff && ticket.SubmittedByUserId != user.Id)
            {
                return Forbid();
            }

            if (!string.IsNullOrWhiteSpace(NewCommentBody))
            {
                _context.TicketComments.Add(new TicketComment
                {
                    Body = NewCommentBody,
                    TicketId = id,
                    AuthorUserId = user.Id,
                    CreatedAt = DateTime.UtcNow
                });

                _context.TicketActivities.Add(new TicketActivity
                {
                    TicketId = id,
                    ActorUserId = user.Id,
                    Description = "Added a comment",
                    CreatedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id });
        }
    }
}