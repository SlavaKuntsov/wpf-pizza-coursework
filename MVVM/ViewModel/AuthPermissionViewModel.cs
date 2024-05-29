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
	public class AuthPermissionViewModel : BaseViewModel
	{
		DataManager _dataManager;
		AuthManager _authManager;
		public AuthPermissionViewModel()
		{
			_dataManager = DataManager.Instance;
			_authManager = AuthManager.Instance;

			_dataManager.ReadAllUnauthorizedEmployeesData();
			Users = _dataManager.GetEmployees();
			Users.CollectionChanged += Users_CollectionChanged;

			//AddReviewCommand = new RelayCommand(AddReview);

			//switch (_authManager.User.Role)
			//{
			//	case Abstractions.ProgramAbstraction.AppRoles.Customer:
			//		ManagerVisibility = true;
			//		break;
			//	case Abstractions.ProgramAbstraction.AppRoles.Manager:
			//		ManagerVisibility = false;
			//		break;
			//}

			//Console.WriteLine("MANAGER VIS: " + ManagerVisibility);
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
