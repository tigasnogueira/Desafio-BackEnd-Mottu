using BikeRentalSystem.Infrastructure.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Infrastructure.Tests.Identity;

public class ApplicationDbContextTests
{
    private DbContextOptions<ApplicationDbContext> CreateDbContextOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void ApplicationDbContext_Should_Create_Database()
    {
        var options = CreateDbContextOptions();

        using (var context = new ApplicationDbContext(options))
        {
            var user = new IdentityUser
            {
                Id = "test-user",
                UserName = "testuser@example.com",
                NormalizedUserName = "TESTUSER@EXAMPLE.COM",
                Email = "testuser@example.com",
                NormalizedEmail = "TESTUSER@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = "hashedpassword",
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            };
            context.Users.Add(user);
            context.SaveChanges();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var userCount = context.Users.Count();
            userCount.Should().Be(1);
        }
    }

    [Fact]
    public void ApplicationDbContext_Should_Have_Identity_Tables()
    {
        var options = CreateDbContextOptions();

        using (var context = new ApplicationDbContext(options))
        {
            context.Users.Should().NotBeNull();
            context.Roles.Should().NotBeNull();
            context.UserRoles.Should().NotBeNull();
            context.UserClaims.Should().NotBeNull();
            context.UserLogins.Should().NotBeNull();
            context.UserTokens.Should().NotBeNull();
            context.RoleClaims.Should().NotBeNull();
        }
    }

    [Fact]
    public void ApplicationDbContext_Should_Add_And_Retrieve_User()
    {
        var options = CreateDbContextOptions();

        var user = new IdentityUser
        {
            Id = "test-user",
            UserName = "testuser@example.com",
            NormalizedUserName = "TESTUSER@EXAMPLE.COM",
            Email = "testuser@example.com",
            NormalizedEmail = "TESTUSER@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = "hashedpassword",
            SecurityStamp = Guid.NewGuid().ToString("D"),
            ConcurrencyStamp = Guid.NewGuid().ToString("D")
        };

        using (var context = new ApplicationDbContext(options))
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var retrievedUser = context.Users.SingleOrDefault(u => u.Id == user.Id);
            retrievedUser.Should().NotBeNull();
            retrievedUser.UserName.Should().Be("testuser@example.com");
        }
    }

    [Fact]
    public void ApplicationDbContext_Should_Not_Add_Invalid_User()
    {
        var options = CreateDbContextOptions();

        var user = new IdentityUser
        {
            Id = "test-user",
            UserName = null,
            Email = "testuser@example.com",
            NormalizedEmail = "TESTUSER@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = "hashedpassword",
            SecurityStamp = Guid.NewGuid().ToString("D"),
            ConcurrencyStamp = Guid.NewGuid().ToString("D")
        };

        using (var context = new ApplicationDbContext(options))
        {
            context.Users.Add(user);
            FluentActions.Invoking(() => context.SaveChanges())
                .Should().Throw<InvalidOperationException>();
        }
    }

    [Fact]
    public void ApplicationDbContext_Should_Add_And_Retrieve_Role()
    {
        var options = CreateDbContextOptions();

        var role = new IdentityRole
        {
            Id = "test-role",
            Name = "TestRole",
            NormalizedName = "TESTROLE"
        };

        using (var context = new ApplicationDbContext(options))
        {
            context.Roles.Add(role);
            context.SaveChanges();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var retrievedRole = context.Roles.SingleOrDefault(r => r.Id == role.Id);
            retrievedRole.Should().NotBeNull();
            retrievedRole.Name.Should().Be("TestRole");
        }
    }

    [Fact]
    public void ApplicationDbContext_Should_Not_Add_Invalid_Role()
    {
        var options = CreateDbContextOptions();

        var role = new IdentityRole
        {
            Id = "test-role",
            Name = null,
            NormalizedName = "TESTROLE"
        };

        using (var context = new ApplicationDbContext(options))
        {
            context.Roles.Add(role);
            FluentActions.Invoking(() => context.SaveChanges())
                .Should().Throw<InvalidOperationException>();
        }
    }

    [Fact]
    public void ApplicationDbContext_Should_Add_And_Retrieve_UserRole()
    {
        var options = CreateDbContextOptions();

        var user = new IdentityUser
        {
            Id = "test-user",
            UserName = "testuser@example.com",
            NormalizedUserName = "TESTUSER@EXAMPLE.COM",
            Email = "testuser@example.com",
            NormalizedEmail = "TESTUSER@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = "hashedpassword",
            SecurityStamp = Guid.NewGuid().ToString("D"),
            ConcurrencyStamp = Guid.NewGuid().ToString("D")
        };

        var role = new IdentityRole
        {
            Id = "test-role",
            Name = "TestRole",
            NormalizedName = "TESTROLE"
        };

        var userRole = new IdentityUserRole<string>
        {
            UserId = user.Id,
            RoleId = role.Id
        };

        using (var context = new ApplicationDbContext(options))
        {
            context.Users.Add(user);
            context.Roles.Add(role);
            context.UserRoles.Add(userRole);
            context.SaveChanges();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var retrievedUserRole = context.UserRoles.SingleOrDefault(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
            retrievedUserRole.Should().NotBeNull();
        }
    }

    [Fact]
    public void ApplicationDbContext_Should_Not_Add_Invalid_UserRole()
    {
        var options = CreateDbContextOptions();

        var userRole = new IdentityUserRole<string>
        {
            UserId = null,
            RoleId = "test-role"
        };

        using (var context = new ApplicationDbContext(options))
        {
            FluentActions.Invoking(() => context.UserRoles.Add(userRole))
                .Should().Throw<InvalidOperationException>();
        }
    }
}
