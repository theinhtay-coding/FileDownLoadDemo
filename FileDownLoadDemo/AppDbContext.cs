using FileDownLoadDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace FileDownLoadDemo;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<FileEntity> Files { get; set; }
}
