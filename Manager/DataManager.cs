using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

using CSharpFunctionalExtensions;

using Pizza.Abstractions;
using Pizza.DataAccess;
using Pizza.Encrypt;
using Pizza.MVVM.Model;

using Pizza.Repository;
using Pizza.Utilities;

namespace Pizza.Manager
{
	public class DataManager : BaseViewModel
	{
		private static DataManager instance;
		private string _connectionString;

		private ObservableCollection<ProductModelNew> _products;
		private ObservableCollection<ProductModel> _basketProducts;

		private readonly CatalogManager _catalogManager;
		private UnitOfWork _unitOfWork;
		private readonly AuthManager _authManager;

		public DataManager()
		{
			_catalogManager = CatalogManager.Instance;
			_authManager = AuthManager.Instance;
			_connectionString = _authManager.ConnectionString;
			_authManager.PropertyChanged += _authManager_PropertyChanged;

			ChangeConnection();

			_products = new ObservableCollection<ProductModelNew>();
			_basketProducts = new ObservableCollection<ProductModel>();

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

		public void AddInBasket(ProductModel product)
		{
			_basketProducts.Add(product);
			Console.WriteLine("111111111111111111111111111");
			_unitOfWork.Basket.AddInBasket(product.Id);
		}

		public void EditProduct(ProductModel product)
		{
			Console.WriteLine("EDIT");
			Console.WriteLine(product.Date);
			_unitOfWork.Product.UpdateProduct(product);

			ReadData();
		}

		public void DeleteProduct(Guid id)
		{
			_unitOfWork.Product.DeleteProduct(id);

			ReadData();
		}

		public ObservableCollection<ProductModelNew> GetAllProducts()
		{
			return _products;
		}

		public ObservableCollection<ProductModel> GetBasket()
		{
			return _basketProducts;
		}

		public int? GetProductsCount()
		{
			Result<int> result = _unitOfWork.Product.GetProductsCount();

			Console.WriteLine("!!!! 2");

			if (result.IsFailure)
			{
				return null;
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

			_catalogManager.LastPageProductId = _products.Last().Id;
			Console.WriteLine("_catalogManager.NumberSelectionModel.LastProductId: " + _catalogManager.LastPageProductId);

			//_products = products.Value;
			Console.WriteLine("PROD LENGTH: " + _products.Count());
		}

		public void ClearData()
		{
			_products.Clear();
		}

		public async Task ReadDataAsync()
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

			//PageInfoModel.LastProductId = _products.Last().Id;

			//_products = products.Value;
			Console.WriteLine("PROD LENGTH: " + _products.Count());
		}

		private void ReadBasketData()
		{
			var basketProducts = _unitOfWork.Basket.GetBasket();

			if (basketProducts.IsFailure)
			{
				return;
			}
			_basketProducts.Clear();
			foreach (var item in basketProducts.Value)
			{
				_basketProducts.Add(item);
			}
		}
	}
}
