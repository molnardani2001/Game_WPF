using System;
using System.Windows.Input;

public class DelegateCommand : ICommand
{
	private readonly Action<Object> _execute;
	private readonly Func<Object, Boolean> _canExecute;

	public event EventHandler CanExecuteChanged
	{
		add { CommandManager.RequerySuggested += value; }
		remove { CommandManager.RequerySuggested -= value; }
	}

	public DelegateCommand(Action<Object> execute, Func<Object,Boolean> canExecute = null)
	{
		if (execute == null)
		{
			throw new ArgumentNullException("execute");
		}

		_execute = execute;
		_canExecute = canExecute;
	}

	public bool CanExecute(object parameter)
	{
		return _canExecute == null ? true : _canExecute(parameter);
	}

	public void Execute(object parameter)
	{
		if (!CanExecute(parameter))
		{
			throw new InvalidOperationException("Command execution is disabled");
		}
		_execute(parameter);
	}
}
