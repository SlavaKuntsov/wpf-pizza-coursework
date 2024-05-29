using System;
using System.Configuration;
using System.Windows.Input;

using CSharpFunctionalExtensions;

using Pizza.DataAccess;
using Pizza.Encrypt;
using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.Utilities;

using static Pizza.Abstractions.ProgramAbstraction;

namespace Pizza.MVVM.ViewModel.Auth
{
	public class AuthViewModel : BaseViewModel
	{
		UnitOfWork _unitOfWork;
		AuthManager _authManager;
		private object _currentView;
		public object CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
		}

		public ICommand ChangeToLogInCommand { get; set; }
		public ICommand ChangeToSignUpCommand { get; set; }
		public ICommand LogInCommand { get; set; }
		public ICommand SignUpCommand { get; set; }
		public AuthViewModel()
		{
			_authManager = AuthManager.Instance;
			_unitOfWork = new UnitOfWork(_authManager.ConnectionString);

			CurrentView = new LogInViewModel();
			AuthTypeLogin = true;
			AuthTypeSignup = false;

			ChangeToLogInCommand = new RelayCommand(ChangeToLogIn);
			ChangeToSignUpCommand = new RelayCommand(ChangeToSignUp);
			LogInCommand = new RelayCommand(LogIn);
			SignUpCommand = new RelayCommand(SignUp);

			Role = AppRoles.Customer;
		}

		private void ChangeToLogIn(object obj)
		{
			//CurrentView = new LogInViewModel();
			AuthTypeLogin = true;
			AuthTypeSignup = false;
		}

		private void ChangeToSignUp(object obj)
		{
			//CurrentView = new SignUpViewModel();
			AuthTypeLogin = false;
			AuthTypeSignup = true;
		}

		private void LogIn(object obj)
		{
			if (string.IsNullOrWhiteSpace(LoginEmail) && string.IsNullOrWhiteSpace(LoginPassword))
			{
				AuthError = "Введите данные для авторизации.";
				return;
			}

			var login = _unitOfWork.User.LogIn(LoginEmail, LoginPassword);

			if (login.IsFailure)
			{
				AuthError = login.Error;

				//_authManager.CheckRoleAndConnection();
				return;
			}

			SaveUserDataHelper file = new SaveUserDataHelper();
			file.SaveUserAuthData(login.Value.Id, login.Value.Name, login.Value.Surname, login.Value.Email, login.Value.Password, login.Value.Address);

			_authManager.CheckRoleAndConnection();

			Console.WriteLine("ADD");
		}

		private void SignUp(object obj)
		{

			Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% 7&&&&&&&&&&&&&&&&&&");
			if (Role == AppRoles.Customer)
			{
				if (
						!string.IsNullOrWhiteSpace(Name) &&
						!string.IsNullOrWhiteSpace(Surname) &&
						!string.IsNullOrWhiteSpace(Email) &&
						!string.IsNullOrWhiteSpace(Password) &&
						!string.IsNullOrWhiteSpace(Address))
				{
					var signup = _unitOfWork.User.SignUp(Role, Name, Surname, Email, Password, Address);
					if (signup.IsFailure)
					{
						AuthError = signup.Error;
						Console.WriteLine(signup.Error);
						return;
					}

					SaveUserDataHelper file = new SaveUserDataHelper();
					file.SaveUserAuthData(signup.Value.Id, signup.Value.Name, signup.Value.Surname, signup.Value.Email, signup.Value.Password, "");

					UserModel user = file.GetUserAuthData();

					if (user != null)
					{
						Console.WriteLine("------" + user.Id.ToString() + user.Email);
					}

					Console.WriteLine("ADD");

					_authManager.CheckRoleAndConnection();

					return;
				}
				AuthError = "Введите данные для регистрации.";
			}
			else
			{
				if (
						!string.IsNullOrWhiteSpace(Name) &&
						!string.IsNullOrWhiteSpace(Surname) &&
						!string.IsNullOrWhiteSpace(Email) &&
						!string.IsNullOrWhiteSpace(Password))
				{
					var signup = _unitOfWork.User.SignUp(Role, Name, Surname, Email, Password, "");

					Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");

					AuthError = "auth_permission is false";

					if (signup.IsFailure)
					{
						AuthError = signup.Error;
						Console.WriteLine(signup.Error);
						return;
					}

					//SaveUserDataHelper file = new SaveUserDataHelper();
					//file.SaveUserAuthData(signup.Value.Id, signup.Value.Name, signup.Value.Surname, signup.Value.Email, signup.Value.Password, "");

					//UserModel user = file.GetUserAuthData();

					//if (user != null)
					//{
					//	Console.WriteLine("------" + user.Id.ToString() + user.Email);
					//}

					//Console.WriteLine("ADD");

					//_authManager.CheckRoleAndConnection();

					return;
				}
				AuthError = "Введите данные для регистрации.";
			}
		}

		private bool _authTypeLogin { get; set; }
		public bool AuthTypeLogin
		{
			get { return _authTypeLogin; }
			set { _authTypeLogin = value; OnPropertyChanged(nameof(AuthTypeLogin)); }
		}
		private bool _authTypeSignup { get; set; }
		public bool AuthTypeSignup
		{
			get { return _authTypeSignup; }
			set { _authTypeSignup = value; OnPropertyChanged(nameof(AuthTypeSignup)); }
		}

		private AppRoles _role { get; set; }
		public AppRoles Role
		{
			get { return _role; }
			set { _role = value; OnPropertyChanged(nameof(Role)); }
		}

		private string _name { get; set; }
		public string Name
		{
			get { return _name; }
			set { _name = value; OnPropertyChanged(nameof(Name)); }
		}

		private string _surname { get; set; }
		public string Surname
		{
			get { return _surname; }
			set { _surname = value; OnPropertyChanged(nameof(Surname)); }
		}

		private string _email { get; set; }
		public string Email
		{
			get { return _email; }
			set { _email = value; OnPropertyChanged(nameof(Email)); }
		}

		private string _password { get; set; }
		public string Password
		{
			get { return _password; }
			set { _password = value; OnPropertyChanged(nameof(Password)); }
		}

		private string _address { get; set; }
		public string Address
		{
			get { return _address; }
			set { _address = value; OnPropertyChanged(nameof(Address)); }
		}

		private string _loginEmail { get; set; }
		public string LoginEmail
		{
			get { return _loginEmail; }
			set { _loginEmail = value; OnPropertyChanged(nameof(LoginEmail)); }
		}

		private string _loginPassword { get; set; }
		public string LoginPassword
		{
			get { return _loginPassword; }
			set { _loginPassword = value; OnPropertyChanged(nameof(LoginPassword)); }
		}

		private string _authError { get; set; }
		public string AuthError
		{
			get { return _authError; }
			set { _authError = value; OnPropertyChanged(nameof(AuthError)); }
		}
	}
}
