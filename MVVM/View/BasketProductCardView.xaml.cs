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
	public partial class BasketProductCardView : UserControl
	{
		public BasketProductCardView()
		{
			InitializeComponent();

		}

		private void OnShowModalClick(object sender, MouseButtonEventArgs e)
		{
			ListViewItem clickedItem = (ListViewItem)sender;

			ProductModel selectedItem = (ProductModel)clickedItem.DataContext;

			Guid id = selectedItem.Id;

			string shortName = selectedItem.ShortName;
			string fullName = selectedItem.FullName;
			string description = selectedItem.Description;
			double price = selectedItem.Price;
			string imageName = selectedItem.Image;
			PizzaCategories category = selectedItem.Category;
			PizzaSizes size = selectedItem.Size;
			Rating rating = selectedItem.Rating;
			int count = selectedItem.Count;


			var selectedProduct = ProductModel.Create(id, shortName, fullName, description, price, imageName, category, size, rating, count);

			if (selectedProduct.IsFailure)
			{
				MessageBox.Show(selectedProduct.Error);
			}

			CatalogGrid.Children.Add(new ProductModalView() { DataContext = new ProductModalViewModel(selectedProduct.Value) });
		}
	}
}