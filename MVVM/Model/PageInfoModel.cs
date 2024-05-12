using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pizza.MVVM.Model
{
	public class PageInfoModel
	{
		public int Value { get; set; }
		public int? LastProductId { get; set; } = null;
		public int ProductOnPage { get; set; }

		public PageInfoModel() { }
		public PageInfoModel(int value)
		{
			Value = value;
		}
	}

	public class NumberSelectionModel : INotifyPropertyChanged
	{
		private int _number;
		private ICommand _command;
		private bool _isSelected;

		public int Number
		{
			get { return _number; }
			set
			{
				_number = value;
				OnPropertyChanged(nameof(Number));
			}
		}

		public ICommand Command
		{
			get { return _command; }
			set
			{
				_command = value;
				OnPropertyChanged(nameof(Command));
			}
		}

		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				_isSelected = value;
				OnPropertyChanged(nameof(IsSelected));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
