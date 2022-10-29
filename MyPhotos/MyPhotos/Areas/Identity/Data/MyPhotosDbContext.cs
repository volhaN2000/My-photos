using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyPhotos.Areas.Identity.Data;

namespace MyPhotos.Areas.Identity.Data;

public class MyPhotosDbContext : IdentityDbContext<MyPhotosUser>
{
    public MyPhotosDbContext(DbContextOptions<MyPhotosDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new MyPhotosUserEntityConfiguration());
    }
}

public class MyPhotosUserEntityConfiguration : IEntityTypeConfiguration<MyPhotosUser>
{
    public void Configure(EntityTypeBuilder<MyPhotosUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(255);
        builder.Property(u => u.LastName).HasMaxLength(255);


    }
}