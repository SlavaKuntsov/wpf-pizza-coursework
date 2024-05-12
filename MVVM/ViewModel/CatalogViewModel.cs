using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.Utilities;

using static Pizza.Abstractions.ProgramAbstraction;


namespace Pizza.MVVM.ViewModel
{
	public class CatalogViewModel : BaseViewModel
	{
		private PageModel _pageModel;
		private DataManager _dataManager;
		private CatalogManager _catalogManager;

		public ICommand SortByDateAscCommand { get; set; }
		public ICommand SortByDateDescCommand { get; set; }
		public ICommand SortByNameAscCommand { get; set; }
		public ICommand SortByNameDescCommand { get; set; }
		public ICommand SortByPriceAscCommand { get; set; }
		public ICommand SortByPriceDescCommand { get; set; }

		public CatalogViewModel()
		{
			_pageModel = new PageModel();
			_dataManager = DataManager.Instance;

			_pageModel = new PageModel();
			_dataManager = DataManager.Instance;
			_catalogManager = CatalogManager.Instance;

			//получение именно продуктов
			int count = _dataManager.GetAllProducts().Count;

			if (count < 1)
			{
				_dataManager.ReadData();
			}
			Products = _dataManager.GetAllProducts();
			//CopyProducts = _dataManager.GetAllProducts();
			Products.CollectionChanged += _products_CollectionChanged;

			_catalogManager.ProductOnPage = 501;
			NumberSelection = new ObservableCollection<NumberSelectionModel>();

			int? totalProducts = _dataManager.GetProductsCount();
			Console.WriteLine("totalProducts: " + totalProducts);
			Console.WriteLine("_catalogManager.ProductOnPage: " + _catalogManager.ProductOnPage);
			int totalPages = (int)Math.Ceiling((double)totalProducts / _catalogManager.ProductOnPage);

			CurrentPage = _catalogManager.CurrentPage;

			for (int i = 1; i <= totalPages; i++)
			{
				NumberSelectionModel model = new NumberSelectionModel
				{
					Number = i,
					Command = new RelayCommand(HandleNumberSelection),
					// Привязываем SelectionCommand к Command
					//SelectionCommand = model.Command
				};
				NumberSelection.Add(model);
			}

			// Установка первоначального выделения
			if (NumberSelection.Count > 0)
			{
				NumberSelection[0].IsSelected = true;
			}

			Console.WriteLine("CurrentPage: " + CurrentPage);

			//ProductsView = CollectionViewSource.GetDefaultView(Products);
			//CollectionSortBy(Abstractions.ProgramAbstraction.SortByProperty.Date, ListSortDirection.Ascending);

			SortByDateAscCommand = new RelayCommand(SortByDateAsc);
			SortByDateDescCommand = new RelayCommand(SortByDateDesc);
			SortByNameAscCommand = new RelayCommand(SortByNameAsc);
			SortByNameDescCommand = new RelayCommand(SortByNameDesc);
			SortByPriceAscCommand = new RelayCommand(SortByPriceAsc);
			SortByPriceDescCommand = new RelayCommand(SortByPriceDesc);

			SortByProperty = SortByPropertyAll.DateAsc;

			//SearchText = _catalogManager.SearchText;
			IsFullSearch = _catalogManager.IsFullSearch;
			SearchVisibility = _catalogManager.SearchVisibility;
			SortVisibility = _catalogManager.SortVisibility;

			_catalogManager.PropertyChanged += _catalogStateManager_PropertyChanged;
		}

		private void HandleNumberSelection(object obj)
		{
			if (obj is int number)
			{
				// Сохраняем предыдущее состояние CurrentPage
				int previousPage = _catalogManager.CurrentPage;

				foreach (var item in NumberSelection)
				{
					item.IsSelected = (item.Number == number);
				}

				_catalogManager.CurrentPage = number;

				// изменить логику получения нужой пагинации
				if (number != previousPage)
				{
					_dataManager.ClearData();

					Console.WriteLine(_catalogManager.LastPageProductId);

					_dataManager.ReadData();

					Products = _dataManager.GetAllProducts();
				}
			}
		}


		public SortByPropertyAll _sortByProperty { get; set; }
		public SortByPropertyAll SortByProperty
		{
			get { return _sortByProperty; }
			set { _sortByProperty = value; OnPropertyChanged(nameof(SortByProperty)); }
		}

		private void SortByDateAsc(object obj)
		{
			CollectionSortBy(Abstractions.ProgramAbstraction.SortByProperty.Date, ListSortDirection.Ascending);
		}
		private void SortByDateDesc(object obj)
		{
			CollectionSortBy(Abstractions.ProgramAbstraction.SortByProperty.Date, ListSortDirection.Descending);
		}
		private void SortByNameAsc(object obj)
		{
			CollectionSortBy(Abstractions.ProgramAbstraction.SortByProperty.Name, ListSortDirection.Ascending);
		}
		private void SortByNameDesc(object obj)
		{
			CollectionSortBy(Abstractions.ProgramAbstraction.SortByProperty.Name, ListSortDirection.Descending);
		}
		private void SortByPriceAsc(object obj)
		{
			CollectionSortBy(Abstractions.ProgramAbstraction.SortByProperty.Price, ListSortDirection.Descending);
		}
		private void SortByPriceDesc(object obj)
		{
			CollectionSortBy(Abstractions.ProgramAbstraction.SortByProperty.Price, ListSortDirection.Ascending);
		}

