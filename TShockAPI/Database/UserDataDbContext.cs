using Microsoft.EntityFrameworkCore;
using TShockAPI.Models;

namespace TShockAPI.Database;

/// <summary>
/// Database context defining player character data
/// </summary>
public class UserDataDbContext : DbContext
{
	public DbSet<UserGroup> UserGroups { get; set; }
	public DbSet<UserAccount> UserAccounts { get; set; }
	public DbSet<CharacterData> CharacterData { get; set; }

	public UserDataDbContext(DbContextOptions<UserDataDbContext> userDbOpts)
		: base(userDbOpts)
	{
	}

	/* protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserAccount>()
			.HasOne(account => account.Group)
			.WithMany(group => group.Accounts);

		modelBuilder.Entity<CharacterData>()
			.HasOne(character => character.UserAccount)
			.WithOne();

		base.OnModelCreating(modelBuilder);
	} */
}
