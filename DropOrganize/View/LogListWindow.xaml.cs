using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DropOrganize.View
{
    /// <summary>
    /// ErrorListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LogListWindow : Window
    {
        private ObservableCollection<Model.OperationLog> _logList;
        public ObservableCollection<Model.OperationLog> LogList
        {
            get => _logList;
            set => _logList = value;
        }

        public LogListWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
