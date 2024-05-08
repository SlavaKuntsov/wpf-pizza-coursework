using System;
using System.Configuration;
using System.Windows;

using Pizza.Manager;
using Pizza.MVVM.View;
using Pizza.MVVM.View.Auth;

namespace Pizza
{
	public partial class App : Application
	{
		private Window _currentWindow;
		AuthManager _authManager;
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			_authManager = AuthManager.Instance;
			_auth = _authManager.Auth;
			_authManager.PropertyChanged += dataManager_PropertyChanged;

			CheckAuth();
		}

		private bool _auth { get; set; }
		public bool Auth
		{
			get { return _auth; }
			set { _auth = value; }
		}

		private void CheckAuth()
		{
			if (_authManager.Auth)
			{
				ShowMainWindow();
			}
			else
			{
				ShowAuthView();
			}
		}

		private void ShowMainWindow()
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();

			CloseCurrentWindow();

			_currentWindow = mainWindow;

		}

		private void ShowAuthView()
		{
			AuthView authWindow = new AuthView();
			authWindow.Show();

			CloseCurrentWindow();

			_currentWindow = authWindow;
		}

		private void CloseCurrentWindow()
		{
			if (_currentWindow != null)
			{
				_currentWindow.Close();
				_currentWindow = null;
			}
		}

		private void dataManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Auth")
			{
				Console.WriteLine("%%% CHECK");
				Auth = _authManager.Auth;
				CheckAuth();
			}
		}
	}
}
//encryptor.EncryptData("email@gmail.com");

//EncryptClass encryptor = new EncryptClass();

//Result<string> decryptedEmailResult = encryptor.GetDecryptedEmail();

//if (decryptedEmailResult.IsSuccess)
//{
//	string decryptedEmail = decryptedEmailResult.Value;
//	Console.WriteLine("Decrypted Email: " + decryptedEmail);
//}
//else
//{
//	Console.WriteLine("Failed to get decrypted email: " + decryptedEmailResult.Error);
//}