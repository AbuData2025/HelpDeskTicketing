using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelpDeskTicketing.Pages
{
    public class StatusCodeModel : PageModel
    {
        public int Code { get; set; }
        public string Title { get; set; } = "Something went wrong";
        public string Message { get; set; } = "Please try again, or head back to the homepage.";

        public void OnGet(int code)
        {
            Code = code;

            (Title, Message) = code switch
            {
                404 => ("Page not found", "The page you're looking for doesn't exist — it may have been moved or the link is outdated."),
                403 => ("Access denied", "You don't have permission to view this page. If you think this is a mistake, contact an administrator."),
                _ => ("Something went wrong", "An unexpected error occurred. Please try again.")
            };
        }
    }
}