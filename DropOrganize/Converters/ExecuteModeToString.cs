using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using DropOrganize.ViewModel;

namespace DropOrganize.Converters
{
    public class ExecuteModeToString : IValueConverter
    {
        //当值从绑定源传播给绑定目标时，调用方法Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var modeByte = (byte)value;
            switch (modeByte)
            {
                case MainViewModel.Organize:
                return "分类";
                case MainViewModel.Rename:
                return "重命名";
                case MainViewModel.Zip:
                return "压缩";
                case MainViewModel.Unzip:
                return "解压";
                default:
                return "";
            }
        }
        //当值从绑定目标传播给绑定源时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var modeByte = (byte)value;
            return modeByte % MainViewModel.ModeTotalCount;

        }
    }
}
