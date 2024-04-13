using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Pizza.Abstractions;
using Pizza.MVVM.ViewModel;

using static Pizza.Abstractions.ProductAbstraction;

namespace Pizza.MVVM.View
{
	public partial class HomeView : UserControl
	{

		public HomeView()
		{
			InitializeComponent();
		}
	}
}