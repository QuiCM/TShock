using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TShockAPI.Models;

/// <summary>
/// TODO
/// </summary>
[PrimaryKey("Name")]
public class UserAccount
{
	/// <summary>
	/// TODO
	/// </summary>
	[MaxLength(32)] //TODO: set to max character name length
	public string Name { get; set; }
	/// <summary>
	/// TODO. This is a hash, not the real thing
	/// </summary>
	public string Password { get; set; }

	/// <summary>
	/// TODO
	/// </summary>
	public UserGroup Group { get; set; }
}
