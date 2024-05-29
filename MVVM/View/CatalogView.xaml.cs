using System;
using System.Windows.Controls;

using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.MVVM.ViewModel;

namespace Pizza.MVVM.View
{
	/// <summary>
	/// Логика взаимодействия для CatalogView.xaml
	/// </summary>
	public partial class CatalogView : UserControl
	{
		DataManager _dataManager;
		CatalogManager _catalogManager;
		public CatalogView()
		{
			InitializeComponent();

			_dataManager = DataManager.Instance;
			_catalogManager = CatalogManager.Instance;
			_catalogManager.PropertyChanged += _catalogManager_PropertyChanged;

			//ShowModal(_catalogManager.ModalProductId);
		}

		public void ShowModal(int selectedItemId)
		{
			Console.WriteLine("--------------ShowModal------------");
			if (selectedItemId != 0)
			{
				ProductModelNew selectedItem = _dataManager.ReadProductInfoData(selectedItemId);
				CatalogGrid.Children.Add(new ProductModalView() { DataContext = new ProductModalViewModel(selectedItem, true) });

				foreach (var item in selectedItem.PropertyName)
				{
					Console.WriteLine("Name: " + item);
				}foreach (var item in selectedItem.PropertyValue)
				{
					Console.WriteLine("Value: " + item);
				}

				Console.WriteLine("ProductCategory: " + selectedItem.Category);
			}
		}

		private void _catalogManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "ModalProductId")
			{
				ShowModal(_catalogManager.ModalProductId);
			}
		}
	}
}
