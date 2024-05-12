using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using Pizza.Manager;

namespace Pizza.Utilities.Converter
{
	public class PageToBolleanConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			CatalogManager catalogManager = CatalogManager.Instance;

			var value1 = values[0];

			if (value1 is int intValue)
			{
				return intValue == (int)catalogManager.CurrentPage;
			}

			return false;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			bool isChecked = (bool)value;

			if (isChecked)
			{
				return new object[] { true };
			}

			return new object[] { Binding.DoNothing };
		}
	}
}
