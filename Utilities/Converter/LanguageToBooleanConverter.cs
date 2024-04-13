using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

using static Pizza.Abstractions.ProgramAbstraction;

namespace Pizza.Utilities.Converter
{
	public class LanguageToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is ProgramLanguages selectedLanguage && parameter is string expectedLanguage)
			{
				return selectedLanguage.ToString() == expectedLanguage;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value && parameter is string selectedLanguage)
			{
				if (Enum.TryParse(selectedLanguage, out ProgramLanguages language))
				{
					return language;
				}
			}
			return DependencyProperty.UnsetValue;
		}
	}
}
