using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.Utilities;

namespace Pizza.MVVM.ViewModel
{
	public class ReviewsViewModel : BaseViewModel
	{
		DataManager _dataManager;
		AuthManager _authManager;
		public ICommand AddReviewCommand { get; set; }

		public ReviewsViewModel()
		{
			_dataManager = DataManager.Instance;
			_authManager = AuthManager.Instance;

			_dataManager.ReadAllReviewsData();
			Reviews = _dataManager.GetReviews();
			Reviews.CollectionChanged += Reviews_CollectionChanged;

			AddReviewCommand = new RelayCommand(AddReview);

			switch (_authManager.User.Role)
			{
				case Abstractions.ProgramAbstraction.AppRoles.Customer:
					ManagerVisibility = true;
					break;
				case Abstractions.ProgramAbstraction.AppRoles.Manager:
					ManagerVisibility = false;
					break;
			}

			Console.WriteLine("MANAGER VIS: " + ManagerVisibility);
		}

		private async void AddReview(object obj)
		{
			if (!string.IsNullOrWhiteSpace(Text))
			{
				Console.WriteLine("ADD REVIEW: " + Text);
				await _dataManager.AddReview(Text);

				Text = "";
			}
		}

		private void Reviews_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Console.WriteLine("Products_CollectionChanged @@@@@@@@@@@@@@@@@@@@@");
			Reviews = _dataManager.GetReviews();
		}

		private ObservableCollection<ReviewModel> _reviews { get; set; }

		public ObservableCollection<ReviewModel> Reviews
		{
			get { return _reviews; }
			set { _reviews = value; OnPropertyChanged(nameof(Reviews)); }
		}

		private string _text { get; set; }

		public string Text
		{
			get { return _text; }
			set { _text = value; OnPropertyChanged(nameof(Text)); }
		}


		private bool _managerVisibility { get; set; }

		public bool ManagerVisibility
		{
			get { return _managerVisibility; }
			set { _managerVisibility = value; OnPropertyChanged(nameof(ManagerVisibility)); }
		}
	}
}
