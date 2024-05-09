using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.Utilities;

namespace Pizza.MVVM.ViewModel
{
 	public  class BasketViewModel : BaseViewModel
	{
		private readonly DataManager _dataManager;

		public BasketViewModel()
		{
			_dataManager = DataManager.Instance;

			Products = _dataManager.GetBasket();
		}

		public ObservableCollection<ProductModel> _products;
		public ObservableCollection<ProductModel> Products
		{
			get { return _products; }
			set { _products = value; OnPropertyChanged(nameof(Products)); }
		}
	}
}
