using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.Utilities;

namespace Pizza.MVVM.ViewModel
{
	public class EmployeesViewModel : BaseViewModel
	{
		DataManager _dataManager;
		AuthManager _authManager;
		public EmployeesViewModel()
		{

			_dataManager = DataManager.Instance;
			_authManager = AuthManager.Instance;

			_dataManager.ReadAllAuthorizedEmployeesData();
			Users = _dataManager.GetEmployees();
			Users.CollectionChanged += Users_CollectionChanged;
		}

		private void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Console.WriteLine("Products_CollectionChanged @@@@@@@@@@@@@@@@@@@@@");
			Users = _dataManager.GetEmployees();
		}

		private ObservableCollection<AuthPermissionModel> _users { get; set; }

		public ObservableCollection<AuthPermissionModel> Users
		{
			get { return _users; }
			set { _users = value; OnPropertyChanged(nameof(Users)); }
		}
	}
}
