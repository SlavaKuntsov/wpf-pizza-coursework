using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.Utilities;

using static Pizza.Abstractions.ProgramAbstraction;


namespace Pizza.MVVM.ViewModel
{
	public class CatalogViewModel : BaseViewModel
	{
		private readonly PageModel _pageModel;
		private readonly DataManager _dataManager;
		private readonly CatalogStateManager _catalogStateManager;

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

			Products = _dataManager.GetAllProducts();
			CopyProducts = _dataManager.GetAllProducts();
			Products.CollectionChanged += _products_CollectionChanged;

			//ProductsView = CollectionViewSource.GetDefaultView(Products);
			//CollectionSortBy(Abstractions.ProgramAbstraction.SortByProperty.Date, ListSortDirection.Ascending);


			SortByDateAscCommand = new RelayCommand(SortByDateAsc);
			SortByDateDescCommand = new RelayCommand(SortByDateDesc);
			SortByNameAscCommand = new RelayCommand(SortByNameAsc);
			SortByNameDescCommand = new RelayCommand(SortByNameDesc);
			SortByPriceAscCommand = new RelayCommand(SortByPriceAsc);
			SortByPriceDescCommand = new RelayCommand(SortByPriceDesc);

			SortByProperty = SortByPropertyAll.DateAsc;

			_catalogStateManager = CatalogStateManager.Instance;
			SearchText = _catalogStateManager.SearchText;
			IsFullSearch = _catalogStateManager.IsFullSearch;
			SearchVisibility = _catalogStateManager.SearchVisibility;
			SortVisibility = _catalogStateManager.SortVisibility;

			_catalogStateManager.PropertyChanged += _catalogStateManager_PropertyChanged;
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

		public ObservableCollection<ProductModel> Products
		{
			get { return _pageModel.Products; }
			set { _pageModel.Products = value; OnPropertyChanged(nameof(Products)); }
		}

		public ObservableCollection<ProductModel> _copyProducts { get; set; }
		public ObservableCollection<ProductModel> CopyProducts
		{
			get { return _copyProducts; }
			set { _copyProducts = value; OnPropertyChanged(nameof(CopyProducts)); }
		}

		//public ICollectionView _productsView { get; set; }
		//public ICollectionView ProductsView
		//{
		//	get { return _productsView; }
		//	set { _productsView = value; OnPropertyChanged(nameof(ProductsView)); }
		//}

		private void CollectionFind(string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				ObservableCollection<ProductModel> filteredProducts;

				if (IsFullSearch)
				{
					filteredProducts = new ObservableCollection<ProductModel>(
						CopyProducts.Where(item => item.ShortName.Equals(value, StringComparison.OrdinalIgnoreCase)));
				}
				else
				{
					filteredProducts = new ObservableCollection<ProductModel>(
						CopyProducts.Where(item => item.ShortName.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0));
				}

				Products = filteredProducts;
			}
			else
			{
				Products = CopyProducts;
			}
		}

		private void CollectionSortBy(SortByProperty property, ListSortDirection direction)
		{
			if (direction == ListSortDirection.Ascending)
			{
				switch (property)
				{
					case Abstractions.ProgramAbstraction.SortByProperty.Date:
						Products = new ObservableCollection<ProductModel>(Products.OrderBy(p => p.Date));
						break;
					case Abstractions.ProgramAbstraction.SortByProperty.Name:
						Products = new ObservableCollection<ProductModel>(Products.OrderBy(p => p.ShortName));
						break;
					case Abstractions.ProgramAbstraction.SortByProperty.Price:
						Products = new ObservableCollection<ProductModel>(Products.OrderBy(p => p.Price));
						break;
					default:
						break;
				}
			}
			if (direction == ListSortDirection.Descending)
			{
				switch (property)
				{
					case Abstractions.ProgramAbstraction.SortByProperty.Date:
						Products = new ObservableCollection<ProductModel>(Products.OrderByDescending(p => p.Date));
						break;
					case Abstractions.ProgramAbstraction.SortByProperty.Name:
						Products = new ObservableCollection<ProductModel>(Products.OrderByDescending(p => p.ShortName));
						break;
					case Abstractions.ProgramAbstraction.SortByProperty.Price:
						Products = new ObservableCollection<ProductModel>(Products.OrderByDescending(p => p.Price));
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

		private string _searchText { get; set; }
		public string SearchText
		{
			get { return _searchText; }
			set
			{
				_searchText = value;
				_catalogStateManager.SearchText = value;
				CollectionFind(value);
				OnPropertyChanged(nameof(SearchText));
			}
		}

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
				SearchVisibility = _catalogStateManager.SearchVisibility;
			}
			else if (e.PropertyName == "SortVisibility")
			{
				SortVisibility = _catalogStateManager.SortVisibility;
			}
			//else if (e.PropertyName == "SearchText")
			//{
			//	SearchText = _catalogStateManager.SearchText;
			//	CollectionFind(SearchText);
			//}
			else if (e.PropertyName == "IsFullSearch")
			{
				IsFullSearch = _catalogStateManager.IsFullSearch;

				CollectionFind(SearchText);
			}
		}
	}
}