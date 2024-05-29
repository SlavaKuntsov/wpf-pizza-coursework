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
	/// Логика взаимодействия для OrdersView.xaml
	/// </summary>
	public partial class OrdersView : UserControl
	{
		DataManager _dataManager;
		public OrdersView()
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
		}

		private void Button_Click2(object sender, RoutedEventArgs e)
		{
			Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!");

			Button clickedButton = sender as Button;
			OrderModel selectedItem = clickedButton?.DataContext as OrderModel;

			if (selectedItem != null)
			{
				Console.WriteLine("CancelOrder CLICK 2 ---------------------");
				Console.WriteLine("selectedItem.Id 2: " + selectedItem.Id);

				// Вызов метода отмены заказа с использованием Order_id
				_dataManager.AcceptOrder(selectedItem.Id);
			}
		}

		private void Button_Click3(object sender, RoutedEventArgs e)
		{
			Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!");

			Button clickedButton = sender as Button;
			OrderModel selectedItem = clickedButton?.DataContext as OrderModel;

			if (selectedItem != null)
			{
				Console.WriteLine("CancelOrder CLICK 3 ---------------------");
				Console.WriteLine("selectedItem.Id 3: " + selectedItem.Id);

				// Вызов метода отмены заказа с использованием Order_id
				_dataManager.CompleteOrder(selectedItem.Id);
			}
		}

		private void Button_Click4(object sender, RoutedEventArgs e)
		{
			Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!");

			Button clickedButton = sender as Button;
			OrderModel selectedItem = clickedButton?.DataContext as OrderModel;

			if (selectedItem != null)
			{
				Console.WriteLine("CancelOrder CLICK 3 ---------------------");
				Console.WriteLine("selectedItem.Id 3: " + selectedItem.Id);

				// Вызов метода отмены заказа с использованием Order_id
				_dataManager.DeliverOrder(selectedItem.Id);
			}
		}

		private void Button_Click5(object sender, RoutedEventArgs e)
		{
			Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!");

			Button clickedButton = sender as Button;
			OrderModel selectedItem = clickedButton?.DataContext as OrderModel;

			if (selectedItem != null)
			{
				Console.WriteLine("CancelOrder CLICK 3 ---------------------");
				Console.WriteLine("selectedItem.Id 3: " + selectedItem.Id);

				// Вызов метода отмены заказа с использованием Order_id
				_dataManager.CompleteDeliverOrder(selectedItem.Id);
			}
		}
	}
}
