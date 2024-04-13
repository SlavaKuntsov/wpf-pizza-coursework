using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Pizza.Utilities.Converter
{
	internal class ImageSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && !string.IsNullOrEmpty(value.ToString()))
			{
				// Возвращаем значение ImageSource, если оно не пустое или недопустимое
				return new BitmapImage(new Uri(value.ToString()));
			}
			else
			{
				// Возвращаем изображение по умолчанию, если значение ImageSource пусто или недопустимо
				return new BitmapImage(new Uri("./Assets/DefaultImage.png", UriKind.Relative));
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
