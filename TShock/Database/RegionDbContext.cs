using Microsoft.EntityFrameworkCore;
using TShock.Models;

namespace TShock.Database;

public class RegionDbContext : DbContext
{
	public DbSet<Region> Regions { get; set; }

	public RegionDbContext(DbContextOptions<RegionDbContext> options) : base(options)
	{
	}
}
