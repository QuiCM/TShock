
using Captain.Commanding;
using Microsoft.Extensions.Logging;

namespace TShock.Commands;

public abstract class TShockCommandService : CommandService<TShockUser>
{
	public TShockCommandService(TShockUser invoker, ILogger<TShockCommandService> logger) : base(invoker, logger) { }
}
