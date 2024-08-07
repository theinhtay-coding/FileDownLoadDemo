using FileDownLoadDemo.Models;
using JWTAuthDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace FileDownLoadDemo;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<FileEntity> Files { get; set; }
    public DbSet<UserModel> Users { get; set; }
    public DbSet<RoleModel> Roles { get; set; }
}
