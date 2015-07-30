using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using CodeGadgets.Open.Framework.MVVM;
using CodeGadgets.Open.MSDNAssist.Model;

namespace CodeGadgets.Open.MSDNAssist.ViewModel
{
	public class MonitoredFolderViewModel : ViewModelBase, IDataErrorInfo, CodeGadgets.Framework.ModelViewViewModel.ITextBoxController
	{
		public event CodeGadgets.Framework.ModelViewViewModel.SelectAllEventHandler SelectAll;
		public event CodeGadgets.Framework.ModelViewViewModel.FocusEventHandler Focus;
		public Action UpdateMonitoredFolders { get; set; }

		public MonitoredFolder Data
		{
			get { return this._Data; }
			set
			{
				this._Data = value;
				this.OnPropertyChanged(nameof(Data));
			}
		}
		public ICommand ShowFolderBrowserCommand
		{
			get { return (_ShowFolderBrowserCommand) ?? (_ShowFolderBrowserCommand = new RelayCommand(param => ShowFolderBrowserCommandExecute())); }
		}
		public ICommand SetTextBoxFocusCommand
		{
			get { return (_SetTextBoxFocusCommand) ?? (_SetTextBoxFocusCommand = new RelayCommand(param => FocusOnTextBox())); }
		}
		public ICommand DeleteCommand
		{
			get { return (_DeleteCommand) ?? (_DeleteCommand = new RelayCommand(param => Logic.Do.Options_DeleteMonitoredFolder(this))); }
		}
		public string FolderName
		{
			get { return Data.FolderName; }
			set
			{
				Data.FolderName = value;
				this.OnPropertyChanged(nameof(FolderName));
			}
		}
		public void FocusOnTextBox() => Focus?.Invoke(this);

		#region Validation Properties
		public string Error
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				if (!string.IsNullOrWhiteSpace(this["FolderName"]))
					sb.AppendLine(string.Format("Monitored Folder Error: {0}", this["FolderName"]));
				return sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
			}
		}
		public string this[string columnName]
		{
			get
			{
				string res = string.Empty;
				switch (columnName)
				{
					case "FolderName": if (!Logic.Do.CheckMonitoredFolder(FolderName)) res = "Folder value is invalid."; break;
				}
				return res;
			}
		}
		#endregion

		private void ShowFolderBrowserCommandExecute()
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if (Logic.Do.CheckMonitoredFolder(FolderName)) fbd.SelectedPath = FolderName;
			if (fbd.ShowDialog() != DialogResult.OK) return;
			if (Logic.Do.CheckMonitoredFolder(fbd.SelectedPath)) FolderName = fbd.SelectedPath;
		}
		private MonitoredFolder _Data;
		private ICommand _ShowFolderBrowserCommand;
		private ICommand _SetTextBoxFocusCommand;
		private ICommand _DeleteCommand;
	}

}