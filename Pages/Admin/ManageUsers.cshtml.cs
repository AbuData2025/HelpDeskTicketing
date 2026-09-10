using HelpDeskTicketing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelpDeskTicketing.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageUsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUsersModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<UserWithRoles> Users { get; set; } = new();
        public List<string> AllRoles { get; set; } = new();

        public class UserWithRoles
        {
            public string Id { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public IList<string> Roles { get; set; } = new List<string>();
        }

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        public async Task<IActionResult> OnPostAssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (!await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveRoleAsync(string userId, string role)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null && currentUser.Id == userId && role == "Admin")
            {
                // Prevent an admin from accidentally removing their own Admin access
                ModelState.AddModelError(string.Empty, "You cannot remove your own Admin role.");
                await LoadDataAsync();
                return Page();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            await _userManager.RemoveFromRoleAsync(user, role);
            return RedirectToPage();
        }

        private async Task LoadDataAsync()
        {
            AllRoles = _roleManager.Roles.Select(r => r.Name!).ToList();

            var allUsers = _userManager.Users.ToList();
            Users = new List<UserWithRoles>();

            foreach (var u in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(u);
                Users.Add(new UserWithRoles
                {
                    Id = u.Id,
                    Email = u.Email ?? "",
                    FullName = u.FullName,
                    Roles = roles
                });
            }
        }
    }
}