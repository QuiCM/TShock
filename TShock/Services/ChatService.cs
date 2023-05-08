using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.UI.Chat;
using TerrariaServerApi.Services;
using TShock.Settings;
using ChatMessage = Terraria.Chat.ChatMessage;
using OnChatHelper = On.Terraria.Chat.ChatHelper;
using OnChatCommandProcessor = On.Terraria.Chat.ChatCommandProcessor;

namespace TShock.Services;

/// <summary>
///
/// </summary>
public class ChatService : PluginService
{
	private readonly ILogger<ChatService> _logger;
	private readonly IOptionsMonitor<ChatSettings> _settingsMonitor;

	/// <summary>
	///
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="settingsMonitor"></param>
	public ChatService(ILogger<ChatService> logger, IOptionsMonitor<ChatSettings> settingsMonitor)
	{
		_logger = logger;
		_settingsMonitor = settingsMonitor;
	}

	/// <inheritdoc />
	public override void Start()
	{
		ChatManager.Commands._commands.Clear();
		ChatManager.Commands._defaultCommand = null;

		// Invoked when an incoming command is identified
		// This occurs for pretty much any piece of text entered in the game, except emojis (/e #, /rps).
		// Regular chat messages are considered 'Say' commands
		OnChatCommandProcessor.ProcessIncomingMessage += CommandReceived;

		// Invoked when a message is sent and appears in chat
		// This includes server broadcasts, which will have an 'author' of 255.
		// Player messages have an author of their client index
		OnChatHelper.BroadcastChatMessageAs += ServerBroadcast;

		//Unknown - looks to be more-or-less the same as DisplayMessageOnClient
		OnChatHelper.SendChatMessageToClientAs += OnPlayerDirectedMessage;

		//'DM' a message to a client
		OnChatHelper.DisplayMessageOnClient += OnDisplayMessageToClient;
	}

	private void ServerBroadcast(OnChatHelper.orig_BroadcastChatMessageAs orig,
		byte author,
		NetworkText text,
		Color color,
		int exclude)
	{
		_logger.LogInformation("{ChatHelper} {Author} {@Value} {@Color} {Exclude}",
			nameof(ServerBroadcast),
			Main.player[author].name,
			text,
			color,
			exclude);

		orig(author, text, color, exclude);
	}

	private void OnPlayerDirectedMessage(OnChatHelper.orig_SendChatMessageToClientAs orig,
		byte author,
		NetworkText text,
		Color color,
		int recipient)
	{
		_logger.LogInformation("{ChatHelper} {Author} {Recipient} {Value} {@Color}",
			nameof(ServerBroadcast),
			author,
			recipient,
			text.ToString(),
			color);

		orig(author, text, color, recipient);
	}

	private void OnDisplayMessageToClient(OnChatHelper.orig_DisplayMessageOnClient orig,
		NetworkText text,
		Color color,
		int recipient)
	{
		_logger.LogInformation("{ChatHelper} {Recipient} {Value} {@Color}",
			nameof(ServerBroadcast),
			recipient,
			text.ToString(),
			color);

		orig(text, color, recipient);
	}

	private void CommandReceived(OnChatCommandProcessor.orig_ProcessIncomingMessage orig,
		ChatCommandProcessor self,
		ChatMessage message,
		int author)
	{
		// Mark the message as consumed so Terraria doesn't do stuff with it?
		message.Consume();

		ChatSettings settings = _settingsMonitor.CurrentValue;

		// We override the Terraria in-built commands completely, so we need to figure out which command is being used
		// and finesse the message text appropriately, such that we have a message that contains the whole
		// command string.
		// E.g., '/me emote text' would be sent as something like { CommandId: Emote, Text: 'emote text' }
		// We rebuild this into a single string of '/me emote text' and then pass that down to our own command processor
		LocalizedText? localizedText = self._localizedCommands
			.FirstOrDefault(kvp => kvp.Value._name == message.CommandId._name)
			.Key;

		string messageText = message.Text;
		if (localizedText != null)
		{
			messageText = $"{localizedText} {messageText}";
		}

		if (messageText.StartsWith(settings.CommandPrefix) || messageText.StartsWith(settings.SuppressedCommandPrefix))
		{
			// Is a command, handle it appropriately
			HandleCommand(messageText, author);
		}
		else
		{
			// Not a command, rebroadcast it as a message in our own message format
			HandlePlayerChat(messageText, author);
		}

		// Deliberately do not call orig(...), we don't want default command handling to exist
	}

	private void HandlePlayerChat(string message, int author)
	{
		ChatSettings settings = _settingsMonitor.CurrentValue;
		Player player = Main.player[author];
		Color chatColor = player.ChatColor(); //TODO: chat colour reimplementation

		string formattedMessage;
		NetworkText netText;

		if (settings.EnableOverheadMessages)
		{
			// Unfortunately, we can't format things as nicely when using overhead text.
			// Terraria forces a '<name> text' format for clients, so all we can really do is format the text.
			//TODO: format the text
			formattedMessage = string.Format(settings.OverheadMessageFormat,
				message);
			netText = NetworkText.FromLiteral(formattedMessage);
			ChatHelper.BroadcastChatMessageAs((byte)author, netText, chatColor);

			return;
		}

		// TODO: expand on chat format
		formattedMessage = string.Format(settings.ChatMessageFormat,
			player.name,
			message);
		netText = NetworkText.FromLiteral(formattedMessage);

		// The broadcast is sent as the server, to avoid the weird default formatting
		ChatHelper.BroadcastChatMessageAs(byte.MaxValue,
			netText,
			chatColor);
	}

	private void HandleCommand(string message, int author)
	{
		//TODO: handle as a command
		_logger.LogInformation("{ChatHelper} {Author} {Value}",
			nameof(CommandReceived),
			author,
			message);
	}
}
