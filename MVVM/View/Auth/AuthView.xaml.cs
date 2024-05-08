using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pizza.MVVM.View.Auth
{
	/// <summary>
	/// Логика взаимодействия для AuthView.xaml
	/// </summary>
	public partial class AuthView : Window
	{
		public AuthView()
		{
			InitializeComponent();

			Console.WriteLine("INIIIIIIIIIIIT");
		}

		private void DragWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				DragMove();
			}
		}

		private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.WindowState = WindowState.Minimized;
		}

		private void WindowStateButton_Click(object sender, RoutedEventArgs e)
		{
			if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
			{
				Application.Current.MainWindow.WindowState = WindowState.Maximized;
				//this.MaxHeight = SystemParameters.WorkArea.Height + SystemParameters.BorderWidth * 2;
				//this.MaxWidth = SystemParameters.WorkArea.Width + SystemParameters.BorderWidth * 2;
			}
			else
			{
				Application.Current.MainWindow.WindowState = WindowState.Normal;
			}
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}
