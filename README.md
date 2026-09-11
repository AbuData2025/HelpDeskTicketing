# HelpDeskTicketing

IT support that doesn't live in a group chat.

## Why this exists

Most small teams don't have a dedicated IT department, so tech issues get handled the same way everything else does — a message in the group chat, a quick "hey can you look at this," and no real record of what happened. That works fine until three people report the same printer issue separately, or a fix gets forgotten because it scrolled out of view.

HelpDeskTicketing is a small, focused ticketing system built to fix exactly that gap: a simple way for small teams and growing organizations to log, assign, and track tech issues to resolution.

## Features

- Role-based accounts: **End User**, **IT Agent**, **Admin**
- End users submit tickets and track their own history
- Agents view all tickets, assign to themselves, and update status
- Full comment thread on every ticket, visible to both the submitter and staff
- Live dashboard showing ticket counts by status
- Admin panel for assigning/removing user roles — no database access required
- Server-side role enforcement on every restricted page, not just hidden nav links

## Tech Stack

- **ASP.NET Core 9** (Razor Pages)
- **Entity Framework Core 9** — code-first migrations
- **SQL Server** (LocalDB for development)
- **ASP.NET Core Identity** — authentication and role management
- **Bootstrap 5**, customized with a small design system (custom color tokens, Public Sans typeface, status/priority badge system)

## Data Model

- `ApplicationUser` — extends Identity's default user with a `FullName` field
- `Ticket` — title, description, category, priority, status, submitter, assigned agent
- `TicketComment` — threaded comments per ticket, tied to both the ticket and the author

Foreign keys use `Restrict` delete behavior on user references (so deleting a user never cascades into deleting their ticket history) and `Cascade` on comment-to-ticket (so comments are cleaned up if a ticket is ever deleted).

## Running Locally

Requires the .NET 9 SDK and SQL Server (LocalDB, included with Visual Studio, works fine).

1. Clone the repository
```bash
   git clone https://github.com/AbuData2025/HelpDeskTicketing.git
   cd HelpDeskTicketing
```
2. Restore packages
```bash
   dotnet restore
```
3. Apply migrations to create the database
```bash
   dotnet ef database update
```
4. Run the app
```bash
   dotnet run
```
5. Open the printed `localhost` URL, register an account (automatically assigned the End User role), and start submitting tickets.

To test the Agent/Admin views, promote your own account via the Admin panel — or, for your very first Admin account, insert the role directly in the `AspNetUserRoles` table (one-time setup only).

## Roadmap

- [ ] Search, filter, and sort on the All Tickets page
- [ ] Email notifications on ticket assignment/status change
- [ ] Deploy to Azure App Service for a live demo
- [ ] Ticket attachments (screenshots of the issue)

## Screenshots

*(Add screenshots of the Dashboard, ticket submission, and All Tickets pages here)*