using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Threading;
using System.Threading.Tasks;

using Terraria;
using Terraria.IO;

using TerrariaServerApi.Services;

using TShock.Settings;

namespace TShock.Services;

/// <summary>
/// Service providing world saving functionality.
/// </summary>
public sealed class WorldSaveService : PluginService
{
	private readonly IOptionsMonitor<SaveSettings> _saveSettings;
	private readonly CancellationToken _cancellationToken;
	private readonly ILogger<WorldSaveService> _logger;
	private readonly EventWaitHandle _waitHandle;

	/// <summary>
	/// Constructs a new world save service
	/// </summary>
	/// <param name="saveSettings">Monitored save settings</param>
	/// <param name="applicationLifetime">ApplicationLifetime providing access to lifetime events</param>
	/// <param name="logger">Logger instance for this service</param>
	public WorldSaveService(IOptionsMonitor<SaveSettings> saveSettings,
							IHostApplicationLifetime applicationLifetime,
							ILogger<WorldSaveService> logger)
	{
		_saveSettings = saveSettings;
		_cancellationToken = applicationLifetime.ApplicationStopping;
		_logger = logger;
		_waitHandle = new EventWaitHandle(true, EventResetMode.AutoReset);
	}

	/// <summary>
	/// Triggers a background world save.
	/// </summary>
	/// <param name="resetTime">Whether the world's time should be reset to daytime</param>
	/// <param name="worldFileName">Optional name to save the world under during this save operation</param>
	public void SaveWorld(bool resetTime = false, string? worldFileName = null)
	{
		QueueWorldSave(resetTime: resetTime, worldFileName: worldFileName);
	}

	/// <summary>
	/// Queues a world save.
	/// </summary>
	/// <param name="resetTime">Whether the world's time should be reset to daytime</param>
	/// <param name="worldFileName">Optional name to save the world under during this save operation</param>
	/// <returns>An awaitable task that will complete when the save has completed.</returns>
	public Task QueueWorldSave(bool resetTime = false, string? worldFileName = null)
	{
		return Task.Run(() =>
		{
			if (WaitHandle.WaitAny(new[] { _waitHandle, _cancellationToken.WaitHandle }) == 0)
			{
				try
				{
					string originalWorldPath = Main.worldPathName;

					Main.ActiveWorldFileData._path = worldFileName ?? originalWorldPath;
					WorldFile.SaveWorld(useCloudSaving: false, resetTime: resetTime);
					Main.ActiveWorldFileData._path = originalWorldPath;

					SaveSettings settings = _saveSettings.CurrentValue;
					// TSPlayer.All.SendInfoMessage(settings.SaveMessage); TODO: reimplement
					_logger.LogInformation("World saved at {WorldPath}", Main.worldPathName);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "World save failed");
				}

				_waitHandle.Set(); // Set the wait handle so the next queued save can proceed
			}
		}, _cancellationToken);
	}
}
