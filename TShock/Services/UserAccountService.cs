using TerrariaServerApi.Services;
using TShock.Database;
using Microsoft.EntityFrameworkCore;
using TShock.Models;

namespace TShock.Services;

/// <summary>
/// Service providing account-related functionality.
/// </summary>
public sealed class UserAccountService //: PluginService
{
	private readonly IDbContextFactory<UserDataDbContext> _context;

	/// <summary>
	/// TODO
	/// </summary>
	/// <param name="contextFactory"></param>
	public UserAccountService(IDbContextFactory<UserDataDbContext> contextFactory)
	{
		_context = contextFactory;
	}

	/// <summary>
	/// Retrieves user by exact name match
	/// </summary>
	/// <param name="name">Name of the user to find</param>
	/// <returns>The user with the given name, or null if no user matched</returns>
	public UserAccount? GetUserAccount(string name)
	{
		using UserDataDbContext userDb = _context.CreateDbContext();
		return userDb.UserAccounts.Find(name);
	}

	/// <summary>
	/// TODO
	/// </summary>
	/// <param name="userAccount"></param>
	public void AddUserAccount(UserAccount userAccount)
	{
		using UserDataDbContext userDb = _context.CreateDbContext();
		userDb.UserAccounts.Attach(userAccount);
	}

	/// <summary>
	/// TODO
	/// </summary>
	/// <param name="userAccount"></param>
	public void RemoveUserAccount(UserAccount userAccount)
	{
		using UserDataDbContext userDb = _context.CreateDbContext();
		userDb.UserAccounts.Remove(userAccount);
	}
}
