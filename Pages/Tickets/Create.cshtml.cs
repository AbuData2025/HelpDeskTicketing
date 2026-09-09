using HelpDeskTicketing.Data;
using HelpDeskTicketing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelpDeskTicketing.Pages.Tickets
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Ticket Ticket { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            // Only bind the fields the user should be able to set directly —
            // never trust Status, CreatedAt, or AssignedToUserId from client input
            var ticket = new Ticket
            {
                Title = Ticket.Title,
                Description = Ticket.Description,
                Category = Ticket.Category,
                Priority = Ticket.Priority,
                Status = TicketStatus.Open,
                CreatedAt = DateTime.UtcNow,
                SubmittedByUserId = user.Id
            };

            ModelState.Clear();
            TryValidateModel(ticket, nameof(Ticket));

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return RedirectToPage("./MyTickets");
        }
    }
}