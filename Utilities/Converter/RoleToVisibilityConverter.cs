using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

using static Pizza.Abstractions.ProgramAbstraction;

namespace Pizza.Utilities.Converter
{
	public class RoleToVisibilityConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length < 2 || !(values[0] is AppRoles role) || !(values[1] is string page))
			{
				return Visibility.Collapsed;
			}

			bool hasAccess = CheckAccess(role, page);
			return hasAccess ? Visibility.Visible : Visibility.Collapsed;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private bool CheckAccess(AppRoles role, string page)
		{
			// Проверьте роль и список страниц для доступа
			// Верните true, если роль имеет доступ к странице, иначе false

			// Пример проверки доступа для некоторых ролей и страниц
			switch (role)
			{
				case AppRoles.Customer:
					return page == "Catalog" || page == "Basket" || page == "Reviews";
				case AppRoles.Manager:
					return page == "AddProduct" || page == "Catalog" || page == "Reviews" || page == "AuthPermission" || page == "Employees";
				case AppRoles.Seller:
					return page == "Orders";
				case AppRoles.Courier:
					return page == "Orders";
				default:
					return false;
			}
		}
	}
}