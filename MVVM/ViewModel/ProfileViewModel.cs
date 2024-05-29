using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.Utilities;

namespace Pizza.MVVM.ViewModel
{
	public class ProfileViewModel : BaseViewModel
	{
		AuthManager _authManager;
		DataManager _dataManager;

		public ICommand UpdateUserCommand { get; set; }

		public ProfileViewModel()
		{
			_authManager = AuthManager.Instance;
			_dataManager = DataManager.Instance;

			// переделать на модель UserModel вместо множества полей
			User = _authManager.User;

			_dataManager.ReadCustomerOrderData();
			Orders = _dataManager.GetOrders();
			Orders.CollectionChanged += Orders_CollectionChanged;

			switch (_authManager.User.Role)
			{
				case Abstractions.ProgramAbstraction.AppRoles.Customer:
					OrdersVisibility = true;
					break;
				default:
					OrdersVisibility = false;
					break;
			}

			switch (_authManager.User.Role)
			{
				case Abstractions.ProgramAbstraction.AppRoles.Customer:
					CustomerVisibility = true;
					break;
				case Abstractions.ProgramAbstraction.AppRoles.Manager:
					CustomerVisibility = false;
					break;
				case Abstractions.ProgramAbstraction.AppRoles.Seller:
					CustomerVisibility = false;
					break;
				case Abstractions.ProgramAbstraction.AppRoles.Courier:
					CustomerVisibility = false;
					break;
				case Abstractions.ProgramAbstraction.AppRoles.Auth:
					CustomerVisibility = false;
					break;
				default:
					break;
			}


			UpdateUserCommand = new RelayCommand(UpdateUser);

			Role = _authManager.User.Role.ToString();
		}

		private void UpdateUser(object obj)
		{
			if (!string.IsNullOrWhiteSpace(User.Address))
			{
				_dataManager.UpdateUser(User.Id, User.Address);
			}
		}

		private void Orders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Console.WriteLine("Products_CollectionChanged @@@@@@@@@@@@@@@@@@@@@");
			Orders = _dataManager.GetOrders();
		}

		//private void CancelOrder(object obj)
		//{
		//	Console.WriteLine("**********");
		//}

		private ObservableCollection<OrderModel> _orders { get; set; }

		public ObservableCollection<OrderModel> Orders
		{
			get { return _orders; }
			set { _orders = value; OnPropertyChanged(nameof(Orders)); }
		}

		//private ObservableCollection<ProductModelNew> _ordersProduct { get; set; }

		//public ObservableCollection<ProductModelNew> OrdersProductList
		//{
		//	get { return _ordersProduct; }
		//	set { _ordersProduct = value; OnPropertyChanged(nameof(OrdersProductList)); }
		//}
		private bool _customerVisibility { get; set; }

		public bool CustomerVisibility
		{
			get { return _customerVisibility; }
			set { _customerVisibility = value; OnPropertyChanged(nameof(CustomerVisibility)); }
		}

		private string _role { get; set; }

		public string Role
		{
			get { return _role; }
			set { _role = value; OnPropertyChanged(nameof(Role)); }
		}

		private UserModel _user { get; set; }

		public UserModel User
		{
			get { return _user; }
			set { _user = value; OnPropertyChanged(nameof(User)); }
		}


		private bool _ordersVisibility { get; set; }

		public bool OrdersVisibility
		{
			get { return _ordersVisibility; }
			set { _ordersVisibility = value; OnPropertyChanged(nameof(OrdersVisibility)); }
		}
	}
}
