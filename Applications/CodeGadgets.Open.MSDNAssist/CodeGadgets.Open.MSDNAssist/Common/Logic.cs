﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGadgets.Framework.Persistence;
using CodeGadgets.Framework.Xml;
using CodeGadgets.Open.MSDNAssist.Model;

namespace CodeGadgets.Open.MSDNAssist
{
	public sealed class Logic
	{
		#region Singleton Implementation
		static readonly Logic _SingletonInstance = new Logic();
		static Logic() { }
		private Logic() { }
		#endregion
		public static Logic Do
		{
			get { return _SingletonInstance; }
		}
		private Core Data { get { return Core.Data; } }

		#region Initialization

		public void Initialize()
		{
			Initialize_CoreData();
			Initiailze_Tools();
			Initialize_UserData();
		}


		private void Initialize_CoreData()
		{
			Data.UserDataFilePath = SaveFileNameGenerator.FormatForLocalApplicationData(MSDNAssist.CompanyName, MSDNAssist.MainWindowTitle, System.Reflection.Assembly.GetEntryAssembly().Location, "userDataXml");
		}
		private void Initiailze_Tools()
		{
			Data.UserDataSaveFile = new UserDataSaveFile();
			Data.UserDataSaveFile.FilePath = Data.UserDataFilePath;
		}

		private void Initialize_UserData()
		{
			// Load User Data From File
			Data.UserData = Data.UserDataSaveFile.Load();
			// If UserData is null for whatever reason, just assign a new one
			if (Data.UserData == null) Data.UserData = new UserData();
		}
		#endregion
		#region Termination
		public	 void Terminate()
		{
			SaveUserData();
		}

		private void SaveUserData()
		{
			Data.UserDataSaveFile.Save(Data.UserData);
		}
		#endregion
	}
}