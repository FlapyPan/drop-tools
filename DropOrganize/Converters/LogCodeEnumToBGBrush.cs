using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DropOrganize.Converters
{
    public class LogCodeEnumToBGBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var logCode = (Model.OperationLog.CodeEnum)value;
            switch (logCode)
            {
                case Model.OperationLog.CodeEnum.Success: return new SolidColorBrush(Color.FromRgb(80, 245, 168));
                case Model.OperationLog.CodeEnum.Error: return new SolidColorBrush(Color.FromRgb(255, 82, 43));
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
