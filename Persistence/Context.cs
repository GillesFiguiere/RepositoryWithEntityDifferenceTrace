using Microsoft.EntityFrameworkCore;
using Test.Persistence.Entities;

namespace Test.Persistence;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) {}
    public DbSet<Attachment> Attachments { get; set; } = null!;
}