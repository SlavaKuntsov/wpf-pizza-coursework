using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;

using Pizza.Abstractions;
using Pizza.Manager;
using Pizza.Utilities;

using static Pizza.Abstractions.ProgramAbstraction;

namespace Pizza.MVVM.ViewModel
{
	public class NavigationViewModel : BaseViewModel
	{
		private readonly ProgramAbstraction programAbstraction;
		private readonly LocalizationManager _localizationManager;
		private readonly CatalogStateManager _catalogStateManager;

		private object _currentView;
		public object CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
		}

		public ICommand HomeCommand { get; set; }
		public ICommand CatalogCommand { get; set; }
		public ICommand SearchCommand { get; set; }

		private void Home(object obj)
		{
			CurrentView = new HomeViewModel();
			ButtonsVisibility = false;
		}

		private void Catalog(object obj)
		{
			CurrentView = new CatalogViewModel();
			ButtonsVisibility = true;
		}

		private void Search(object obj)
		{
			//CurrentView = new SearchViewModel();
			ButtonsVisibility = false;
		}

		public Dictionary<ProgramLanguages, string> ProgramLanguagesDictionary;

		public NavigationViewModel()
		{
			HomeCommand = new RelayCommand(Home);
			CatalogCommand = new RelayCommand(Catalog);
			//SearchCommand = new RelayCommand(Search);

			CurrentView = new HomeViewModel();

			programAbstraction = new ProgramAbstraction();
			ProgramLanguagesDictionary = programAbstraction.ProgramLanguagesDictionary;

			_localizationManager = LocalizationManager.Instance;

			_catalogStateManager = CatalogStateManager.Instance;
			SearchVisibility = _catalogStateManager.SearchVisibility;
			SortVisibility = _catalogStateManager.SortVisibility;
			ButtonsVisibility = _catalogStateManager.ButtonsVisibility;

		}


		public ProgramLanguages _language { get; set; }
		public ProgramLanguages Language
		{
			get { return _language; }
			set
			{
				if (value != _language)
				{
					switch (value.ToString())
					{
						case "Ru":
							_localizationManager.CurrentLanguage = new CultureInfo("ru-RU");
							break;
						case "En":
							_localizationManager.CurrentLanguage = new CultureInfo("en-US");
							break;
					}
					_language = value;
					OnPropertyChanged(nameof(Language));
				}
			}
		}

		private bool _searchVisibility { get; set; }
		public bool SearchVisibility
		{
			get { return _searchVisibility; }
			set
			{
				_searchVisibility = value;
				_catalogStateManager.SearchVisibility = value;
				OnPropertyChanged(nameof(SearchVisibility));
			}
		}

		private bool _sortVisibility { get; set; }
		public bool SortVisibility
		{
			get { return _sortVisibility; }
			set
			{
				_sortVisibility = value;
				_catalogStateManager.SortVisibility = value;
				OnPropertyChanged(nameof(SortVisibility));
			}
		}
		private bool _buttonsVisibility { get; set; }
		public bool ButtonsVisibility
		{
			get { return _buttonsVisibility; }
			set
			{
				_buttonsVisibility = value;
				OnPropertyChanged(nameof(ButtonsVisibility));
			}
		}
	}
}
