using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Pizza.MVVM.ViewModel;

namespace Pizza.MVVM.View
{
	/// <summary>
	/// Логика взаимодействия для ProductModal.xaml
	/// </summary>
	public partial class ProductModalView : UserControl
	{
		ProductModalViewModel modal;
		public ProductModalView()
		{
			InitializeComponent();

			DataContext = modal;
		}

		private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Visibility = Visibility.Collapsed;
		}
		private void Rectangle_MouseDown(object sender, RoutedEventArgs e)
		{
			this.Visibility = Visibility.Collapsed;
		}

		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			foreach (var ch in e.Text)
			{
				if (!char.IsDigit(ch) || ch == ' ')
				{
					e.Handled = true; // Отменяет событие ввода
					break;
				}
			}
		}

		private void TextBox_PreviewTextInput_WithDot(object sender, TextCompositionEventArgs e)
		{
			foreach (var ch in e.Text)
			{
				if (!char.IsDigit(ch) && ch != '.' || ch == ' ')
				{
					e.Handled = true; // Cancels the input event
					break;
				}

				// Allowing only one decimal point
				if (ch == '.' && ((TextBox)sender).Text.Contains('.'))
				{
					e.Handled = true; // Cancels the input event
					break;
				}
			}
		}

		private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space)
			{
				e.Handled = true; // Отменяет событие ввода
			}
		}
	}
}
