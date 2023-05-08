using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TerrariaServerApi.Configurator;


namespace TShock.Configurators;

/// <summary>
/// Configures the configuration provider, adding TShock's configuration options
/// </summary>
public class SettingsConfigurator : ConfigConfigurator
{
	/// <summary>
	/// The name of the file containing TShock's json configuration
	/// </summary>
	private const string TShockConfigFile = "tshock.json";
	private const string DefaultConfigRoot = "config";

	/// <summary>
	/// Construct a new SettingsConfigurator, setting a priority of 100
	/// </summary>
	public SettingsConfigurator() : base()
	{
		// Arbitrarily higher than 0. This configuration provided can be completely or partially overwritten
		// by a lower priority configurator
		Priority = 100;
	}

	/// <inheritdoc/>
	public override void Configure(HostBuilderContext hostContext, IConfigurationBuilder configBuilder, string[] args)
	{
		// Configuration is loaded as such:
		// commandline > environment vars > json configuration.
		// Environment variables and commandline are built first here, to determine if an alternative config path has been set.
		IConfiguration tshockEnvVars = new ConfigurationBuilder()
			.AddEnvironmentVariables("TSHOCK_")
			.AddCommandLine(args)
			.Build();

		// To set the config root via environment variable: export TSHOCK_CONFIG__ROOT=~/my/config dir/
		// To set the config root via commandline: dotnet run --config:root "~/my/config dir/"
		string configRoot = tshockEnvVars.GetSection("config:root").Value ??
		                    Path.Combine(AppContext.BaseDirectory, DefaultConfigRoot);

		Directory.CreateDirectory(configRoot);

		// We now build a second IConfiguration that first takes values from the json file,
		// then overwrites any duplicates with values from the environment variables, then the commandline
		IConfiguration tshockConfig = new ConfigurationBuilder()
			.SetBasePath(configRoot)
			.AddJsonFile(TShockConfigFile, optional: true, reloadOnChange: true)
			.AddConfiguration(tshockEnvVars)
			.Build();

		// Then we synchronise back to the IConfigurationBuilder we are configuring
		configBuilder.AddConfiguration(tshockConfig);
	}
}
