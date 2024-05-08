using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Pizza.Encrypt;
using Pizza.Utilities;

namespace Pizza.MVVM.ViewModel.Auth
{
	public class LogInViewModel : BaseViewModel
	{
		public LogInViewModel()
		{

		}

		private bool _name { get; set; }
		public bool Name
		{
			get { return _name; }
			set { _name = value; OnPropertyChanged(nameof(Name)); }
		}

		private bool _surname { get; set; }
		public bool Surname
		{
			get { return _surname; }
			set { _surname = value; OnPropertyChanged(nameof(Surname)); }
		}

		private bool _email { get; set; }
		public bool Email
		{
			get { return _email; }
			set { _email = value; OnPropertyChanged(nameof(Email)); }
		}

		private bool _password { get; set; }
		public bool Password
		{
			get { return _password; }
			set { _password = value; OnPropertyChanged(nameof(Password)); }
		}
	}
}
