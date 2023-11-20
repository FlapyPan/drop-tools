using System;
using System.Collections.Generic;
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
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        private System.Threading.Mutex mutex = null;

        public SettingWindow()
        {
            InitializeComponent();
        }

        private void CloseWin(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_loaded(object sender, RoutedEventArgs args)
        {
            bool newMutex = false;
            try
            {
                mutex = new System.Threading.Mutex(true, "SettingWindow", out newMutex);
            }
            catch { }
            if (!newMutex)
            {
                Close();
            }
        }
    }
}
