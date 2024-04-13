using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Pizza.Utilities;

namespace Pizza.Manager
{
	public class LocalizationManager : BaseViewModel
	{
		private static LocalizationManager instance;
		private ResourceDictionary resourceDictionary;
		private CultureInfo _currentLanguage;

		public LocalizationManager()
		{
			_currentLanguage = new CultureInfo("ru-RU"); 
			UpdateTranslations();

		}

		public static LocalizationManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new LocalizationManager();
				}
				return instance;
			}
		}

		public CultureInfo CurrentLanguage
		{
			get { return _currentLanguage; }
			set
			{
				if (_currentLanguage != value)
				{
					_currentLanguage = value;
					UpdateTranslations();
					OnPropertyChanged(nameof(CurrentLanguage));
				}
			}
		}

		public void SetCultureLanguage(CultureInfo lang)
		{
			CurrentLanguage = lang;
		}
		public CultureInfo GetCultureLanguage()
		{
			return _currentLanguage;
		}

		private void UpdateTranslations()
		{
			string resourcePath = string.Format("../Resources/Resources.{0}.xaml", _currentLanguage.Name.ToLower());
			var newResourceDictionary = new ResourceDictionary
			{
				Source = new Uri(resourcePath, UriKind.RelativeOrAbsolute)
			};
			System.Windows.Application.Current.Resources.MergedDictionaries.Add(newResourceDictionary);
			System.Windows.Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
			resourceDictionary = newResourceDictionary;
		}
	}
}
