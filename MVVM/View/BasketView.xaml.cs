using System;
using System.Windows.Controls;

using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.MVVM.ViewModel;

namespace Pizza.MVVM.View
{
	/// <summary>
	/// Логика взаимодействия для BasketView.xaml
	/// </summary>
	public partial class BasketView : UserControl
	{
		DataManager _dataManager;
		CatalogManager _catalogManager;
		public BasketView()
		{
			InitializeComponent();

			_dataManager = DataManager.Instance;
			_catalogManager = CatalogManager.Instance;
			_catalogManager.PropertyChanged += _catalogManager_PropertyChanged;
		}
		public void ShowBasketModal(int selectedItemId)
		{
			Console.WriteLine("--------------ShowBasketModal------------");
			if (selectedItemId != 0)
			{
				ProductModelNew selectedItem = _dataManager.ReadBasketInfoData(selectedItemId);
				BasketGrid.Children.Add(new ProductModalView() { DataContext = new ProductModalViewModel(selectedItem, false) });

				foreach (var item in selectedItem.PropertyName)
				{
					Console.WriteLine("Name: " + item);
				}
				foreach (var item in selectedItem.PropertyValue)
				{
					Console.WriteLine("Value: " + item);
				}

				Console.WriteLine("basket select size: " + selectedItem.Size);
				Console.WriteLine("basket select type: " + selectedItem.Type);

				Console.WriteLine("ProductCategory: " + selectedItem.Category);
			}
		}

		private void _catalogManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "ModalBasketProductId")
			{
				ShowBasketModal(_catalogManager.ModalBasketProductId);
			}
		}
	}
}
