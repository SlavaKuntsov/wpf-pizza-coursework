using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pizza.Manager;
using Pizza.MVVM.Model;

namespace Pizza.MVVM.ViewModel
{
	public class ProfileViewModel : BasketViewModel
	{
		AuthManager _authManager;
		public ProfileViewModel()
		{
			_authManager = AuthManager.Instance;

			// переделать на модель UserModel вместо множества полей
			User = _authManager.User;
		}

		private UserModel _user { get; set; }

		public UserModel User
		{
			get { return _user; }
			set { _user = value; OnPropertyChanged(nameof(User)); }
		}
	}
}
