using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGadgets.Open.Framework.MVVM
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public string Name
		{
			get { return _Name; }
			set
			{
				if (_Name == value) return;
				_Name = value;
				OnPropertyChanged(nameof(Name));
			}
		}
		protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		private string _Name;
	}
}
