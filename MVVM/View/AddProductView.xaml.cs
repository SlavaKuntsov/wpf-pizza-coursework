using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Pizza.Abstractions;
using Pizza.MVVM.ViewModel;

namespace Pizza.MVVM.View
{
	/// <summary>
	/// Логика взаимодействия для AddProductView.xaml
	/// </summary>
	public partial class AddProductView : UserControl
	{
		AddProductViewModel add;

		public AddProductView()
		{
			InitializeComponent();



			add = new AddProductViewModel();
			DataContext = add;
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
