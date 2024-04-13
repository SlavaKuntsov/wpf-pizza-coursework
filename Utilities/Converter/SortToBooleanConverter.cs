using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

using static Pizza.Abstractions.ProgramAbstraction;

namespace Pizza.Utilities.Converter
{
	public class SortToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is SortByPropertyAll selectedSortProperty && parameter is string expectedSortProperty)
			{
				return selectedSortProperty.ToString() == expectedSortProperty;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value && parameter is string selectedLanguage)
			{
				if (Enum.TryParse(selectedLanguage, out SortByPropertyAll sortProperty))
				{
					return sortProperty;
				}
			}
			return DependencyProperty.UnsetValue;
		}
	}
}
