using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DropOrganize.ViewModel
{
    public class CommandHelper : ICommand
    {
        private readonly Action _exec;
        private readonly Func<bool> _canExec;
        public CommandHelper(Action exec, Func<bool> canExec = null)
        {
            _exec = exec;
            _canExec = canExec;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object _)
        {
            if (_canExec is null) return true;
            return _canExec.Invoke();
        }

        public void Execute(object _)
        {
            _exec?.Invoke();
        }
    }

    public class CommandHelper<T> : ICommand
    {
        private readonly Action<T> _exec;
        private readonly Func<T, bool> _canExec;
        public CommandHelper(Action<T> exec, Func<T, bool> canExec = null)
        {
            _exec = exec;
            _canExec = canExec;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (_canExec is null) return true;
            try
            {
                T v = (T)parameter;
                return _canExec.Invoke(v);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
                return false;
            }
            
        }

        public void Execute(object parameter)
        {
            if(_exec is null) return;
            try
            {
                T v = (T)parameter;
                _exec.Invoke(v);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }
        }

    }
}
