using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CodeGadgets.Open.Framework.MVVM;
using CodeGadgets.Open.MSDNAssist.View;

namespace CodeGadgets.Open.MSDNAssist.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
		public ICommand ShowOptionsCommand
		{
			get { return (_ShowOptionsCommand) ?? (_ShowOptionsCommand = new RelayCommand(param => ShowOptionsCommandExecute())); }
		}
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

		public MainWindowViewModel(MainWindowView view)
		{
			this.WindowTitle = MSDNAssist.MainWindowTitle;
			this.MW = view;
		}

		private void ShowOptionsCommandExecute()
		{
			var ov = new OptionsView();
			var ovm = new OptionsViewModel(ov);
			Logic.Do.Options_InitializeMonitoredFolders(ovm);
			ov.DataContext = ovm;
			ov.Owner = MW;
			var res = ov.ShowDialog();
			if (res.HasValue && res.Value)
			{
				Logic.Do.Options_UpdateMonitoredFolders(ovm);
				Logic.Do.SaveUserData();
			}
		}

		private MainWindowView MW;
		private string _WindowTitle;
		private bool? _DialogResult;
		private ICommand _ShowOptionsCommand;
	}
}
