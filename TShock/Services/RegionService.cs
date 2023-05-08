

using Microsoft.EntityFrameworkCore;
using TerrariaServerApi.Services;
using TShock.Database;

namespace TShock.Services;

public class RegionService //: PluginService
{

	private readonly IDbContextFactory<RegionDbContext> _context;

	public RegionService(IDbContextFactory<RegionDbContext> contextFactory)
	{
		_context = contextFactory;
	}
}
