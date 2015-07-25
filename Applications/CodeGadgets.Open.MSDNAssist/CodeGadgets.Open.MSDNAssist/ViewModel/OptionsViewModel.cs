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


		public OptionsViewModel(OptionsView view)
		{
			this.View = view;
			this.Name = MSDNAssist.OptionsWindowTitle;
		}


		private bool? _DialogResult;
		private OptionsView View;
	}
}
