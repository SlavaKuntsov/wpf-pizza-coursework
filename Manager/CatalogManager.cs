using Pizza.MVVM.Model;
using System.Collections.Generic;

using Pizza.Utilities;

namespace Pizza.Manager
{
	public class CatalogManager : BaseViewModel
	{
		private static CatalogManager instance;
		public CatalogManager()
		{
			//NumberSelectionModel = new PageInfoModel();
			LastPageProductId = null;
			CurrentPage = 1;

			_searchText = "";
			IsFullSearch = false;

			SearchVisibility = false;
			SortVisibility = false;
			//NumberSelectionModel.LastProductId = null;

			AuthManager authManager = AuthManager.Instance;

			switch (authManager.User.Role)
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
					instance = new CatalogManager();
				}
				return instance;
			}
		}

		private int? _lastPageProductId { get; set; }
		public int? LastPageProductId	
		{
			get { return _lastPageProductId; }
			set { _lastPageProductId = value; OnPropertyChanged(nameof(LastPageProductId)); }
		}

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
	}
}
