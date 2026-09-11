using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HelpDeskTicketing.Models;

namespace HelpDeskTicketing.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ticket> Tickets { get; set; } = default!;
    public DbSet<TicketComment> TicketComments { get; set; } = default!;
    public DbSet<TicketActivity> TicketActivities { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // A user can submit many tickets — don't cascade delete the ticket if the user is removed
        builder.Entity<Ticket>()
            .HasOne(t => t.SubmittedByUser)
            .WithMany()
            .HasForeignKey(t => t.SubmittedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // A user can be assigned many tickets — same restriction to avoid multiple cascade paths
        builder.Entity<Ticket>()
            .HasOne(t => t.AssignedToUser)
            .WithMany()
            .HasForeignKey(t => t.AssignedToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TicketComment>()
            .HasOne(c => c.Ticket)
            .WithMany()
            .HasForeignKey(c => c.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

       builder.Entity<TicketComment>()
            .HasOne(c => c.AuthorUser)
            .WithMany()
            .HasForeignKey(c => c.AuthorUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TicketActivity>()
            .HasOne(a => a.Ticket)
            .WithMany()
            .HasForeignKey(a => a.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<TicketActivity>()
            .HasOne(a => a.ActorUser)
            .WithMany()
            .HasForeignKey(a => a.ActorUserId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}