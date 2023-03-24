using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TShockAPI.Models;

/// <summary>
/// TODO
/// </summary>
[PrimaryKey("Name")]
public class UserGroup
{
	public string Name { get; set; }
	public IList<UserAccount> Accounts { get; set; }
}
