using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.MVVM.ViewModel;

using static Pizza.Abstractions.ProductAbstraction;

namespace Pizza.MVVM.View
{
	/// <summary>
	/// </summary>
	public partial class BasketProductCardView : UserControl
	{
		CatalogManager _catalogManager;
		public BasketProductCardView()
		{
			InitializeComponent();

			_catalogManager = CatalogManager.Instance;

		}

		private void OnShowModalClick(object sender, MouseButtonEventArgs e)
		{
			ListViewItem clickedItem = (ListViewItem)sender;

			ProductModelNew selectedItem = (ProductModelNew)clickedItem.DataContext;

			
			foreach (var item in selectedItem.PropertyValue)
			{
				Console.WriteLine("$$$$$$$ value: " + item);
			}

			Console.WriteLine("ShowBasketModal CLICK ---------------------");
			_catalogManager.ModalBasketProductId = selectedItem.Id;
			Console.WriteLine(" selectedItem.Id: " + selectedItem.Id);

			//Guid id = selectedItem.Id;
			//string shortName = selectedItem.ShortName;
			//string fullName = selectedItem.FullName;
			//string description = selectedItem.Description;
			//double price = selectedItem.Price;
			//string imageName = selectedItem.Image;
			//PizzaCategories category = selectedItem.ProductCategory;
			//PizzaSizes size = selectedItem.ProductSize;
			//Rating rating = selectedItem.Rating;
			//int count = selectedItem.Count;


			//var selectedProduct = ProductModel.Create(id, shortName, fullName, description, price, imageName, category, size, rating, count);

			//if (selectedProduct.IsFailure)
			//{
			//	MessageBox.Show(selectedProduct.Error);
			//}

			//CatalogGrid.Children.Add(new ProductModalView() { DataContext = new ProductModalViewModel(selectedItem) });
		}

		// перехват прокрутки колесиком мышки
		private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
				{
					RoutedEvent = UIElement.MouseWheelEvent,
					Source = sender
				};
				var parent = ((Control)sender).Parent as UIElement;
				parent?.RaiseEvent(eventArg);
			}
		}
	}
}