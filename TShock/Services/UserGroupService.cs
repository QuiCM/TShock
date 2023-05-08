using TerrariaServerApi.Services;
using TShock.Database;
using Microsoft.EntityFrameworkCore;
using TShock.Models;

namespace TShock.Services;

/// <summary>
/// Service providing group-related functionality.
/// </summary>
public sealed class UserGroupService //: PluginService
{
	private readonly IDbContextFactory<UserDataDbContext> _context;

	/// <summary>
	/// TODO
	/// </summary>
	/// <param name="contextFactory"></param>
	public UserGroupService(IDbContextFactory<UserDataDbContext> contextFactory)
	{
		_context = contextFactory;
	}


	/// <summary>
	/// TODO
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public UserGroup? GetGroup(string name)
	{
		using UserDataDbContext db = _context.CreateDbContext();
		return db.UserGroups.Find(name);
	}
}
