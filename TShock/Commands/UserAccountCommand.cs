
using Captain.Attributes;
using Microsoft.Extensions.Logging;
using TShock.Models;
using TShock.Services;

namespace TShock.Commands;

/// <summary>
/// TODO
/// </summary>
[Command("Account", "User")]
[Description("Command group for managing TShock User Accounts")]
[Authenticated(loginRequired: false, permission: "tshock.account")]
[AccessibleTo(typeof(TShockUser))]
public class UserAccountCommand : TShockCommandService
{
	private readonly UserAccountService _accountService;
	private readonly UserGroupService _groupService;

	/// <summary>
	/// TODO
	/// </summary>
	/// <param name="invoker"></param>
	/// <param name="logger"></param>
	/// <param name="accountService"></param>
	/// <param name="groupService"></param>
	public UserAccountCommand(TShockUser invoker,
		ILogger<UserAccountCommand> logger,
		UserAccountService accountService,
		UserGroupService groupService)
		: base(invoker, logger)
	{
		_accountService = accountService;
		_groupService = groupService;
	}


	/// <summary>
	/// TODO
	/// </summary>
	/// <param name="accountName"></param>
	/// <param name="userGroup"></param>
	/// <param name="password"></param>
	[Command("Create", "Add", "New")]
	[Description("Create a new account, optionally setting a password")]
	[Authenticated(loginRequired: true, permission: "tshock.account.create")]
	[Sensitive]
	//[AccessibleTo(typeof(TShockServer))]   TODO: AccessibleTo should be additive
	public void Create(string accountName, UserGroup userGroup, string? password = null)
	{
		//TODO: use password or set temp
		//password = password ?? GenerateTemporaryPassword()
		_accountService.AddUserAccount(new UserAccount()
		{
			Name = accountName,
			Group = userGroup
		});

		Invoker.SendSuccess($"Created new account '{accountName}'");
	}

	/// <summary>
	/// TODO
	/// </summary>
	/// <param name="account"></param>
	[Command("Delete", "Remove")]
	[Description("Delete an account and its associated data")]
	[Authenticated(loginRequired: true, permission: "tshock.account.delete")]
	//[AccessibleTo(typeof(TShockServer))]   TODO: AccessibleTo should be additive
	public void Delete(UserAccount account)
	{
		_accountService.RemoveUserAccount(account);

		Invoker.SendSuccess($"Removed account '{account.Name}'");
	}


	/// <summary>
	/// TODO
	/// </summary>
	/// <param name="username"></param>
	[Command("Register")]
	[Description("Registers a new account")]
	[Authenticated(loginRequired: false, permission: "tshock.account.register")]
	[Sensitive]
	public void Register(string? username)
	{
		username = username ?? Invoker.Name; // Use character name if no user name provided
		UserGroup? defaultGroup = _groupService.GetGroup("default"); //TODO: use config for default group

		if (defaultGroup == null)
		{
			Invoker.SendError("Failed to register account"); //TODO: provide brief explanation of group config issue
			return;
		}

		//TODO: generate temporary password
		_accountService.AddUserAccount(new()
		{
			Name = username,
			Group = defaultGroup
		});

		Invoker.SendSuccess("Registered new user");
	}

	/// <summary>
	/// TODO
	/// </summary>
	/// <param name="username"></param>
	/// <param name="password"></param>
	[Command("Login")]
	[Description("Login to an account")]
	[Authenticated(loginRequired: false, permission: "tshock.account.login")]
	[Sensitive]
	public void Login(string? username, string? password)
	{
		UserAccount? account = _accountService.GetUserAccount(username ?? Invoker.Name);

		if (account == null)
		{
			Invoker.SendError("No account found");
			return;
		}

		bool loggedIn = false; //AttemptLogin(account, uuid, password)

		if (!loggedIn)
		{
			Invoker.SendError("Authentication failed");
		}
		else
		{
			Invoker.SendSuccess("Logged in");
		}
	}
}
