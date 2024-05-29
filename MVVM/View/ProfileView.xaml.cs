using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Pizza.Manager;
using Pizza.MVVM.Model;

namespace Pizza.MVVM.View
{
	/// <summary>
	/// Логика взаимодействия для ProfileView.xaml
	/// </summary>
	public partial class ProfileView : UserControl
	{
		DataManager _dataManager;
		public ProfileView()
		{
			InitializeComponent();

			_dataManager = DataManager.Instance;
		}

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

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!");

			Button clickedButton = sender as Button;
			OrderModel selectedItem = clickedButton?.DataContext as OrderModel;

			if (selectedItem != null)
			{
				Console.WriteLine("CancelOrder CLICK ---------------------");
				Console.WriteLine("selectedItem.Id: " + selectedItem.Id);

				// Вызов метода отмены заказа с использованием Order_id
				_dataManager.CancelOrder(selectedItem.Id);
			}

			//_dataManager.CancelOrder();

			//ListViewItem clickedItem = (ListViewItem)sender;

			//OrderModel selectedItem = (OrderModel)clickedItem.DataContext;

			//Console.WriteLine("ShowOrderModal CLICK ---------------------");
			////_catalogManager.ModalProductId = selectedItem.Id;
			//Console.WriteLine(" selectedItem.Id: " + selectedItem.Id);
		}
    }
}
