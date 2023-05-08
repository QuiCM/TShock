using System;
using Captain.Commanding;
using Microsoft.Extensions.Logging;

namespace TShock;

public class SystemUser : IInvoker
{
	public string Name => "System";

	private ILogger<SystemUser> _logger;

	public SystemUser(ILogger<SystemUser> logger)
	{
		_logger = logger;
	}

	public void SendSuccess(string message, params object[] args)
	{
		_logger.LogInformation(message, args);
	}

	public void SendWarning(string message, params object[] args)
	{
		_logger.LogWarning(message, args);
	}

	public void SendError(string message, Exception? ex = null, params object[] args)
	{
		_logger.LogError(ex, message, args);
	}
}
