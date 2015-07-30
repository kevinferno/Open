using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CodeGadgets.Open.Framework.MVVM;
using CodeGadgets.Open.MSDNAssist.Model;
using CodeGadgets.Open.MSDNAssist.View;

namespace CodeGadgets.Open.MSDNAssist.ViewModel
{
	public class OptionsViewModel : ViewModelBase
	{
		public bool? DialogResult
		{
			get { return _DialogResult; }
			set
			{
				if (_DialogResult == value) return;
				_DialogResult = value;
				OnPropertyChanged(nameof(DialogResult));
			}
		}
		public ObservableCollection<MonitoredFolderViewModel> MonitoredFolders
		{
			get { return (this._MonitoredFolders) ?? (this._MonitoredFolders = new ObservableCollection<MonitoredFolderViewModel>()); }
		}
		public MonitoredFolderViewModel SelectedMonitoredFolder
		{
			get { return this._SelectedMonitoredFolder; }
			set
			{
				if (this._SelectedMonitoredFolder == value) return;
				this._SelectedMonitoredFolder = value;
				this.OnPropertyChanged(nameof(SelectedMonitoredFolder));
			}
		}
		public string WindowTitle
		{
			get { return this._WindowTitle; }
			set
			{
				if (this._WindowTitle == value) return;
				this._WindowTitle = value;
				this.OnPropertyChanged(nameof(WindowTitle));
			}
		}
		public ICommand AddMonitoredFolderCommand
		{
			get
			{
				return (_AddMonitoredFolderCommand) ?? (_AddMonitoredFolderCommand = new RelayCommand(param => Logic.Do.Options_AddNewMonitoredFolder(this)));
			}
		}
		public ICommand OkCommand
		{
			get
			{
				return (_OkCommand) ?? (_OkCommand =
					new RelayCommand(
					param => DialogResult = true,
					param => MonitoredFolders.Count > 0 && MonitoredFolders.All(mf => Logic.Do.CheckMonitoredFolder(mf.Data.FolderName))));
			}
		}
		public ICommand CancelCommand
		{
			get { return (_CancelCommand) ?? (_CancelCommand = new RelayCommand(param => this.DialogResult = false)); }
		}

		public OptionsViewModel(OptionsView view)
		{
			this.View = view;
			this.WindowTitle = MSDNAssist.OptionsWindowTitle;
		}

		private ObservableCollection<MonitoredFolderViewModel> _MonitoredFolders;
		private MonitoredFolderViewModel _SelectedMonitoredFolder;
		private bool? _DialogResult;
		private ICommand _CancelCommand;
		private ICommand _OkCommand;
		private OptionsView View;
		private string _WindowTitle;
		private ICommand _AddMonitoredFolderCommand;
	}
}
