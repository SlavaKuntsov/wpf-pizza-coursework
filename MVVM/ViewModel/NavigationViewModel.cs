using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;

using Pizza.Abstractions;
using Pizza.DataAccess;
using Pizza.Manager;
using Pizza.Utilities;

using static Pizza.Abstractions.ProgramAbstraction;

namespace Pizza.MVVM.ViewModel
{
	public class NavigationViewModel : BaseViewModel
	{
		UnitOfWork _unitOfWork;
		AuthManager _authManager;

		private readonly ProgramAbstraction programAbstraction;
		private readonly LocalizationManager _localizationManager;
		private readonly CatalogManager _catalogStateManager;

		private object _currentView;
		public object CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
		}

		public ICommand HomeCommand { get; set; }
		public ICommand CatalogCommand { get; set; }
		public ICommand BasketCommand { get; set; }
		public ICommand OrdersCommand { get; set; }
		public ICommand ProfileCommand { get; set; }
		public ICommand ReviewsCommand { get; set; }
		public ICommand AuthPermissionCommand { get; set; }
		public ICommand EmployeesCommand { get; set; }
		public ICommand LogOutCommand { get; set; }

		public Dictionary<ProgramLanguages, string> ProgramLanguagesDictionary;

		public NavigationViewModel()
		{
			_authManager = AuthManager.Instance;
			_unitOfWork = new UnitOfWork(_authManager.ConnectionString);

			Role = _authManager.User.Role;

			HomeCommand = new RelayCommand(Home);
			CatalogCommand = new RelayCommand(Catalog);
			BasketCommand = new RelayCommand(Basket);
			OrdersCommand = new RelayCommand(Orders);
			ProfileCommand = new RelayCommand(Profile);
			ReviewsCommand = new RelayCommand(Reviews);
			AuthPermissionCommand = new RelayCommand(AuthPermission);
			EmployeesCommand = new RelayCommand(Employees);
			LogOutCommand = new RelayCommand(LogOut);

			switch (Role)
			{
				case AppRoles.Customer:
					CurrentView = new CatalogViewModel();
					break;
				case AppRoles.Manager:
					CurrentView = new HomeViewModel();
					break;
				case AppRoles.Seller:
					CurrentView = new OrdersViewModel();
					break;
				case AppRoles.Courier:
					CurrentView = new OrdersViewModel();
					break;
			}

			programAbstraction = new ProgramAbstraction();
			ProgramLanguagesDictionary = programAbstraction.ProgramLanguagesDictionary;

			_localizationManager = LocalizationManager.Instance;

			_catalogStateManager = CatalogManager.Instance;
			SearchVisibility = _catalogStateManager.SearchVisibility;
			SortVisibility = _catalogStateManager.SortVisibility;
			ButtonsVisibility = _catalogStateManager.ButtonsVisibility;
		}

		private void Employees(object obj)
		{
			CurrentView = new EmployeesViewModel();
			ButtonsVisibility = false;
			_authManager.BasketBoolVisibility = false;
		}

		private void Reviews(object obj)
		{
			Console.WriteLine("))))))*************************************** reviews");

			CurrentView = new ReviewsViewModel();
			ButtonsVisibility = false;
			_authManager.BasketBoolVisibility = false;
		}

		private void Orders(object obj)
		{
			CurrentView = new OrdersViewModel();
			ButtonsVisibility = false;
			_authManager.BasketBoolVisibility = false;
		}

		private void Home(object obj)
		{
			CurrentView = new HomeViewModel();
			ButtonsVisibility = false;
			_authManager.BasketBoolVisibility = false;
		}

		private void Catalog(object obj)
		{
			CurrentView = new CatalogViewModel();
			ButtonsVisibility = true;
			_authManager.BasketBoolVisibility = false;
		}

		private void Basket(object obj)
		{
			Console.WriteLine("))))))*************************************** Basket");
			CurrentView = new BasketViewModel();
			ButtonsVisibility = false;
			_authManager.BasketBoolVisibility = true;
		}

		private void Profile(object obj)
		{
			CurrentView = new ProfileViewModel();
			ButtonsVisibility = false;
			_authManager.BasketBoolVisibility = false;
			Console.WriteLine(_authManager.BasketBoolVisibility);
		}

		private void AuthPermission(object obj)
		{
			CurrentView = new AuthPermissionViewModel();
			ButtonsVisibility = false;
			_authManager.BasketBoolVisibility = false;
			Console.WriteLine(_authManager.BasketBoolVisibility);
		}

		private void LogOut(object obj)
		{
			SaveUserDataHelper file = new SaveUserDataHelper();
			file.ClearUserData();

			_authManager.CheckRoleAndConnection();
		}

		public ProgramLanguages _language { get; set; }
		public ProgramLanguages Language
		{
			get { return _language; }
			set
			{
				if (value != _language)
				{
					switch (value.ToString())
					{
						case "Ru":
							_localizationManager.CurrentLanguage = new CultureInfo("ru-RU");
							break;
						case "En":
							_localizationManager.CurrentLanguage = new CultureInfo("en-US");
							break;
					}
					_language = value;
					OnPropertyChanged(nameof(Language));
				}
			}
		}

		private bool _searchVisibility { get; set; }
		public bool SearchVisibility
		{
			get { return _searchVisibility; }
			set
			{
				_searchVisibility = value;
				_catalogStateManager.SearchVisibility = value;
				OnPropertyChanged(nameof(SearchVisibility));
			}
		}

		private bool _sortVisibility { get; set; }
		public bool SortVisibility
		{
			get { return _sortVisibility; }
			set
			{
				_sortVisibility = value;
				_catalogStateManager.SortVisibility = value;
				OnPropertyChanged(nameof(SortVisibility));
			}
		}
		private bool _buttonsVisibility { get; set; }
		public bool ButtonsVisibility
		{
			get { return _buttonsVisibility; }
			set
			{
				_buttonsVisibility = value;
				OnPropertyChanged(nameof(ButtonsVisibility));
			}
		}

		private AppRoles _role { get; set; }
		public AppRoles Role
		{
			get { return _role; }
			set { _role = value; OnPropertyChanged(nameof(Role)); }
		}
	}
}
