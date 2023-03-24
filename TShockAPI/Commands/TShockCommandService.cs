
using Captain.Commanding;

namespace TShock.Commands;

public class TShockCommandService : CommandService
{
	public TShockCommandService(TShockUser receiver) : base(receiver) { }
}
