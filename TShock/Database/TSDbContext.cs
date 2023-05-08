using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using TShock.Settings;

namespace TShock.Database;

/// <summary>
/// Database context defining core TShock data
/// </summary>
public class TSDbContext : DbContext
{

	public TSDbContext(DbContextOptions<TSDbContext> tsdbContextOptions, IOptionsMonitor<ServerSettings> dbSettings)
		: base(tsdbContextOptions)
	{

	}
}
