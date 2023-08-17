using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data;

public class NzWalksAuthDbContext: IdentityDbContext
{
    public NzWalksAuthDbContext(DbContextOptions<NzWalksAuthDbContext> options): base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var readerRoleId = "00e7f87b-d5e6-48a1-b24b-58fc2e500be0";
        var writeRoleId = "2d48a0a0-f668-4420-9057-87d1acc46606";

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id=readerRoleId,
                ConcurrencyStamp=readerRoleId,
                Name="Reader",
                NormalizedName="Reader".ToUpper()
            },
             new IdentityRole
            {
                Id=writeRoleId,
                ConcurrencyStamp=writeRoleId,
                Name="Writer",
                NormalizedName="Writer".ToUpper()
            },
        };

        builder.Entity<IdentityRole>().HasData(roles);
    }
}