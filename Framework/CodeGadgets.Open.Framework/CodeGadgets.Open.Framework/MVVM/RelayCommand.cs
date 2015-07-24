using System;
using System.Windows.Input;

namespace CodeGadgets.Open.Framework.MVVM
{
	public class RelayCommand : ICommand
	{
		private readonly Action<object> _Execute;
		private readonly Predicate<object> _CanExecute;

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public RelayCommand(Action<object> execute) : this(execute, null) { }
		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
		{
			if (execute == null) throw new ArgumentNullException(nameof(execute));
			this._Execute = execute;
			this._CanExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _CanExecute == null ? true : _CanExecute(parameter);
		}
		public void Execute(object parameter)
		{
			this._Execute(parameter);
		}
	}
}
