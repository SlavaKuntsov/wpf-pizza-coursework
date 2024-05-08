using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;

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

		private ObservableCollection<ProductModel> _products;
		private ObservableCollection<ProductModel> _basketProducts;

		UnitOfWork _unitOfWork;
		AuthManager _authManager;

		public DataManager()
		{
			_authManager = AuthManager.Instance;
			_connectionString = _authManager.ConnectionString;
			_authManager.PropertyChanged += _authManager_PropertyChanged;

			ChangeConnection();

			_products = new ObservableCollection<ProductModel>();
			_basketProducts = new ObservableCollection<ProductModel>();

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

		public void AddProduct(ProductModel product)
		{
			Console.WriteLine("add prod: " + product.ShortName);
			_products.Add(product);

			_unitOfWork.Product.AddProduct(product);
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

		public ObservableCollection<ProductModel> GetAllProducts()
		{
			return _products;
		}
		public ObservableCollection<ProductModel> GetBasket()
		{
			return _basketProducts;
		}

		private void ReadData()
		{
			var products = _unitOfWork.Product.GetAllProducts();

			if (products.IsFailure)
			{
				return;
			}
			_products.Clear();
			foreach (var item in products.Value)
			{
				_products.Add(item);
			}
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
