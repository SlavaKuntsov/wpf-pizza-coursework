using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Pizza.MVVM.Model;
using Pizza.MVVM.ViewModel;

using static Pizza.Abstractions.ProductAbstraction;

namespace Pizza.MVVM.View
{
	/// <summary>
	/// </summary>
	public partial class ProductCardView : UserControl
	{
		public ProductCardView()
		{
			InitializeComponent();

		}

		private void OnShowModalClick(object sender, MouseButtonEventArgs e)
		{
			ListViewItem clickedItem = (ListViewItem)sender;



			ProductModelNew selectedItem = (ProductModelNew)clickedItem.DataContext;

			//int id = selectedItem.Id;
			//string name = selectedItem.Name;
			//string fullName = selectedItem.FullName;
			//string description = selectedItem.Description;
			//double price = selectedItem.Price;
			//string imageName = selectedItem.Image;
			//PizzaCategories category = selectedItem.Category;
			//PizzaSizes size = selectedItem.Size;
			//Rating rating = selectedItem.Rating;
			//int count = selectedItem.Count;


			//var selectedProduct = ProductModel.Create(id, shortName, fullName, description, price, imageName, category, size, rating, count);

			//if (selectedProduct.IsFailure)
			//{
			//	MessageBox.Show(selectedProduct.Error);
			//}

			CatalogGrid.Children.Add(new ProductModalView() { DataContext = new ProductModalViewModel(selectedItem) });
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