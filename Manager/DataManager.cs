using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

using CSharpFunctionalExtensions;

using Pizza.Abstractions;
using Pizza.DataAccess;
using Pizza.Encrypt;
using Pizza.MVVM.Model;

using Pizza.Repository;
using Pizza.Utilities;

using Remotion.Linq.Clauses;

namespace Pizza.Manager
{
	public class DataManager : BaseViewModel
	{
		private static DataManager instance;
		private string _connectionString;

		private ObservableCollection<ProductModelNew> _products;
		private ObservableCollection<ProductModelNew> _basketProducts;
		private ObservableCollection<OrderModel> _orders;
		private ObservableCollection<ReviewModel> _reviews;
		private ObservableCollection<AuthPermissionModel> _authPerm;

		private UnitOfWork _unitOfWork;
		private readonly AuthManager _authManager;

		public DataManager()
		{
			_authManager = AuthManager.Instance;

			_connectionString = _authManager.ConnectionString;

			_authManager.PropertyChanged += _authManager_PropertyChanged;

			ChangeConnection();

			_products = new ObservableCollection<ProductModelNew>();
			_basketProducts = new ObservableCollection<ProductModelNew>();
			_orders = new ObservableCollection<OrderModel>();
			_reviews = new ObservableCollection<ReviewModel>();
			_authPerm = new ObservableCollection<AuthPermissionModel>();

			Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
			//ReadData();
			//ReadBasketData();
		}

