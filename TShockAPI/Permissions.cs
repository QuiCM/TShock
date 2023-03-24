﻿/*
TShock, a server mod for Terraria
Copyright (C) 2011-2019 Pryaxis & TShock Contributors

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using TShock.Modules;

// Since the permission nodes have annotations that say what they are, we don't need XML comments.
#pragma warning disable 1591

namespace TShockAPI
{
	/// <summary>Contains the permission nodes used in TShock.</summary>
	public static class Permissions
	{
		#region tshock.account nodes
		[Description("User can register account in game.")]
		public const string canregister = "tshock.account.register";

		[Description("User can login in game.")]
		public const string canlogin = "tshock.account.login";

		[Description("User can logout in game.")]
		public const string canlogout = "tshock.account.logout";

		[Description("User can change password in game.")]
		public const string canchangepassword = "tshock.account.changepassword";
		#endregion

		#region tshock.admin nodes
		[Description("User can set build protection status.")]
		public const string antibuild = "tshock.admin.antibuild";

		[Description("Prevents you from being kicked.")]
		public static readonly string immunetokick = "tshock.admin.nokick";

		[Obsolete("Ban immunity is no longer available.")]
		[Description("Prevents you from being banned.")]
		public static readonly string immunetoban = "tshock.admin.noban";

		[Description("Specific log messages are sent to users with this permission.")]
		public const string logs = "tshock.admin.viewlogs";

		[Description("User can kick others.")]
		public const string kick = "tshock.admin.kick";

		[Description("User can ban others.")]
		public const string ban = "tshock.admin.ban";

		[Description("User can manage warps.")]
		public const string managewarp = "tshock.admin.warp";

		[Description("User can manage item bans.")]
		public const string manageitem = "tshock.admin.itemban";

		[Description("User can manage projectile bans.")]
		public const string manageprojectile = "tshock.admin.projectileban";

		[Description("User can manage tile bans.")]
		public const string managetile = "tshock.admin.tileban";

		[Description("User can manage groups.")]
		public const string managegroup = "tshock.admin.group";

		[Description("User can manage regions.")]
		public const string manageregion = "tshock.admin.region";

		[Description("User can mute and unmute users.")]
		public const string mute = "tshock.admin.mute";

		[Description("User can see the id of players with /who.")]
		public static readonly string seeids = "tshock.admin.seeplayerids";

		[Description("User can save all the players SSI (server side character) state.")]
		public const string savessc = "tshock.admin.savessi";

		[Description("User can upload their joined character data as SSC data.")]
		public const string uploaddata = "tshock.ssc.upload";

		[Description("User can upload other players join data to the SSC database.")]
		public static readonly string uploadothersdata = "tshock.ssc.upload.others";

		[Description("User can elevate other users' groups temporarily.")]
		public const string settempgroup = "tshock.admin.tempgroup";

		[Description("User can broadcast messages.")]
		public const string broadcast = "tshock.admin.broadcast";

		[Description("User can get other users' info.")]
		public const string userinfo = "tshock.admin.userinfo";
		#endregion

		#region tshock.buff nodes
		[Description("User can buff self.")]
		public const string buff = "tshock.buff.self";

		[Description("User can buff other players.")]
		public const string buffplayer = "tshock.buff.others";
		#endregion

		#region tshock.cfg nodes
		[Description("User is notified when an update is available, user can turn off / restart the server.")]
		public const string maintenance = "tshock.cfg.maintenance";

		[Description("User can modify the whitelist.")]
		public const string whitelist = "tshock.cfg.whitelist";

		[Description("User can edit the server password.")]
		public const string cfgpassword = "tshock.cfg.password";

		[Description("User can reload the configurations file.")]
		public const string cfgreload = "tshock.cfg.reload";

		[Description("User can create reference files of Terraria IDs and the permission matrix in the server folder.")]
		public const string createdumps = "tshock.cfg.createdumps";
		#endregion

		#region tshock.ignore nodes
		[Description("Prevents you from being reverted by kill tile abuse detection.")]
		public static readonly string ignorekilltiledetection = "tshock.ignore.removetile";

		[Description("Prevents you from being reverted by place tile abuse detection.")]
		public static readonly string ignoreplacetiledetection = "tshock.ignore.placetile";

		[Description("Prevents you from being disabled by liquid set abuse detection.")]
		public static readonly string ignoreliquidsetdetection = "tshock.ignore.liquid";

		[Description("Prevents you from being disabled by projectile abuse detection.")]
		public static readonly string ignoreprojectiledetection = "tshock.ignore.projectile";

		[Description("Prevents you from being disabled by paint abuse detection.")]
		public static readonly string ignorepaintdetection = "tshock.ignore.paint";

		[Description("Prevents you from being disabled by stack hack detection.")]
		public static readonly string ignorestackhackdetection = "tshock.ignore.itemstack";

		[Description("Prevents your actions from being ignored if damage is too high.")]
		public static readonly string ignoredamagecap = "tshock.ignore.damage";

		[Description("Bypass server side character checks.")]
		public static readonly string bypassssc = "tshock.ignore.ssc";

		[Description("Allow unrestricted SendTileSquare usage, for client side world editing.")]
		public static readonly string allowclientsideworldedit = "tshock.ignore.sendtilesquare";

		[Description("Allow dropping banned items without the item being eaten.")]
		public static readonly string allowdroppingbanneditems = "tshock.ignore.dropbanneditem";

		[Description("Prevents you from being disabled by abnormal HP.")]
		public static readonly string ignorehp = "tshock.ignore.hp";

		[Description("Prevents you from being disabled by abnormal MP.")]
		public static readonly string ignoremp = "tshock.ignore.mp";
		#endregion

		#region tshock.item nodes
		[Description("User can give items.")]
		public const string give = "tshock.item.give";

		[Description("User can spawn items.")]
		public const string item = "tshock.item.spawn";

		[Description("Allows you to use banned items.")]
		public static readonly string usebanneditem = "tshock.item.usebanned";
		#endregion

		#region tshock.npc nodes
		[Description("User can edit the max spawns.")]
		public const string maxspawns = "tshock.npc.maxspawns";

		[Description("User can edit the spawnrate.")]
		public const string spawnrate = "tshock.npc.spawnrate";

		[Description("User can start an invasion. Warning: high network use. Easy to abuse.")]
		public const string invade = "tshock.npc.invade";

		[Description("User can hurt town NPCs.")]
		public static readonly string hurttownnpc = "tshock.npc.hurttown";

		[Description("User can spawn bosses.")]
		public static readonly string spawnboss = "tshock.npc.spawnboss";

		[Description("User can spawn pets. Warning: high network use. Easy to abuse.")]
		public static readonly string spawnpets = "tshock.npc.spawnpets";

		[Description("User can rename NPCs.")]
		public const string renamenpc = "tshock.npc.rename";

		[Description("User can spawn npcs.")]
		public const string spawnmob = "tshock.npc.spawnmob";

		[Description("User can kill all enemy npcs.")]
		public const string butcher = "tshock.npc.butcher";

		[Description("User can summon bosses using items.")]
		public static readonly string summonboss = "tshock.npc.summonboss";

		[Description("User can start invasions (Goblin/Snow Legion) using items.")]
		public static readonly string startinvasion = "tshock.npc.startinvasion";

		[Description("User can start the dd2 event.")]
		public static readonly string startdd2 = "tshock.npc.startdd2";

		[Description("User can clear the list of users who have completed an angler quest that day.")]
		public const string clearangler = "tshock.npc.clearanglerquests";

		[Description("Meant for super admins only.")]
		public const string user = "tshock.superadmin.user";

		[Description("Allows a user to elevate to superadmin for 10 minutes.")]
		public const string su = "tshock.su";
		#endregion

		#region tshock.tp nodes
		[Description("User can teleport *everyone* to them.")]
		public static readonly string tpallothers = "tshock.tp.allothers";

		[Description("User can teleport to others.")]
		public const string tp = "tshock.tp.self";

		[Description("User can teleport other people.")]
		public const string tpothers = "tshock.tp.others";

		[Description("User can teleport to tile positions.")]
		public const string tppos = "tshock.tp.pos";

		[Description("User can get the position of players.")]
		public const string getpos = "tshock.tp.getpos";

		[Description("User can teleport to an NPC.")]
		public const string tpnpc = "tshock.tp.npc";

		[Description("Users can stop people from teleporting.")]
		public const string tpallow = "tshock.tp.block";

		[Description("Users can override teleport blocks.")]
		public static readonly string tpoverride = "tshock.tp.override";

		[Description("Users can teleport to people without showing a notice")]
		public static readonly string tpsilent = "tshock.tp.silent";

		[Description("User can use /home.")]
		public const string home = "tshock.tp.home";

		[Description("User can use /spawn.")]
		public const string spawn = "tshock.tp.spawn";

		[Description("User can use the Rod of Discord.")]
		public static readonly string rod = "tshock.tp.rod";

		[Description("User can use wormhole potions.")]
		public static readonly string wormhole = "tshock.tp.wormhole";

		[Description("User can use pylons to teleport")]
		public static readonly string pylon = "tshock.tp.pylon";

		[Description("User can use Teleportation Potions.")]
		public static readonly string tppotion = "tshock.tp.tppotion";

		[Description("User can use the Magic Conch.")]
		public static readonly string magicconch = "tshock.tp.magicconch";

		[Description("User can use the Demon Conch.")]
		public static readonly string demonconch = "tshock.tp.demonconch";
		#endregion

		#region tshock.world nodes
		[Description("User can use the 'worldevent' command")]
		public const string manageevents = "tshock.world.events";

		[Description("User can use the 'bloodmoon' subcommand of the 'worldevent' command")]
		public const string managebloodmoonevent = "tshock.world.events.bloodmoon";

		[Description("User can use the 'fullmoon' subcommand of the 'worldevent' command")]
		public const string managefullmoonevent = "tshock.world.events.fullmoon";

		[Description("User can use the 'invasion' subcommand of the 'worldevent' command")]
		public const string manageinvasionevent = "tshock.world.events.invasion";

		[Description("User can use the 'meteor' subcommand of the 'worldevent' command")]
		public const string managemeteorevent = "tshock.world.events.meteor";

		[Description("User can use the 'eclipse' subcommand of the 'worldevent' command")]
		public const string manageeclipseevent = "tshock.world.events.eclipse";

		[Description("User can use the 'sandstorm' subcommand of the 'worldevent' command")]
		public const string managesandstormevent = "tshock.world.events.sandstorm";

		[Description("User can use the 'rain' subcommand of the 'worldevent' command")]
		public const string managerainevent = "tshock.world.events.rain";

		[Description("User can use the 'lanternsnight' subcommand of the 'worldevent' command")]
		public const string managelanternsnightevent = "tshock.world.events.lanternsnight";

		[Description("User can change expert state.")]
		public const string toggleexpert = "tshock.world.toggleexpert";

		[Description("Allows you to edit the spawn.")]
		public const string editspawn = "tshock.world.editspawn";

		[Description("Allows you to edit regions.")]
		public static readonly string editregion = "tshock.world.editregion";

		[Description("User can force a blood moon.")]
		public const string bloodmoon = "tshock.world.time.bloodmoon";

		[Description("User can set the time.")]
		public const string time = "tshock.world.time.set";

		[Description("Player can use the Enchanted Sundial item.")]
		public static readonly string usesundial = "tshock.world.time.usesundial";

		[Description("User can grow plants.")]
		public const string grow = "tshock.world.grow";

		[Description("User can grow evil biome plants.")]
		public static readonly string growevil = "tshock.world.growevil";

		[Description("User can change hardmode state.")]
		public const string hardmode = "tshock.world.hardmode";

		[Description("User can change the homes of NPCs.")]
		public static readonly string movenpc = "tshock.world.movenpc";

		[Description("User can convert hallow into corruption and vice-versa.")]
		public static readonly string converthardmode = "tshock.world.converthardmode";

		[Description("User can force the server to Halloween mode.")]
		public const string halloween = "tshock.world.sethalloween";

		[Description("User can force the server to Christmas mode.")]
		public const string xmas = "tshock.world.setxmas";

		[Description("User can save the world.")]
		public const string worldsave = "tshock.world.save";

		[Description("User can settle liquids.")]
		public const string worldsettle = "tshock.world.settleliquids";

		[Description("User can get the world info.")]
		public const string worldinfo = "tshock.world.info";

		[Description("User can set the world spawn.")]
		public const string worldspawn = "tshock.world.setspawn";

		[Description("User can set the dungeon's location.")]
		public const string dungeonposition = "tshock.world.setdungeon";

		[Description("User can drop a meteor.")]
		public const string dropmeteor = "tshock.world.time.dropmeteor";

		[Description("User can force an eclipse.")]
		public const string eclipse = "tshock.world.time.eclipse";

		[Description("User can force a full moon.")]
		public const string fullmoon = "tshock.world.time.fullmoon";

		[Description("User can modify the world.")]
		public static readonly string canbuild = "tshock.world.modify";

		[Description("User can paint tiles.")]
		public static readonly string canpaint = "tshock.world.paint";

		[Description("User can turn on or off sandstorms.")]
		public const string sandstorm = "tshock.world.sandstorm";

		[Description("User can turn on or off the rain.")]
		public const string rain = "tshock.world.rain";

		[Description("User can modify the wind.")]
		public const string wind = "tshock.world.wind";

		[Description("Player can toggle party event.")]
		public static readonly string toggleparty = "tshock.world.toggleparty";
		#endregion

		#region tshock.journey nodes
		[Description("User can use Creative UI freeze time.")]
		public static readonly string journey_timefreeze = "tshock.journey.time.freeze";

		[Description("User can use Creative UI to set world time.")]
		public static readonly string journey_timeset = "tshock.journey.time.set";

		[Description("User can use Creative UI to set world time speed.")]
		public static readonly string journey_timespeed = "tshock.journey.time.setspeed";

		[Description("User can use Creative UI to toggle character godmode.")]
		public static readonly string journey_godmode = "tshock.journey.godmode";

		[Description("User can use Creative UI to set world wind strength/seed.")]
		public static readonly string journey_windstrength = "tshock.journey.wind.strength";

		[Description("User can use Creative UI to stop the world wind strength from changing.")]
		public static readonly string journey_windfreeze = "tshock.journey.wind.freeze";

		[Description("User can use Creative UI to set world rain strength/seed.")]
		public static readonly string journey_rainstrength = "tshock.journey.rain.strength";

		[Description("User can use Creative UI to stop the world rain strength from changing.")]
		public static readonly string journey_rainfreeze = "tshock.journey.rain.freeze";

		[Description("User can use Creative UI to toggle increased placement range.")]
		public static readonly string journey_placementrange = "tshock.journey.placementrange";

		[Description("User can use Creative UI to set world difficulty/mode.")]
		public static readonly string journey_setdifficulty = "tshock.journey.setdifficulty";

		[Description("User can use Creative UI to stop the biome spread of the world.")]
		public static readonly string journey_biomespreadfreeze = "tshock.journey.biomespreadfreeze";

		[Description("User can use Creative UI to set the NPC spawn rate of the world.")]
		public static readonly string journey_setspawnrate = "tshock.journey.setspawnrate";

		[Description("User can contribute research by sacrificing items")]
		public static readonly string journey_contributeresearch = "tshock.journey.research";
		#endregion

		#region Non-grouped
		[Description("User can clear items or projectiles.")]
		public const string clear = "tshock.clear";

		[Description("User can kill others.")]
		public const string kill = "tshock.kill";

		[Description("Player can respawn themselves.")]
		public const string respawn = "tshock.respawn";

		[Description("Player can respawn others.")]
		public static readonly string respawnother = "tshock.respawn.other";

		[Description("Allows you to bypass the max slots for up to 5 slots above your max.")]
		public static readonly string reservedslot = "tshock.reservedslot";

		[Description("User can use warps.")]
		public const string warp = "tshock.warp";

		[Description("User can slap others.")]
		public const string slap = "tshock.slap";

		[Description("User can whisper to others.")]
		public const string whisper = "tshock.whisper";

		[Description("User can annoy others.")]
		public const string annoy = "tshock.annoy";

		[Description("User can heal players.")]
		public const string heal = "tshock.heal";

		[Description("User can use party chat in game.")]
		public const string canpartychat = "tshock.partychat";

		[Description("User can talk in third person.")]
		public const string cantalkinthird = "tshock.thirdperson";

		[Description("User can get the server info.")]
		public const string serverinfo = "tshock.info";

		[Description("Player recovers health as damage is taken.  Can be one shotted.")]
		public const string godmode = "tshock.godmode";

		[Description("User can godmode other players.")]
		public static readonly string godmodeother = "tshock.godmode.other";

		[Description("Player can chat.")]
		public static readonly string canchat = "tshock.canchat";

		[Description("Player can use banned projectiles.")]
		public static readonly string canusebannedprojectiles = "tshock.projectiles.usebanned";

		[Description("Player can place banned tiles.")]
		public static readonly string canusebannedtiles = "tshock.tiles.usebanned";

		[Description("Player can check if a username is registered and see its last login time.")]
		public const string checkaccountinfo = "tshock.accountinfo.check";

		[Description("Player can see advanced information about any user account.")]
		public static readonly string advaccountinfo = "tshock.accountinfo.details";

		[Description("Player can resync themselves with server state.")]
		public const string synclocalarea = "tshock.synclocalarea";

		[Description("Player can send emotes.")]
		public static readonly string sendemoji = "tshock.sendemoji";
		#endregion

		/// <summary>
		/// Dumps the descriptions of each permission to a file in Markdown format.
		/// </summary>
		public static void DumpDescriptions(ICommandService commandService)
		{
			var sb = new StringBuilder();
			foreach (var field in typeof(Permissions).GetFields().OrderBy(f => f.Name))
			{
				var name = (string)field.GetValue(null);

				var descattr =
					field.GetCustomAttributes(false).FirstOrDefault(o => o is DescriptionAttribute) as DescriptionAttribute;
				var desc = descattr != null && !string.IsNullOrWhiteSpace(descattr.Description) ? descattr.Description : "None";

				var commands = commandService.GetCommands(permission: name);
				//foreach (var c in commands)
				//{
				//	for (var i = 0; i < c.Names.Count; i++)
				//	{
				//		c.Names[i] = "/" + c.Names[i];
				//	}
				//}
				var strs =
					commands.Select(
						c =>
						c.Names.First() + (c.Names.Count() > 1 ? "({0})".SFormat(string.Join(" ", c.Names.ToArray(), 1, c.Names.Count() - 1)) : ""));

				sb.AppendLine("{0}".SFormat(name));
				sb.AppendLine("Description: {0}  ".SFormat(desc));
				sb.AppendLine("Commands: {0}  ".SFormat(strs.Count() > 0 ? string.Join(" ", strs) : "None"));
				sb.AppendLine();
			}

			File.WriteAllText("PermissionsDescriptions.txt", sb.ToString());
		}
	}
}
