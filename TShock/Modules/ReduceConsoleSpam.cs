/*
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

namespace TShock.Modules;

/// <summary>
/// A module to reduce console spam coming from terraria world load/save etc
/// </summary>
public class ReduceConsoleSpam : Module
{
	/// <inheritdoc/>
	public ReduceConsoleSpam() =>
		OTAPI.Hooks.Main.StatusTextChange += OnMainStatusTextChange;

	/// <inheritdoc/>
	public override void Dispose() =>
		OTAPI.Hooks.Main.StatusTextChange -= OnMainStatusTextChange;

	/// <summary>
	/// Holds the last status text value, to determine if there is a suitable change to report.
	/// </summary>
	private string? _lastStatusText;

	/// <summary>
	/// Aims to reduce the amount of console spam by filtering out load/save progress
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e">OTAPI event</param>
	private void OnMainStatusTextChange(object? sender, OTAPI.Hooks.Main.StatusTextChangeArgs e)
	{
		void WriteIfChange(string text)
		{
			if (_lastStatusText != text)
			{
				Console.WriteLine(text); // write it manually instead of terraria which causes double writes
				_lastStatusText = text;
			}
		}
		bool Replace(string text)
		{
			if (!e.Value.StartsWith(text))
			{
				return false;
			}

			string segment = e.Value[..text.Length];
			WriteIfChange(segment);
			e.Value = "";
			return true;
		}

		if (Replace("Resetting game objects")
			|| Replace("Settling liquids")
			|| Replace("Loading world data")
			|| Replace("Saving world data")
			|| Replace("Validating world save"))
			return;

		// try parsing % - [text] - %
		const string findMaster = "% - ";
		const string findSub = " - ";
		int master = e.Value.IndexOf(findMaster);
		if (master > -1)
		{
			int sub = e.Value.LastIndexOf(findSub);
			if (sub > master)
			{
				string mprogress = e.Value.Substring(0, master + 1/*%*/);
				string sprogress = e.Value.Substring(sub + findSub.Length);
				if (mprogress.EndsWith("%") && sprogress.EndsWith("%"))
				{
					string text = e.Value.Substring(master + findMaster.Length, sub - master - findMaster.Length).Trim();

					if (text.Length > 0 && !(
						    // relogic has made a mess of this
						    (
							    _lastStatusText != "Validating world save"
							    || _lastStatusText != "Saving world data"
						    )
						    && text == "Finalizing world"
					    ))
					{
						WriteIfChange(text);
					}

					e.Value = "";
				}
			}
		}
	}

}