		private void _authManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "ConnectionString")
			{
				_connectionString = _authManager.ConnectionString;
			}
		}

		private void ChangeConnection()
		{
			_unitOfWork = new UnitOfWork(_connectionString);
		}

		public static DataManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new DataManager();
				}
				return instance;
			}
		}

		public async void AddProduct(ProductModelNew product)
		{
			Console.WriteLine("add prod: " + product.Name);
			//_products.Add(product);

			await _unitOfWork.Product.AddProduct(product);
		}

		public async Task AddInBasket(ProductModelNew product)
		{
			Console.WriteLine("111111111111111111111111111");
			Guid customerId = _authManager.User.Id;
			//Console.WriteLine(productId + " " + count + " " + customerId);
			await _unitOfWork.Basket.AddInBasket(product, customerId);
			Console.WriteLine("22222222222222222222");
		}

		public async Task AddOrder(bool isDelivery)
		{
			Guid customerId = _authManager.User.Id;

			await _unitOfWork.Order.AddCustomerOrder(customerId, isDelivery);

			ReadBasketData();

			Console.WriteLine("ADD ORDER");
		}

		public async Task AddReview(string text)
		{
			Guid customerId = _authManager.User.Id;

			await _unitOfWork.Review.AddReview(customerId, text);

			ReadAllReviewsData();

			Console.WriteLine("ADD ORDER");
		}

		public async Task DeleteFromBasket(int basketId)
		{
			Console.WriteLine("111111111111111111111111111");
			await _unitOfWork.Basket.DeleteFromBasket(basketId);
			Console.WriteLine("22222222222222222222");

			ReadBasketData();
		}

		public async Task EditProduct(ProductModelNew product)
		{
			Console.WriteLine("EDIT 1");
			await _unitOfWork.Product.EditProduct(product);

			ReadData();
		}
		
		public async void UpdateProductInBasket(ProductModelNew product)
		{
			Console.WriteLine("UPDATE");
			await _unitOfWork.Basket.UpdateProductInBasket(product);

			ReadBasketData();
		}

		public async Task DeleteProduct(int id)
		{
			await _unitOfWork.Product.DeleteProduct(id);
			ReadData();
		}

		public async Task DeleteReviewFromCustomer(int id)
		{
			Guid customerId = _authManager.User.Id;

			await _unitOfWork.Review.DeleteReviewFromCustomer(id, customerId);
			ReadAllReviewsData();
		}

		public async Task DeleteReview(int id)
		{
			await _unitOfWork.Review.DeleteReview(id);
			ReadAllReviewsData();
		}

		public async void CancelOrder(Guid orderId)
		{
			await _unitOfWork.Order.CancelOrder(orderId);
			ReadAllOrdersData();
			Console.WriteLine("CANCEL ORDER");
		}

		public async void AcceptOrder(Guid orderId)
		{
			Guid customerId = _authManager.User.Id;

			await _unitOfWork.Order.AcceptOrder(orderId, customerId);

			ReadAllOrdersData();
			Console.WriteLine("ACCEPT ORDER");
		}

		public async void CompleteOrder(Guid orderId)
		{
			Guid customerId = _authManager.User.Id;

			await _unitOfWork.Order.CompleteOrder(orderId, customerId);

			ReadAllOrdersData();
			Console.WriteLine("Complete ORDER");
		}

		public async void DeliverOrder(Guid orderId)
		{
			Guid courierId = _authManager.User.Id;

			await _unitOfWork.Order.DeliverOrder(orderId, courierId);

			ReadCourierOrdersData();
			Console.WriteLine("DELIVER ORDER");
		}

		public async void CompleteDeliverOrder(Guid orderId)
		{
			Guid courierId = _authManager.User.Id;

			await _unitOfWork.Order.CompleteDeliverOrder(orderId, courierId);

			ReadCourierOrdersData();
			Console.WriteLine("Complete Deliver ORDER");
		}

		public ObservableCollection<ProductModelNew> GetProducts()
		{
			return _products;
		}

		public ObservableCollection<ProductModelNew> GetBasket()
		{
			return _basketProducts;
		}

		public ObservableCollection<OrderModel> GetOrders()
		{
			return _orders;
		}

		public ObservableCollection<ReviewModel> GetReviews()
		{
			return _reviews;
		}

		public ObservableCollection<AuthPermissionModel> GetEmployees()
		{
			return _authPerm;
		}

		public int GetProductsCount()
		{
			Console.WriteLine("ROLEW AFTER CATALOG" + _authManager.User.Role);
			Console.WriteLine(_authManager.ConnectionString);
			Result<int> result = _unitOfWork.Product.GetProductsCount();

			Console.WriteLine("!!!! 2");

			if (result.IsFailure)
			{
				return 0;
			}
			return result.Value;
		}

		public int GetProductsCountInstock()
		{
			Console.WriteLine("ROLEW AFTER CATALOG" + _authManager.User.Role);
			Console.WriteLine(_authManager.ConnectionString);
			Result<int> result = _unitOfWork.Product.GetProductsCountInstock();

			Console.WriteLine("!!!! 2");

			if (result.IsFailure)
			{
				return 0;
			}
			return result.Value;
		}

		public async void ReadData()
		{
			Console.WriteLine("############# 1");
			//var products = await _unitOfWork.Product.GetAllProductsPreview();
			var products = await _unitOfWork.Product.GetProductPage();


			Console.WriteLine("############# 2");
			if (products.IsFailure)
			{
				return;
			}
			_products.Clear();
			Console.WriteLine("############# 3");
			foreach (ProductModelNew item in products.Value)
			{
				_products.Add(item);
			}

			//_catalogManager.LastPageProductId = _products.Last().Id;
			//Console.WriteLine("_catalogManager.NumberSelectionModel.LastProductId: " + _catalogManager.LastPageProductId);

			//_products = products.Value;
			Console.WriteLine("PROD LENGTH: " + _products.Count());
		}

		public async void ReadDataInstock(string sortColumn, string sortDirection)
		{
			Console.WriteLine("############# 1");
			//var products = await _unitOfWork.Product.GetAllProductsPreview();
			var products = await _unitOfWork.Product.GetProductPageInstock(sortColumn, sortDirection);


			Console.WriteLine("############# 2");
			if (products.IsFailure)
			{
				return;
			}
			_products.Clear();
			Console.WriteLine("############# 3");
			foreach (ProductModelNew item in products.Value)
			{
				_products.Add(item);
			}

			//_catalogManager.LastPageProductId = _products.Last().Id;
			//Console.WriteLine("_catalogManager.NumberSelectionModel.LastProductId: " + _catalogManager.LastPageProductId);

			//_products = products.Value;
			Console.WriteLine("PROD LENGTH: " + _products.Count());
		}

		public async void ReadCustomerOrderData()
		{
			Guid customerId = _authManager.User.Id;

			var orders = await _unitOfWork.Order.GetCustomerOrders(customerId);

			foreach (var item in orders.Value)
			{
				Console.WriteLine("order: " + item.Id);

				foreach (var item2 in item.OrderProducts)
				{
					Console.WriteLine("item: " + item2.Name);
				}
			}

			_orders.Clear();
			foreach (var item in orders.Value)
			{
				_orders.Add(item);
			}
		}

		public async void ReadAllOrdersData()
		{
			var orders = await _unitOfWork.Order.GetAllOrders();

			foreach (var item in orders.Value)
			{
				Console.WriteLine("order: " + item.Id);

				foreach (var item2 in item.OrderProducts)
				{
					Console.WriteLine("item: " + item2.Name);
				}
			}

			_orders.Clear();
			foreach (var item in orders.Value)
			{
				_orders.Add(item);
			}
		}

		public async void ReadAllReviewsData()
		{
			Guid customerId = _authManager.User.Id;

			var orders = await _unitOfWork.Review.GetAllReviews(customerId);

			if (orders.IsFailure)
			{
				return;
			}

			_reviews.Clear();
			foreach (var item in orders.Value)
			{
				_reviews.Add(item);
			}
		}

		public async void ReadAllUnauthorizedEmployeesData()
		{
			var orders = await _unitOfWork.User.GetUnauthorizedEmployees();

			_authPerm.Clear();
			foreach (var item in orders)
			{
				_authPerm.Add(item);
			}
		}

		public async void ReadAllAuthorizedEmployeesData()
		{
			var orders = await _unitOfWork.User.GetAuthorizedEmployees();

			_authPerm.Clear();
			foreach (var item in orders)
			{
				_authPerm.Add(item);
			}
		}

		public async void ReadCourierOrdersData()
		{
			var orders = await _unitOfWork.Order.GetCourierOrders();

			if (orders.IsFailure)
			{
				return;
			}

			foreach (var item in orders.Value)
			{
				Console.WriteLine("order: " + item.Id);

				foreach (var item2 in item.OrderProducts)
				{
					Console.WriteLine("item: " + item2.Name);
				}
			}

			_orders.Clear();
			foreach (var item in orders.Value)
			{
				_orders.Add(item);
			}
		}

		public ProductModelNew ReadProductInfoData(int id)
		{
			var products = _unitOfWork.Product.GetProductInfo(id);

			return products.Value;
		}

		public ProductModelNew ReadBasketInfoData(int basketId)
		{
			Guid customerId = _authManager.User.Id;

			var products = _unitOfWork.Basket.GetBasketProductInfo(customerId, basketId);

			return products.Value;
		}

		public void ClearData()
		{
			_products.Clear();
		}

		public async void ReadBasketData()
		{
			Console.WriteLine("############# 1");
			Guid customerId = _authManager.User.Id;
			var basketProducts = await _unitOfWork.Basket.GetBasket(customerId);

			Console.WriteLine("############# 2");
			if (basketProducts.IsFailure)
			{
				return;
			}
			_basketProducts.Clear();
			Console.WriteLine("############# 3");
			foreach (var item in basketProducts.Value)
			{
				_basketProducts.Add(item);
				Console.WriteLine("BASKET PROD: " + item.Name);
				foreach (var i in item.PropertyValue)
				{
					Console.WriteLine("value: " + i);
				}
			}
		}

		public async void UpdateUser(Guid userId, string address)
		{
			await _unitOfWork.User.UpdateCustomer(userId, address);
		}

		public async void AuthorizeEmployee(Guid userId)
		{
			await _unitOfWork.User.AuthorizeEmployee(userId);

			ReadAllUnauthorizedEmployeesData();
		}

		public async void DeauthorizeEmployee(Guid userId)
		{
			await _unitOfWork.User.DeauthorizeEmployee(userId);

			ReadAllAuthorizedEmployeesData();
		}

		public async void DeleteEmployee(Guid userId)
		{
			await _unitOfWork.User.DeleteEmployee(userId);

			ReadAllAuthorizedEmployeesData();
		}

	}
}