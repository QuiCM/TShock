
using Captain.Attributes;
using Captain.Commanding;
using Microsoft.Extensions.Logging;
using TShock.Models;
using TShock.Services;

namespace TShock.Commands;

[Command("Account", "User")]
[Description("Command group for managing TShock User Accounts")]
[Authenticated(loginRequired: false, permission: "tshock.account")]
[AccessibleTo(Accessibility.Server | Accessibility.Player)]
public class UserAccountCommand : TShockCommandService
{
	private readonly UserAccountService _accountService;
	private readonly UserGroupService _groupService;
	private readonly ILogger<UserAccountCommand> _logger;

	public UserAccountCommand(TShockUser receiver,
							  UserAccountService accountService,
							  UserGroupService groupService,
							  ILogger<UserAccountCommand> logger)
		: base(receiver)
	{
		_accountService = accountService;
		_groupService = groupService;
		_logger = logger;
	}


	[Command("Create", "Add", "New")]
	[Description("Create a new account, optionally setting a password")]
	[Authenticated(loginRequired: true, permission: "tshock.account.create")]
	[Sensitive]
	public void Create(string accountName, UserGroup userGroup, string? password = null)
	{
		//TODO: use password or set temp
		//password = password ?? GenerateTemporaryPassword()
		_accountService.AddUserAccount(new()
		{
			Name = accountName,
			Group = userGroup
		});

		Receiver.SendSuccess($"Created new account '{accountName}'");
	}

	[Command("Delete", "Remove")]
	[Description("Delete an account and its associated data")]
	[Authenticated(loginRequired: true, permission: "tshock.account.delete")]
	public void Delete(UserAccount account)
	{
		_accountService.RemoveUserAccount(account);

		Receiver.SendSuccess($"Removed account '{account.Name}'");
	}


	[Command("Register")]
	[Description("Registers a new account")]
	[Authenticated(loginRequired: false, permission: "tshock.account.register")]
	[Sensitive]
	public void Register(string? username)
	{
		username = username ?? Receiver.Name; // Use character name if no user name provided
		UserGroup? defaultGroup = _groupService.GetGroup("default"); //TODO: use config for default group

		if (defaultGroup == null)
		{
			Receiver.SendError("Failed to register account"); //TODO: provide brief explanation of group config issue
			return;
		}

		//TODO: generate temporary password
		_accountService.AddUserAccount(new()
		{
			Name = username,
			Group = defaultGroup
		});

		Receiver.SendSuccess("Registered new user");
	}

	[Command("Login")]
	[Description("Login to an account")]
	[Authenticated(loginRequired: false, permission: "tshock.account.login")]
	[Sensitive]
	public void Login(string? username, string? password)
	{
		UserAccount? account = _accountService.GetUserAccount(username ?? Receiver.Name);

		if (account == null)
		{
			Receiver.SendError("No account found");
			return;
		}

		bool loggedIn = false; //AttemptLogin(account, uuid, password)

		if (!loggedIn)
		{
			Receiver.SendError("Authentication failed");
		}
		else
		{
			Receiver.SendSuccess("Logged in");
		}
	}
}
