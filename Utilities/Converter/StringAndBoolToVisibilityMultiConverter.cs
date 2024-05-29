using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Pizza.Utilities.Converter
{
	public class StringAndBoolToVisibilityMultiConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Console.WriteLine("!!!!!!!!!!!");
			if (values == null || values.Length != 4)
				return DependencyProperty.UnsetValue;

			//if (!(values[0] is bool) || !(values[1] is string) || !(values[2] is string))
			//	return DependencyProperty.UnsetValue;
			try
			{

				string role = (string)values[0];
				string targetRole = (string)values[1];
				string status = (string)values[2];
				string targetStatus= (string)values[3];

				Console.WriteLine("---------converter");
				Console.WriteLine(role);
				Console.WriteLine(targetRole);
				Console.WriteLine(status);
				Console.WriteLine(targetStatus);

				if (role == targetRole && status == targetStatus)
				{
					return Visibility.Visible;
				}
				else
					return Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return DependencyProperty.UnsetValue;
			//if (managerVisibility && status.Equals(targetString, StringComparison.OrdinalIgnoreCase))
			//	return Visibility.Visible;
			//else
			//	return Visibility.Collapsed;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
