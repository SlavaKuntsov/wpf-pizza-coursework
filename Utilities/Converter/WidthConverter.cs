using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Pizza.Utilities.Converter
{
	public class WidthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double containerWidth)
			{
				Console.WriteLine(containerWidth);

				double desiredWidth = 0;

				if (containerWidth >= 1150)
				{
					desiredWidth = containerWidth * 0.24 - 1; // 4
				}
				else if (containerWidth >= 800)
				{
					desiredWidth = containerWidth * 0.3 - 1; // 3
				}
				else if (containerWidth >= 520)
				{
					desiredWidth = containerWidth * 0.45 - 1; // 2
				}
				else
				{
					desiredWidth = containerWidth - 1; // 1
				}

				return desiredWidth;
			}

			return DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}