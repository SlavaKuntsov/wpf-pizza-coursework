using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

using Pizza	.Abstractions;
using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.Utilities;

using static Pizza.Abstractions.ProductAbstraction;

namespace Pizza.MVVM.ViewModel
{
	public class ProductModalViewModel : BaseViewModel
	{
		public ICommand DeleteCommand { get; set; }
		public ICommand EditCommand { get; set; }
		public ICommand SaveEditCommand { get; set; }

		public Dictionary<PizzaSizes, string> PizzaSizesDictionary { get; set; }
		public Dictionary<PizzaCategories, string> PizzaCategoryDictionary { get; set; }

		public ProductModalViewModel(ProductModel product)
		{
			Product = product;
			ChangedProduct = product.DeepCopy();

			CategoryString = Product.GetCategoryString(Product.Category);
			SizeString = Product.GetSizeString(Product.Size);

			SliderValue = (double)ChangedProduct.Rating;
			//CategoryString = "123";

			var productAbstraction = new ProductAbstraction();
			PizzaSizesDictionary = productAbstraction.PizzaSizesDictionary;
			PizzaCategoryDictionary = productAbstraction.PizzaCategoriesDictionary;

			DeleteCommand = new RelayCommand(Delete);
			EditCommand = new RelayCommand(Edit);
			SaveEditCommand = new RelayCommand(SaveEdit);

			IsEdit = false;
			Visible = true;
			Invisible = false;
		}

		private void Delete(object obj)
		{
			DataManager.Instance.DeleteProduct(_product.Id);

			Console.WriteLine("Элемент успешно удален из JSON.");
		}

		private void Edit(object obj)
		{
			_isEdit = !_isEdit;

			Visible = !Visible;
			Invisible = !Invisible;

			Console.WriteLine($"{_isEdit}, {_visible}, {_invisible}");


			Console.WriteLine("изменить");

			Console.WriteLine("product: " + Product.ShortName);
			Console.WriteLine("changedProduct: " + ChangedProduct.ShortName);
		}

		private void SaveEdit(object obj)
		{
			DataManager.Instance.EditProduct(_changedProduct);
			Console.WriteLine("_changedProduct.Rating: " + _changedProduct.Rating);

			Console.WriteLine("измененно и сохраненно");
		}

		private bool _isEdit { get; set; }
		public bool IsEdit
		{
			get { return _isEdit; }
			set { _isEdit = value; OnPropertyChanged(nameof(IsEdit)); }
		}

		private bool _visible { get; set; }
		public bool Visible
		{
			get { return _visible; }
			set { _visible = value; OnPropertyChanged(nameof(Visible)); }
		}

		private bool _invisible { get; set; }
		public bool Invisible
		{
			get { return _invisible; }
			set { _invisible = value; OnPropertyChanged(nameof(Invisible)); }
		}

		private ProductModel _product { get; set; }
		public ProductModel Product
		{
			get { return _product; }
			set { _product = value; OnPropertyChanged(nameof(Product)); }
		}

		private string _сategoryString { get; set; }
		public string CategoryString
		{
			get { return _сategoryString; }
			set { _сategoryString = value; OnPropertyChanged(nameof(CategoryString)); }
		}

		private string _sizeString { get; set; }
		public string SizeString
		{
			get { return _sizeString; }
			set
			{
				_sizeString = value;
				OnPropertyChanged(nameof(SizeString));
			}
		}

		private ProductModel _changedProduct { get; set; }
		public ProductModel ChangedProduct
		{
			get { return _changedProduct; }
			set { _changedProduct = value; OnPropertyChanged(nameof(ChangedProduct)); }
		}

		private string _shortName { get; set; }
		public string ShortName
		{
			get { return _shortName; }
			set { _shortName = value; OnPropertyChanged(nameof(ShortName)); }
		}
		public string _fullName { get; set; }
		public string FullName
		{
			get { return _fullName; }
			set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
		}
		public string _description { get; set; }
		public string Description
		{
			get { return _description; }
			set { _description = value; OnPropertyChanged(nameof(Description)); }
		}
		public string _image { get; set; }
		public string Image
		{
			get { return _image; }
			set { _image = value; OnPropertyChanged(nameof(Image)); }
		}
		private ImageSource _selectedImage;
		public ImageSource SelectedImage
		{
			get { return _selectedImage; }
			set
			{
				_selectedImage = value;
				OnPropertyChanged(nameof(SelectedImage));
			}
		}
		public PizzaCategories _category { get; set; }
		public PizzaCategories Category
		{
			get { return Category; }
			set
			{
				_category = value;
				OnPropertyChanged(nameof(Category));

				if (value == PizzaCategories.Pizza)
				{
					SizesVisibility = true;
				}
				else
				{
					SizesVisibility = false;
				}
			}
		}
		private bool _sizesVisibility { get; set; }
		public bool SizesVisibility
		{
			get { return _sizesVisibility; }
			set { _sizesVisibility = value; OnPropertyChanged(nameof(SizesVisibility)); }
		}
		public PizzaSizes _size { get; set; }
		public PizzaSizes Size
		{
			get { return _size; }
			set
			{
				_size = value;
				OnPropertyChanged(nameof(Size));
			}
		}
		private double _sliderValue { get; set; }
		public double SliderValue
		{
			get { return _sliderValue; }
			set
			{
				_sliderValue = value;
				OnPropertyChanged(nameof(SliderValue));
				Rating convertRating = ConvertSliderValueToRating(value);
				ChangedProduct.Rating = convertRating;
			}
		}
		public Rating _rating { get; set; }
		public Rating Rating
		{
			get { return _rating; }
			set { _rating = value; OnPropertyChanged(nameof(Rating)); }
		}
		private string _priceString;
		public string PriceString
		{
			get { return _priceString; }
			set
			{
				_priceString = value;
				OnPropertyChanged(nameof(PriceString));
				double convertPrice;
				if (double.TryParse(value, out convertPrice))
				{
					Price = convertPrice;
				}
			}
		}
		public double Price
		{
			get { return Price; }
			set { Price = value; OnPropertyChanged(nameof(Price)); }
		}
		private string _countString;
		public string CountString
		{
			get { return _countString; }
			set
			{
				_countString = value;
				OnPropertyChanged(nameof(CountString));
				int convertCount;
				if (int.TryParse(value, out convertCount))
				{
					Count = convertCount;
				}
			}
		}
		public int Count
		{
			get { return Count; }
			set { Count = value; OnPropertyChanged(nameof(Count)); }
		}

		private Rating ConvertSliderValueToRating(double sliderValue)
		{
			switch (sliderValue)
			{
				case 1:
					return Rating.One;
				case 2:
					return Rating.Two;
				case 3:
					return Rating.Three;
				case 4:
					return Rating.Four;
				case 5:
					return Rating.Five;
				default:
					return Rating.None;
			}
		}
	}
}
