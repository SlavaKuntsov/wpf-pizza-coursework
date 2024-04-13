using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

using static Pizza.Abstractions.ProductAbstraction;

namespace Pizza.Utilities.Converter
{
	public class CategoryToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is PizzaCategories selectedCategory && parameter is string categoryName)
			{
				return selectedCategory.ToString() == categoryName;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value && parameter is string categoryName)
			{
				if (Enum.TryParse(categoryName, out PizzaCategories category))
				{
					return category;
				}
			}
			return DependencyProperty.UnsetValue;
		}
	}
}
