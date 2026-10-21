
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;

namespace WebBanHang.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}
