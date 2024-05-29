using System;
using System.Configuration;

using Pizza.DataAccess;
using Pizza.MVVM.Model;
using Pizza.Utilities;

using static Pizza.Abstractions.ProgramAbstraction;

namespace Pizza.Manager
{
	public class AuthManager : BaseViewModel
	{
		private static AuthManager instance;
		UnitOfWork _unitOfWork;
		CatalogManager _catalogManager;
		DataManager _dataManager;
		SaveUserDataHelper file = new SaveUserDataHelper();

		public AuthManager()
		{
			System.Console.WriteLine("AUTH MANAGER");

			User = new UserModel();

			Console.WriteLine("DSADASDSASASDSADSAD@@@@@@@@@@@@@@@@@@@@@@");


			CheckRoleAndConnection();
		}

		public void CheckRoleAndConnection()
		{
			Console.WriteLine("********************************* CheckRoleAndConnection");
			UserModel user = file.GetUserAuthData();
			ConnectionString = ConfigurationManager.ConnectionStrings["db_auth"].ConnectionString;

			if (user != null)
			//if (!string.IsNullOrEmpty(user.Email))
			{
				Console.WriteLine("--------- STORED EMAIL: " + user.Email + " " + user.Id + " " + user.Role);

				User = user;

				//ConnectionString = ConfigurationManager.ConnectionStrings["db_auth"].ConnectionString;
				_unitOfWork = new UnitOfWork(ConnectionString);

				User.Role = _unitOfWork.User.GetUserRole(user.Id);

				//Console.WriteLine("ROLEEEEE: " + Role + " !!!");

				switch (User.Role)
				{
					case AppRoles.Customer:
						ConnectionString = ConfigurationManager.ConnectionStrings["db_customer"].ConnectionString;
						break;
					case AppRoles.Manager:
						ConnectionString = ConfigurationManager.ConnectionStrings["db_manager"].ConnectionString;
						break;
					case AppRoles.Seller:
						ConnectionString = ConfigurationManager.ConnectionStrings["db_seller"].ConnectionString;
						break;
					case AppRoles.Courier:
						ConnectionString = ConfigurationManager.ConnectionStrings["db_courier"].ConnectionString;
						break;
				}
				Auth = true;

			}
			else
			{
				Console.WriteLine("--------- No email found in the file.");
				User.Role = AppRoles.Auth;

				_catalogManager = CatalogManager.Instance;


				_catalogManager.CurrentPage = 1;
				_dataManager = DataManager.Instance;
				_dataManager.ClearData();

				//ConnectionString = ConfigurationManager.ConnectionStrings["db_auth"].ConnectionString;
				Auth = false;
			}

			Console.WriteLine("___");
			Console.WriteLine("ROLEEEEE: " + User.Role + " !!!");
			Console.WriteLine(ConnectionString);

			BasketBoolVisibility = false;

			switch (User.Role)
			{
				case AppRoles.Customer:
					ManagerBoolVisibility = false;
					break;
				case AppRoles.Manager:
					ManagerBoolVisibility = true;
					break;
				case AppRoles.Seller:
					ManagerBoolVisibility = false;
					break;
				case AppRoles.Courier:
					ManagerBoolVisibility = false;
					break;
				case AppRoles.Auth:
					ManagerBoolVisibility = false;
					break;
				default:
					break;
			}
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
		//private AppRoles _role { get; set; }
		//public AppRoles Role
		//{
		//	get { return _role; }
		//	set { _role = value; OnPropertyChanged(nameof(Role)); }
		//}

		private UserModel _user { get; set; }

		public UserModel User
		{
			get { return _user; }
			set { _user = value;  OnPropertyChanged(nameof(User)); }
		}

		private bool _managerBoolVisibility { get; set; }
		public bool ManagerBoolVisibility
		{
			get { return _managerBoolVisibility; }
			set { _managerBoolVisibility = value; OnPropertyChanged(nameof(ManagerBoolVisibility)); }
		}

		private bool _basketBoolVisibility { get; set; }
		public bool BasketBoolVisibility
		{
			get { return _basketBoolVisibility; }
			set { _basketBoolVisibility = value; OnPropertyChanged(nameof(BasketBoolVisibility)); }
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
