using Pizza.MVVM.Model;
using System.Collections.Generic;

using Pizza.Utilities;

namespace Pizza.Manager
{
	public class CatalogManager : BaseViewModel
	{
		private static CatalogManager instance;
		AuthManager _authManager;
		public CatalogManager()
		{
			//NumberSelectionModel = new PageInfoModel();
			ModalProductId = 0;
			ModalBasketProductId = 0;
			//LastPageProductId = null;
			//ProductOnPage = 501;
			ProductOnPage = 252;
			CurrentPage = 1;

			_searchText = "";
			IsFullSearch = false;

			SearchVisibility = false;
			SortVisibility = false;

			_authManager = AuthManager.Instance;

			System.Console.WriteLine("after auth manager");

			switch (_authManager.User.Role)
			{
				case Abstractions.ProgramAbstraction.AppRoles.Customer:
					ButtonsVisibility = true;
					break;
				case Abstractions.ProgramAbstraction.AppRoles.Manager:
					ButtonsVisibility = false;
					break;
				case Abstractions.ProgramAbstraction.AppRoles.Seller:
					ButtonsVisibility = false;
					break;
				case Abstractions.ProgramAbstraction.AppRoles.Courier:
					ButtonsVisibility = false;
					break;
			}

		}

		public static CatalogManager Instance
		{
			get
			{
				if (instance == null)
				{
					System.Console.WriteLine("NEW CATALOG MANAGER");
					instance = new CatalogManager();
				}
				return instance;
			}
		}
		
		private int _modalProductId { get; set; }
		public int ModalProductId
		{
			get { return _modalProductId; }
			set { _modalProductId = value; OnPropertyChanged(nameof(ModalProductId)); }
		}
		
		private int _modalBasketProductId { get; set; }
		public int ModalBasketProductId
		{
			get { return _modalBasketProductId; }
			set { _modalBasketProductId = value; OnPropertyChanged(nameof(ModalBasketProductId)); }
		}

		//private int? _lastPageProductId { get; set; }
		//public int? LastPageProductId	
		//{
		//	get { return _lastPageProductId; }
		//	set { _lastPageProductId = value; OnPropertyChanged(nameof(LastPageProductId)); }
		//}

		private int _productOnPage { get; set; }
		public int ProductOnPage
		{
			get { return _productOnPage; }
			set { _productOnPage = value; OnPropertyChanged(nameof(ProductOnPage)); }
		}

		private int _currentPage { get; set; }
		public int CurrentPage
		{
			get { return _currentPage; }
			set { _currentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
		}

		private string _searchText { get; set; }
		public string SearchText
		{
			get { return _searchText; }
			set { _searchText = value; OnPropertyChanged(nameof(SearchText)); }
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
		private bool _buttonsVisibility { get; set; }
		public bool ButtonsVisibility
		{
			get { return _buttonsVisibility; }
			set { _buttonsVisibility = value; OnPropertyChanged(nameof(ButtonsVisibility)); }
		}

		//private double _priceSum;
		//public double PriceSum
		//{
		//	get { return _priceSum; }
		//	set { _priceSum = value; OnPropertyChanged(nameof(PriceSum)); }
		//}
	}
}
