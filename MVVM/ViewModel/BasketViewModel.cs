using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.Utilities;

using static Pizza.Abstractions.ProductAbstraction;

namespace Pizza.MVVM.ViewModel
{
	public class BasketViewModel : BaseViewModel
	{
		private readonly DataManager _dataManager;
		private readonly CatalogManager _catalogManager;

		public ICommand OrderCommand { get; set; }

		public BasketViewModel()
		{
			_dataManager = DataManager.Instance;
			_catalogManager = CatalogManager.Instance;

			_dataManager.ReadBasketData();
			Products = _dataManager.GetBasket();
			Products.CollectionChanged += Products_CollectionChanged;

			ProductsCount = 0;
			IsDelivery = PizzaIsDelivery.No;

			OrderCommand = new RelayCommand(Order);
		}

		private void Calculate()
		{
			PriceSum = 0;
			ProductsCount = 0;

			foreach (var product in Products)
			{
				Console.WriteLine("product.Price: " + product.Price);
				Console.WriteLine("product.Count: " + product.Count);
				PriceSum += product.Price * product.Count;
				ProductsCount += product.Count;
			}

			//_catalogManager.PriceSum = PriceSum;
		}

		private void Products_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Console.WriteLine("Products_CollectionChanged @@@@@@@@@@@@@@@@@@@@@");
			Products = _dataManager.GetBasket();

			Calculate();
		}

		private async void Order(object obj)
		{
			Console.WriteLine("_______ ORDER");
			switch (IsDelivery)
			{
				case PizzaIsDelivery.Yes:
					await _dataManager.AddOrder(true);
					break;
				case PizzaIsDelivery.No:
					await _dataManager.AddOrder(false);
					break;
			}
		}

		public ObservableCollection<ProductModelNew> _products;
		public ObservableCollection<ProductModelNew> Products
		{
			get { return _products; }
			set { _products = value; OnPropertyChanged(nameof(Products)); }
		}

		private double _priceSum;
		public double PriceSum
		{
			get { return _priceSum; }
			set { _priceSum = value; OnPropertyChanged(nameof(PriceSum)); }
		}

		private double _productsCount;
		public double ProductsCount
		{
			get { return _productsCount; }
			set { _productsCount = value; OnPropertyChanged(nameof(ProductsCount)); }
		}

		private PizzaIsDelivery _isDelivery;
		public PizzaIsDelivery IsDelivery
		{
			get { return _isDelivery; }
			set { _isDelivery = value; OnPropertyChanged(nameof(IsDelivery)); }
		}
	}
}
