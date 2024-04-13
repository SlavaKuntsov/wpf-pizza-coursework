using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Pizza.MVVM.Model;

namespace Pizza.Manager
{
	public class DataManager
	{
		JsonActionsManager json = new JsonActionsManager();

		private static DataManager instance;
		private ObservableCollection<ProductModel> _products;

		public DataManager()
		{
			_products = new ObservableCollection<ProductModel>();

			ReadData();

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

			json.WriteNewProduct(product);
		}

		public void EditProduct(ProductModel product)
		{
			ProductModel find = _products.First(p => p.Id == product.Id);

			int index = _products.IndexOf(find);

			_products[index] = product;

			List<ProductModel> productsList = new List<ProductModel>();

			foreach (var item in _products)
			{
				productsList.Add(item);
			}

			json.WriteAllProducts(productsList);
		}

		public void DeleteProduct(Guid id)
		{
			ProductModel product = _products.First(p => p.Id == id);

			Console.WriteLine("delete prod: " + product.Id);
			_products.Remove(product);

			List<ProductModel> productsList = new List<ProductModel>();

			foreach(var item in _products)
			{
				productsList.Add(item);
			}

			json.WriteAllProducts(productsList);
		}

		public ObservableCollection<ProductModel> GetAllProducts()
		{
			return _products;
		}

		private void ReadData()
		{
			var products = json.GetAllProducts();

			if (products.IsFailure)
			{
				return;
			}
			foreach (var item in products.Value)
			{
				_products.Add(item);
			}
		}
	}
}
