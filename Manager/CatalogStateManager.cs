using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Pizza.MVVM.Model;
using Pizza.Utilities;

namespace Pizza.Manager
{
	public class CatalogStateManager : BaseViewModel
	{
		private static CatalogStateManager instance;
		public CatalogStateManager()
		{
			_searchText = "";
			IsFullSearch = false;

			SearchVisibility = false;
			SortVisibility = false;

			ButtonsVisibility = false;
		}

		public static CatalogStateManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new CatalogStateManager();
				}
				return instance;
			}
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
