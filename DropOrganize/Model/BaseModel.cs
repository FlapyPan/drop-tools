using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DropOrganize.Model
{
    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaiseAndSetIfChanged<T>(ref T a, T v, [CallerMemberName] string propName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(a, v))
            {
                a = v;
                if (propName != null)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
                }
            }

        }
    }
}
