using Microsoft.EntityFrameworkCore;
using TerrariaApi.Server.Services;
using TShock.Database;

/// <summary>
/// Service providing character data related functionality.
/// </summary>
public sealed class CharacterDataService : PluginService
{
	private readonly IDbContextFactory<UserDataDbContext> _context;

	public CharacterDataService(IDbContextFactory<UserDataDbContext> contextFactory)
	{
		_context = contextFactory;
	}
}
