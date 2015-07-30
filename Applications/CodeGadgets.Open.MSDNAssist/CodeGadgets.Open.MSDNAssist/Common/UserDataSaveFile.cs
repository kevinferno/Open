using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGadgets.Framework.Persistence;
using CodeGadgets.Open.MSDNAssist.Model;

namespace CodeGadgets.Open.MSDNAssist
{
	public class UserDataSaveFile : XmlSerializedSaveFileBase<UserData>
	{
		public override IEnumerable<Type> GetKnownTypes()
		{
			return new List<Type>
			{
				typeof(MonitoredFolder)
			};
		}

		protected override bool SaveExecute(UserData data)
		{
			try
			{
				// Generate Save data
				// Read existing data file
				// Compare. Only save if different
				if (File.Exists(Core.Data.UserDataFilePath))
				{
					// Generate new Save Text
					string newData = this.ToXml(Core.Data.UserData);
					// Get Existing Save Text
					string existingData = File.ReadAllText(Core.Data.UserDataFilePath);
					// Compare
					if (newData.Trim() != existingData.Trim()) // If the files are no the same
					{
						// Update Metrics
						_SaveCount++;
						// Move it into archine
						SaveFileArchive.MoveFileIntoArchive(Core.Data.UserDataFilePath);
						// Write the text
						File.WriteAllText(Core.Data.UserDataFilePath, newData);
					}
					else _SkipCount++;
				}
				// No user Data File
				else
				{
					_SaveCount++;
					return base.SaveExecute(data);
				}
				DisplaySaveStats();


				return true;
			}
			catch { return false; }
		}

		#region Debug Metrics
		private int _SaveCount;
		private int _SkipCount;
		[Conditional("DEBUG")]
		private void DisplaySaveStats()
		{
			Debug.WriteLine($@"[{DateTime.Now:hh:mm:ss}] UserData.Save-Save Count={_SaveCount}, Skip Count={_SkipCount}");
		}
		#endregion
	}
}
