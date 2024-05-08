using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using static Pizza.Abstractions.ProgramAbstraction;

namespace Pizza.Utilities.Converter
{
	public class RoleToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is AppRoles selectedRole && parameter is string roleName)
			{
				return selectedRole.ToString() == roleName;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value && parameter is string sizeName)
			{
				if (Enum.TryParse(sizeName, out AppRoles role))
				{
					return role;
				}
			}
			return DependencyProperty.UnsetValue;
		}
	}
}
