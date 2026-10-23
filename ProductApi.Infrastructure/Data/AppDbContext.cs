using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;

namespace ProductApi.Infrastructure.Data;
public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{ } 
	public DbSet<Product> Products {get; set;}
  	public DbSet<User> Users {get; set;}

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
 	{
	 modelBuilder.Entity<User>()
		   .HasIndex(u => u.Username)
		   .IsUnique();

	 modelBuilder.Entity<Product>()
		 .HasOne(p => p.Category)
		 .WithMany(c => c.Products)
		 .HasForeignKey(p => p.CategoryId);
    }
}
