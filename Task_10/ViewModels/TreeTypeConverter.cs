using System;
using System.Globalization;
using System.Windows.Data;
using Task_10.Models;

namespace Task_10.ViewModels
{
    internal class TreeTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TreeType type)
            {
                switch (type)
                {
                    case TreeType.Birch:
                        return "Береза";
                    case TreeType.Apple:
                        return "Яблоня";
                    case TreeType.Cherry:
                        return "Вишня";
                    default:
                        return value != null ? value.ToString() : string.Empty;
                }
            }

            return value != null ? value.ToString() : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                switch (str)
                {
                    case "Береза":
                        return TreeType.Birch;
                    case "Яблоня":
                        return TreeType.Apple;
                    case "Вишня":
                        return TreeType.Cherry;
                    default:
                        throw new ArgumentException(string.Format("Неизвестный тип дерева: {0}", str));
                }
            }

            throw new ArgumentException("Некорректное значение для конвертации");
        }
    }

}
