using System;
using System.Globalization;
using System.Windows.Data;

namespace Pizza.Utilities.Converter
{
	public class BooleanToEnabledConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool boolValue)
			{
				return !boolValue; // Инвертируем значение для выключения кнопки
			}

			return true; // По умолчанию кнопка включена
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
