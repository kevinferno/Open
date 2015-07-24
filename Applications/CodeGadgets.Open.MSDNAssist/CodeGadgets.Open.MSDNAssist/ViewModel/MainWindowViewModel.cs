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

		public MainWindowViewModel(MainWindowView view)
		{
			this.Name = MSDNAssist.MainWindowTitle;
			this.MW = view;
		}

		private void ShowOptionsCommandExecute()
		{
			var ov = new OptionsView();
			var ovm = new OptionsViewModel(ov);
			ov.DataContext = ovm;
			ov.ShowDialog();
		}



		private MainWindowView MW;
		private bool? _DialogResult;
		private ICommand _ShowOptionsCommand;
	}
}
