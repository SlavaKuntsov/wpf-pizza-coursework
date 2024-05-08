using System;
using System.Configuration;

using Pizza.DataAccess;
using Pizza.Encrypt;
using Pizza.MVVM.Model;
using Pizza.Utilities;

using static Pizza.Abstractions.ProgramAbstraction;

namespace Pizza.Manager
{
	public class AuthManager : BaseViewModel
	{
		private static AuthManager instance;
		UnitOfWork _unitOfWork;
		SaveUserDataHelper file = new SaveUserDataHelper();

		public AuthManager()
		{
			CheckRoleAndConnection();
		}

		public void CheckRoleAndConnection()
		{
			UserData user = file.GetUserAuthData();

			Console.WriteLine("********************************* CheckRoleAndConnection");
			if (user != null)
			//if (!string.IsNullOrEmpty(user.Email))
			{
				Console.WriteLine("--------- STORED EMAIL: " + user.Email);

				ConnectionString = ConfigurationManager.ConnectionStrings["db_auth"].ConnectionString;
				_unitOfWork = new UnitOfWork(ConnectionString);

				Role = _unitOfWork.User.GetUserRole(user.Id);

				//Console.WriteLine("ROLEEEEE: " + Role + " !!!");

				switch (Role)
				{
					case AppRoles.Customer:
						// Логика для роли Customer
						ConnectionString = ConfigurationManager.ConnectionStrings["db_customer"].ConnectionString;
						break;
					case AppRoles.Manager:
						// Логика для роли Manager
						ConnectionString = ConfigurationManager.ConnectionStrings["db_manager"].ConnectionString;
						break;
					case AppRoles.Seller:
						// Логика для роли Seller
						ConnectionString = ConfigurationManager.ConnectionStrings["db_seller"].ConnectionString;
						break;
					case AppRoles.Courier:
						// Логика для роли Courier
						ConnectionString = ConfigurationManager.ConnectionStrings["db_courier"].ConnectionString;
						break;
				}
				Auth = true;

			}
			else
			{
				Console.WriteLine("--------- No email found in the file.");
				Role = AppRoles.Auth;
				ConnectionString = ConfigurationManager.ConnectionStrings["db_auth"].ConnectionString;
				Auth = false;
			}

			Console.WriteLine("ROLEEEEE: " + Role + " !!!");
			Console.WriteLine(ConnectionString);
		}

		public static AuthManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new AuthManager();
				}
				return instance;
			}
		}

		private bool _auth { get; set; }
		public bool Auth
		{
			get { return _auth; }
			set { _auth = value; OnPropertyChanged(nameof(Auth)); }
		}
		private string _connectionString { get; set; }
		public string ConnectionString
		{
			get { return _connectionString; }
			set { _connectionString = value; OnPropertyChanged(nameof(ConnectionString)); }
		}
		private AppRoles _role { get; set; }
		public AppRoles Role
		{
			get { return _role; }
			set { _role = value; OnPropertyChanged(nameof(Role)); }
		}

		//private bool _name { get; set; }
		//public bool Name
		//{
		//	get { return _name; }
		//	set { _name = value; OnPropertyChanged(nameof(Name)); }
		//}

		//private bool _surname { get; set; }
		//public bool Surname
		//{
		//	get { return _surname; }
		//	set { _surname = value; OnPropertyChanged(nameof(Surname)); }
		//}

		//private bool _email { get; set; }
		//public bool Email
		//{
		//	get { return _email; }
		//	set { _email = value; OnPropertyChanged(nameof(Email)); }
		//}

		//private bool _password { get; set; }
		//public bool Password
		//{
		//	get { return _password; }
		//	set { _password = value; OnPropertyChanged(nameof(Password)); }
		//}
	}
}
