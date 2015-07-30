using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGadgets.Framework.Utility;
using CodeGadgets.Open.Framework.MVVM;
using CodeGadgets.Open.MSDNAssist.Model;

namespace CodeGadgets.Open.MSDNAssist
{
	public sealed class Core
	{
		#region Singleton Implementation
		static readonly Core _SingletonInstance = new Core();
		static Core() { }
		private Core() { }
		#endregion
		public static Core Data
		{
			get { return _SingletonInstance; }
		}
		public static Logic Logic { get { return Logic.Do; } }

		public string UserDataFilePath { get; set; }
		public UserData UserData { get; set; }
		public UserDataSaveFile UserDataSaveFile { get; set; }
		public BackgroundMaintenanceProcessor AutoSaveProcessor { get; set; }
	}
}
