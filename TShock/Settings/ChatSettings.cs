namespace TShock.Settings;

/// <summary>
/// Settings specific to  chat
/// </summary>
public sealed class ChatSettings
{
	/// <summary>
	/// Sets the chat message prefix that indicates that a message should be processed as a command.
	/// </summary>
	public char CommandPrefix { get; set; } = '/';

	/// <summary>
	/// Sets the chat message prefix that indicates that a message should be processed as a command,
	/// in suppressed mode.
	/// </summary>
	public char SuppressedCommandPrefix { get; set; } = '!';

	/// <summary>
	/// The message format for chat messages that appear in a player's chat log.
	/// Format parameters:
	/// - {0}: player name
	/// - {1}: chat message
	/// </summary>
	public string ChatMessageFormat { get; set; } = "{0}: {1}";

	/// <summary>
	/// Enable or disable overhead chat messages. This uses combat text to display the player's message.
	/// <para/>
	/// Default: false
	/// </summary>
	public bool EnableOverheadMessages { get; set; } = false;

	/// <summary>
	/// The message format for chat messages that appear over a player's head.
	/// Format parameters:
	/// - {0}: chat message
	/// </summary>
	public string OverheadMessageFormat { get; set; } = "{0}";
}