		public ObservableCollection<ProductModelNew> _products;
		public ObservableCollection<ProductModelNew> Products
		{
			get { return _products; }
			set { _products = value; OnPropertyChanged(nameof(Products)); }
		}

		public ObservableCollection<NumberSelectionModel> _numberSelection;
		public ObservableCollection<NumberSelectionModel> NumberSelection
		{
			get { return _numberSelection; }
			set { _numberSelection = value; OnPropertyChanged(nameof(NumberSelection)); }
		}
		private int _currentPage { get; set; }
		public int CurrentPage
		{
			get { return _currentPage; }
			set { _currentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
		}

		//public ObservableCollection<ProductPreviewModel> _copyProducts { get; set; }
		//public ObservableCollection<ProductPreviewModel> CopyProducts
		//{
		//	get { return _copyProducts; }
		//	set { _copyProducts = value; OnPropertyChanged(nameof(CopyProducts)); }
		//}

		//public ICollectionView _productsView { get; set; }
		//public ICollectionView ProductsView
		//{
		//	get { return _productsView; }
		//	set { _productsView = value; OnPropertyChanged(nameof(ProductsView)); }
		//}

		//private void CollectionFind(string value)
		//{
		//	if (!string.IsNullOrWhiteSpace(value))
		//	{
		//		ObservableCollection<ProductPreviewModel> filteredProducts;

		//		if (IsFullSearch)
		//		{
		//			filteredProducts = new ObservableCollection<ProductPreviewModel>(
		//				CopyProducts.Where(item => item.Name.Equals(value, StringComparison.OrdinalIgnoreCase)));
		//		}
		//		else
		//		{
		//			filteredProducts = new ObservableCollection<ProductPreviewModel>(
		//				CopyProducts.Where(item => item.Name.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0));
		//		}

		//		Products = filteredProducts;
		//	}
		//	else
		//	{
		//		Products = CopyProducts;
		//	}
		//}

		private void CollectionSortBy(SortByProperty property, ListSortDirection direction)
		{
			if (direction == ListSortDirection.Ascending)
			{
				switch (property)
				{
					//case Abstractions.ProgramAbstraction.SortByProperty.Date:
					//	Products = new ObservableCollection<ProductModel>(Products.OrderBy(p => p.Date));
					//	break;
					case Abstractions.ProgramAbstraction.SortByProperty.Name:
						Products = new ObservableCollection<ProductModelNew>(Products.OrderBy(p => p.Name));
						break;
					case Abstractions.ProgramAbstraction.SortByProperty.Price:
						Products = new ObservableCollection<ProductModelNew>(Products.OrderBy(p => p.Price));
						break;
					default:
						break;
				}
			}
			if (direction == ListSortDirection.Descending)
			{
				switch (property)
				{
					//case Abstractions.ProgramAbstraction.SortByProperty.Date:
					//	Products = new ObservableCollection<ProductPreviewModel>(Products.OrderByDescending(p => p.Date));
					//	break;
					case Abstractions.ProgramAbstraction.SortByProperty.Name:
						Products = new ObservableCollection<ProductModelNew>(Products.OrderByDescending(p => p.Name));
						break;
					case Abstractions.ProgramAbstraction.SortByProperty.Price:
						Products = new ObservableCollection<ProductModelNew>(Products.OrderByDescending(p => p.Price));
						break;
					default:
						break;
				}
			}

		}

		private void _products_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Products = _dataManager.GetAllProducts();
		}

		//private string _searchText { get; set; }
		//public string SearchText
		//{
		//	get { return _searchText; }
		//	set
		//	{
		//		_searchText = value;
		//		_catalogManager.SearchText = value;
		//		CollectionFind(value);
		//		OnPropertyChanged(nameof(SearchText));
		//	}
		//}

		private bool _isFullSearch { get; set; }
		public bool IsFullSearch
		{
			get { return _isFullSearch; }
			set { _isFullSearch = value; OnPropertyChanged(nameof(IsFullSearch)); }
		}


		private bool _searchVisibility { get; set; }
		public bool SearchVisibility
		{
			get { return _searchVisibility; }
			set { _searchVisibility = value; OnPropertyChanged(nameof(SearchVisibility)); }
		}

		private bool _sortVisibility { get; set; }
		public bool SortVisibility
		{
			get { return _sortVisibility; }
			set { _sortVisibility = value; OnPropertyChanged(nameof(SortVisibility)); }
		}

		private void _catalogStateManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "SearchVisibility")
			{
				SearchVisibility = _catalogManager.SearchVisibility;
			}
			else if (e.PropertyName == "SortVisibility")
			{
				SortVisibility = _catalogManager.SortVisibility;
			}
			//else if (e.PropertyName == "SearchText")
			//{
			//	SearchText = _catalogManager.SearchText;
			//	CollectionFind(SearchText);
			//}
			else if (e.PropertyName == "IsFullSearch")
			{
				IsFullSearch = _catalogManager.IsFullSearch;

				//CollectionFind(SearchText);
			}
			//else if (e.PropertyName == "LastPageProductId")
			//{
			//	int? totalProducts = _dataManager.GetProductsCount();
			//	int totalPages = (int)Math.Ceiling((double)totalProducts / _catalogManager.ProductOnPage);

			//	for (int i = 1; i <= totalPages; i++)
			//	{
			//		NumberSelectionModel model = new NumberSelectionModel
			//		{
			//			Number = i,
			//			Command = new RelayCommand(HandleNumberSelection)
			//		};
			//		NumberSelection.Add(model);
			//	}
			//}
		}
	}
}