using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Pizza.Utilities.Converter
{
	public class BoolToVisibilityMultiConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Console.WriteLine("------milti 2");
			if (values == null || values.Length != 2)
				return DependencyProperty.UnsetValue;

			Console.WriteLine("------milti 1");
			try
			{
				bool managerVisibility = (bool)values[0];
				bool visible = (bool)values[1];

				Console.WriteLine("------milti");
				Console.WriteLine(managerVisibility);
				Console.WriteLine(visible);

				if (managerVisibility && visible)
					return Visibility.Visible;
				if (managerVisibility && !visible)
					return Visibility.Collapsed;
				if (!managerVisibility && visible)
					return Visibility.Collapsed;
				if (!managerVisibility && !visible)
					return Visibility.Collapsed;
				else
					return Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return DependencyProperty.UnsetValue;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
