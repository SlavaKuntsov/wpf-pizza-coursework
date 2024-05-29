using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

using static Pizza.Abstractions.ProductAbstraction;

namespace Pizza.Utilities.Converter
{
	public class IsDeliveryToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is PizzaIsDelivery selectedCategory && parameter is string categoryName)
			{
				return selectedCategory.ToString() == categoryName;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value && parameter is string categoryName)
			{
				if (Enum.TryParse(categoryName, out PizzaIsDelivery category))
				{
					return category;
				}
			}
			return DependencyProperty.UnsetValue;
		}
	}
}
