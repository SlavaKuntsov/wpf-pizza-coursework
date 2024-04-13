using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Pizza
{
	public partial class MainWindow : Window
	{
		Cursor cursor;

		public MainWindow()
		{
			InitializeComponent();

			Console.WriteLine("___________________________");

			string cursorDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Assets\\cursors";
			cursor = new Cursor($"{cursorDirectory}\\hello_kitty2.cur");

			Project.Cursor = cursor;
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
			System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized;
		}

		private void WindowStateButton_Click(object sender, RoutedEventArgs e)
		{
			if (System.Windows.Application.Current.MainWindow.WindowState != WindowState.Maximized)
			{
				System.Windows.Application.Current.MainWindow.WindowState = WindowState.Maximized;
				//this.MaxHeight = SystemParameters.WorkArea.Height + SystemParameters.BorderWidth * 2;
				//this.MaxWidth = SystemParameters.WorkArea.Width + SystemParameters.BorderWidth * 2;
			}
			else
			{
				System.Windows.Application.Current.MainWindow.WindowState = WindowState.Normal;
			}
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}
	}
}
