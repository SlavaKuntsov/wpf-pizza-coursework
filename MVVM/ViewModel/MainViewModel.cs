using System.Collections.ObjectModel;
using Pizza.MVVM.Model;

namespace Pizza.MVVM.ViewModel
{
	public class MainViewModel
	{
		private ObservableCollection<ProductModel> Products { get; set; }

		public MainViewModel()
		{
			//Products = new ObservableCollection<ProductModel>();

			//for (int i = 0; i < 5; i++)
			//{
			//	Products.Add(new ProductModel
			//	{
			//		Name = $"Name {i}",
			//		Price = i
			//	});
			//}
		}
	}
}
