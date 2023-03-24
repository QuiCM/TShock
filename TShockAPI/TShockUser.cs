
using System;
using Captain.Commanding;
using TShock.Models;

namespace TShock;

public class TShockUser : IReceiver
{
	public UserAccount UserAccount { get; set; }
	public UserGroup UserGroup { get; set; }
	public string Name { get; set; }

	public TShockUser(string name)
	{
		Name = name;
	}


	public void SendSuccess(string message, params object[] args)
	{
		throw new NotImplementedException();
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
