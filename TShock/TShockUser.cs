
using System;
using Captain.Commanding;
using Terraria;
using TShock.Models;

namespace TShock;

public class TShockUser : IInvoker
{
	public UserAccount UserAccount { get; set; }
	public UserGroup UserGroup { get; set; }
	public Player Player { get; }

	public string Name => Player.name;

	public TShockUser(Player player)
	{
		Player = player;
	}

	public void SendSuccess(string message, params object[] args)
	{

	}

	public void SendWarning(string message, params object[] args)
	{
		throw new NotImplementedException();
	}

	public void SendError(string message, Exception? ex = null, params object[] args)
	{
		throw new NotImplementedException();
	}
}
