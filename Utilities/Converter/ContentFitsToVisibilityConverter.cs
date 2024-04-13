using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Linq;
using System.Windows.Controls;

namespace Pizza.Utilities.Converter
{
	public class ContentFitsToVisibilityConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length == 2 && values[0] is double stackPanelHeight && values[1] is ScrollViewer scrollViewer)
			{
				if (stackPanelHeight > scrollViewer.ActualHeight)
				{
					return Visibility.Visible;
				}
			}

			return Visibility.Collapsed;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
