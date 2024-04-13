using System;
using System.Windows;

namespace Pizza
{
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
		}
	}
}