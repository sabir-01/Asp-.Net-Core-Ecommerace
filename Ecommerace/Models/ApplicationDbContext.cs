using Microsoft.EntityFrameworkCore;

namespace Ecommerace.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Admin> tbl_admin { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Catagory> tbl_Catagory { get; set; }
        public DbSet<Product> tbl_Product { get; set; }
        public DbSet<Carts> tbl_Carts { get; set; }
        public DbSet<Feedback> tbl_Feedback { get; set; }
        public DbSet<Faqs> tbl_Faqs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
        .HasOne(p => p.catagory)
        .WithMany(c => c.products)
        .HasForeignKey(p => p.cat_id);
        }
    }


}
