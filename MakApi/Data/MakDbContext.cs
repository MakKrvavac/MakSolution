using MakApi.Models.Domain;
using Microsoft.EntityFrameworkCore;
using File = MakApi.Models.Domain.File;
using Task = MakApi.Models.Domain.Task;

namespace MakApi.Data;

public class MakDbContext : DbContext
{
    public MakDbContext(DbContextOptions<MakDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Deadline> Deadlines { get; set; }
    public DbSet<Epic> Epics { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }
}