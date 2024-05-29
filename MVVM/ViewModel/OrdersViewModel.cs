using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.Utilities;

namespace Pizza.MVVM.ViewModel
{
	public class OrdersViewModel : BaseViewModel
	{
		AuthManager _authManager;
		DataManager _dataManager;
		public OrdersViewModel()
		{
			_authManager = AuthManager.Instance;
			_dataManager = DataManager.Instance;

			// переделать на модель UserModel вместо множества полей

			switch (_authManager.User.Role)
			{
				case Abstractions.ProgramAbstraction.AppRoles.Seller:
					_dataManager.ReadAllOrdersData();
					break;
				case Abstractions.ProgramAbstraction.AppRoles.Courier:
					_dataManager.ReadCourierOrdersData();
					break;
			}

			Orders = _dataManager.GetOrders();
			Orders.CollectionChanged += Orders_CollectionChanged;

			switch (_authManager.User.Role)
			{
				case Abstractions.ProgramAbstraction.AppRoles.Customer:
					SellerVisibility = false;
					CustomerVisibility = true;
					break;
				case Abstractions.ProgramAbstraction.AppRoles.Manager:
					SellerVisibility = true;
					CustomerVisibility = false;
					break;
				case Abstractions.ProgramAbstraction.AppRoles.Seller:
					SellerVisibility = true;
					CustomerVisibility = false;
					break;
				case Abstractions.ProgramAbstraction.AppRoles.Courier:
					SellerVisibility = true;
					CustomerVisibility = false;
					break;
				case Abstractions.ProgramAbstraction.AppRoles.Auth:
					SellerVisibility = false;
					CustomerVisibility = false;
					break;
				default:
					break;
			}

			Role = _authManager.User.Role.ToString();
			Console.WriteLine("ROLE::: " + Role);

		}

		private void Orders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Console.WriteLine("Products_CollectionChanged @@@@@@@@@@@@@@@@@@@@@");
			Orders = _dataManager.GetOrders();

			foreach (var o in Orders)
			{
				Console.WriteLine("status: " + o.Status);
				switch (o.Status)
				{
					case "Отменен":
						CreatedOrderVisibility = true;
						ProcessedOrderVisibility = true;
						CanceledOrderVisibility = false;
						ReadyOrderVisibility = true;
						break;
					case "Готовится":
						CreatedOrderVisibility = true;
						ProcessedOrderVisibility = false;
						CanceledOrderVisibility = true;
						ReadyOrderVisibility = true;
						break;
					case "Оформлен":
						CreatedOrderVisibility = false;
						ProcessedOrderVisibility = true;
						CanceledOrderVisibility = false;
						ReadyOrderVisibility = true;
						break;
					case "Завершен(Выдан)":
						CreatedOrderVisibility = true;
						ProcessedOrderVisibility = true;
						CanceledOrderVisibility = false;
						ReadyOrderVisibility = false;
						break;
					case "В пути":
						CreatedOrderVisibility = true;
						ProcessedOrderVisibility = true;
						CanceledOrderVisibility = false;
						ReadyOrderVisibility = false;
						break;
					case "Завершен(Доставлен)":
						CreatedOrderVisibility = true;
						ProcessedOrderVisibility = true;
						CanceledOrderVisibility = false;
						ReadyOrderVisibility = false;
						break;
					default:
						break;
				}

				if (o.Status == "Оформлен" && _authManager.User.Role == Abstractions.ProgramAbstraction.AppRoles.Customer)
				{
					CanceledOrderVisibility = true;
				}


				Console.WriteLine("visible");
				//Console.WriteLine(CustomerVisibility);
				//Console.WriteLine(SellerVisibility);
				Console.WriteLine("btn visible");
				Console.WriteLine(CreatedOrderVisibility);
				Console.WriteLine(ProcessedOrderVisibility);
				Console.WriteLine(CanceledOrderVisibility);
				Console.WriteLine(ReadyOrderVisibility);
			}
		}

		private ObservableCollection<OrderModel> _orders { get; set; }

		public ObservableCollection<OrderModel> Orders
		{
			get { return _orders; }
			set { _orders = value; OnPropertyChanged(nameof(Orders)); }
		}

		private string _role { get; set; }

		public string Role
		{
			get { return _role; }
			set { _role = value; OnPropertyChanged(nameof(Role)); }
		}

		private bool _sellerVisibility { get; set; }

		public bool SellerVisibility
		{
			get { return _sellerVisibility; }
			set { _sellerVisibility = value; OnPropertyChanged(nameof(SellerVisibility)); }
		}

		private bool _customerVisibility { get; set; }

		public bool CustomerVisibility
		{
			get { return _customerVisibility; }
			set { _customerVisibility = value; OnPropertyChanged(nameof(CustomerVisibility)); }
		}

		private bool _createdOrderVisibility { get; set; }

		public bool CreatedOrderVisibility
		{
			get { return _createdOrderVisibility; }
			set { _createdOrderVisibility = value; OnPropertyChanged(nameof(CreatedOrderVisibility)); }
		}

		private bool _processedOrderVisibility { get; set; }

		public bool ProcessedOrderVisibility
		{
			get { return _processedOrderVisibility; }
			set { _processedOrderVisibility = value; OnPropertyChanged(nameof(ProcessedOrderVisibility)); }
		}

		private bool _readyOrderVisibility { get; set; }

		public bool ReadyOrderVisibility
		{
			get { return _readyOrderVisibility; }
			set { _readyOrderVisibility = value; OnPropertyChanged(nameof(ReadyOrderVisibility)); }
		}

		private bool _canceledOrderVisibility { get; set; }

		public bool CanceledOrderVisibility
		{
			get { return _canceledOrderVisibility; }
			set { _canceledOrderVisibility = value; OnPropertyChanged(nameof(CanceledOrderVisibility)); }
		}

		private int _abc { get; set; }

		public int abc
		{
			get { return _abc; }
			set { _abc = value; OnPropertyChanged(nameof(abc)); }
		}

		private bool _sellerIdVisibility { get; set; }

		public bool SellerIdVisibility
		{
			get { return _sellerIdVisibility; }
			set { _sellerIdVisibility = value; OnPropertyChanged(nameof(SellerIdVisibility)); }
		}

		private bool _courierIdVisibility { get; set; }

		public bool CourierIdVisibility
		{
			get { return _courierIdVisibility; }
			set { _courierIdVisibility = value; OnPropertyChanged(nameof(CourierIdVisibility)); }
		}

	}
}
