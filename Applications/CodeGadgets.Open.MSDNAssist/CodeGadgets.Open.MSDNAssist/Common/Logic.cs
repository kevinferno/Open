﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGadgets.Framework.Persistence;
using CodeGadgets.Open.MSDNAssist.Model;
using CodeGadgets.Open.MSDNAssist.ViewModel;
using CodeGadgets.Open.Framework.Threading;

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
			Initialize_UserData();
			Initiailze_Tools();
		}
		private void Initialize_CoreData()
		{
			Data.UserDataFilePath = SaveFileNameGenerator.FormatForLocalApplicationData(MSDNAssist.CompanyName, MSDNAssist.MainWindowTitle, System.Reflection.Assembly.GetEntryAssembly().Location, "userDataXml");
		}
		private void Initialize_UserData()
		{
			Data.UserDataSaveFile = new UserDataSaveFile();
			Data.UserDataSaveFile.FilePath = Data.UserDataFilePath;
			// Load User Data From File
			Data.UserData = Data.UserDataSaveFile.Load();
			// If UserData is null for whatever reason, just assign a new one
			if (Data.UserData == null) Data.UserData = new UserData();
		}
		private void Initiailze_Tools()
		{
			// AutoSave
			Data.AutoSaveProcessor = BackgroundMaintenanceProcessor.Create(() => Data.UserDataSaveFile.Save(Data.UserData), MSDNAssist.AUTOSAVE_INTERVAL_MS, MSDNAssist.AUTOSAVE_INITIALDELAY_MS); ;
			Data.AutoSaveProcessor.Start();
		}
		#endregion
		#region Termination
		public void Terminate()
		{
			SaveUserData();
		}
		internal void SaveUserData()
		{
			Data.UserDataSaveFile.Save(Data.UserData);
		}
		#endregion
		#region Validation
		public bool CheckMonitoredFolder(string src)
		{
			return !string.IsNullOrWhiteSpace(src) && Directory.Exists(src);
		}
		#endregion
		#region Options
		public void Options_Initialize(OptionsViewModel ovm)
		{
			// Monitored Folders
			ovm.MonitoredFolders.Clear();
			foreach (var folder in Core.Data.UserData.MonitoredFolders)
			{
				var vm = new FolderViewModel()
				{
					Data = folder,
					IsVisible_DeleteButton = true,
					UpdateMonitoredFolders = () => Options_Initialize(ovm)
				};
				ovm.MonitoredFolders.Add(vm);
			}
			// Destination Folder
			ovm.DestinationFolder = new FolderViewModel() { Data = new Folder() { FolderName = Core.Data.UserData.DestinationFolder } };
		}
		internal void Options_AddNewMonitoredFolder(OptionsViewModel ovm)
		{
			var vm = new FolderViewModel()
			{
				Data = new Folder(),
				UpdateMonitoredFolders = () => Options_Initialize(ovm)
			};
			ovm.MonitoredFolders.Add(vm);
		}
		internal void Options_UpdateMonitoredFolders(OptionsViewModel ovm)
		{
			// Removed Folders are ones that are in the Data but not found in the Options Data
			var removedFolders = Core.Data.UserData.MonitoredFolders.Where(src => !ovm.MonitoredFolders.Any(dat => dat.FolderName.Trim().ToLower() == src.FolderName.Trim().ToLower())).ToList();
			foreach (var rf in removedFolders) Core.Data.UserData.MonitoredFolders.Remove(rf);
			// Added Folders are ones that in the Options, but not in the User Data
			var addedFolders = ovm.MonitoredFolders.Where(src => !Core.Data.UserData.MonitoredFolders.Any(dat => dat.FolderName.Trim().ToLower() == src.FolderName.Trim().ToLower())).ToList();
			foreach (var af in addedFolders) Core.Data.UserData.MonitoredFolders.Add(af.Data);
		}
		internal void Options_UpdateData(OptionsViewModel ovm)
		{
			Core.Data.UserData.DestinationFolder = ovm.DestinationFolder?.Data?.FolderName;
		}
		internal void Options_DeleteMonitoredFolder(FolderViewModel vm)
		{
			var data = vm.Data;
			if (!Data.UserData.MonitoredFolders.Contains(data)) return;
			Data.UserData.MonitoredFolders.Remove(data);
			vm.UpdateMonitoredFolders?.Invoke();
		}
		#endregion
		#region Background Processes
		public void BackgroundProcesses_StartAll()
		{
		}
		public void BackgroundProcesses_StopAll()
		{

		}
		#endregion
	}
}
