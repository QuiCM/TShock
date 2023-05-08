using System;
using System.Collections.Generic;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;

namespace TShock.Models;

public class CharacterData
{
	public UserAccount UserAccount { get; set; }

	public int Health { get; set; }
	public int MaxHealth { get; set; }
	public int Mana { get; set; }
	public int MaxMana { get; set; }
	public int AnglerQuestsCompleted { get; set; }
	public List<DeathRecord> DeathRecords { get; set; }

	public Point Spawn { get; set; }

	public int SkinVariant { get; set; }
	public int HairType { get; set; }
	public int HairDye { get; set; }

	public Color HairColor { get; set; }
	public Color EyeColor { get; set; }
	public Color SkinColor { get; set; }
	public Color ShirtColor { get; set; }
	public Color UnderShirtColor { get; set; }
	public Color PantsColor { get; set; }
	public Color ShoeColor { get; set; }

	public bool UnlockedExtraAccessory { get; set; }
	public bool HideAccessories { get; set; }
	public bool UnlockedBiomeTorches { get; set; }
	public bool UsingBiomeTorches { get; set; }
	public bool HappyFunTorchTime { get; set; }

	public DateTime Created { get; set; }
	public long PlayTime { get; set; }
}
