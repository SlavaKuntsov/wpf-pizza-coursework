using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Pizza.Abstractions;
using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.Utilities;

using Microsoft.Win32;

using static Pizza.Abstractions.ProductAbstraction;

namespace Pizza.MVVM.ViewModel
{
	public class AddProductViewModel : BaseViewModel
	{
		private readonly ProductAbstraction productAbstraction;

		public ICommand SaveCommand { get; set; }
		public ICommand OpenImageFileCommand { get; set; }
		public Dictionary<PizzaCategories, string> PizzaCategoryDictionary { get; set; }
		public Dictionary<PizzaSizes, string> PizzaSizesDictionary { get; set; }
		
		public AddProductViewModel()
		{
			_product = new ProductModelNew();
			Category = PizzaCategories.Pizza;

			productAbstraction = new ProductAbstraction();
			PizzaCategoryDictionary = productAbstraction.PizzaCategoriesDictionary;
			PizzaSizesDictionary = productAbstraction.PizzaSizesDictionary;

			//SliderValue = 4;
			//Size = PizzaSizes.Medium;

			SaveCommand = new RelayCommand(Save);
			OpenImageFileCommand = new RelayCommand(OpenImageFile);
		}

		private void Save(object obj)
		{
			var addedProduct = ProductModelNew.Create(Name, Description, Image, Price, true, PizzaCategories.Pizza, new List<string>{ "Размер" }, new List<string>{ "Средний" });
			//var addedProduct = ProductModelNew.Create(Name, Description, Image, Price, true, Category, new List<string>{ "Размер" }, new List<string>{ "Средний" });

			if (addedProduct.IsFailure)
			{
				MessageBox.Show(addedProduct.Error);
				return;
			}

			DataManager.Instance.AddProduct(addedProduct.Value);
			//_dataManager.AddProduct(addedProduct.Value);
			//json.WriteNewProduct(addedProduct.Value);

			ResetAll();
		}

		private void ResetAll()
		{
			Name = "";
			//FullName = "";
			Description = "";
			//Image = "";
			//Image = BitmapImage.Create(
			//	2,
			//	2,
			//	96,
			//	96,
			//	PixelFormats.Indexed1,
			//	new BitmapPalette(new List<Color> { Colors.Transparent }),
			//	new byte[] { 0, 0, 0, 0 },
			//	1);
			Category = PizzaCategories.Pizza;
			//Size = PizzaSizes.Big;
			//SliderValue = 4;
			PriceString = "";
			//CountString = "";
		}

		private void OpenImageFile(object obj)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			//string currentDirectory = Directory.GetCurrentDirectory();
			//string initialDirectory = Path.Combine(currentDirectory, "..", "..", "..", "Assets", "pizza");

			//openFileDialog.InitialDirectory = Path.GetFullPath(initialDirectory);

			string initialDirectory = @"C:\my\projects\dotnet_templates\wpf\PIzza\PIzza\Assets\pizza";

			openFileDialog.InitialDirectory = initialDirectory;
			openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";

			if (openFileDialog.ShowDialog() == true)
			{
				string selectedImagePath = openFileDialog.FileName;

				//string selectedImageName = Path.GetFileName(selectedImagePath);

				BitmapImage bitmapImage = new BitmapImage(new Uri(selectedImagePath));

				Image = bitmapImage;
				//Image = selectedImageName;
			}
			else
			{
				Image = null;
			}
		}

		private ProductModelNew _product { get; set; }
		public ProductModelNew Product
		{
			get { return _product; }
			set { _product = value; OnPropertyChanged(nameof(Product)); }
		}
		public string Name
		{
			get { return _product.Name; }
			set { _product.Name = value; OnPropertyChanged(nameof(Name)); }
		}

		//public string FullName
		//{
		//	get { return _product.FullName; }
		//	set { _product.FullName = value; OnPropertyChanged(nameof(FullName)); }
		//}

		public string Description
		{
			get { return _product.Description; }
			set { _product.Description = value; OnPropertyChanged(nameof(Description)); }
		}

		private BitmapImage _image;
		public BitmapImage Image
		{
			get { return _image; }
			set
			{
				_image = value;
				OnPropertyChanged(nameof(Image));
			}
		}

		public PizzaCategories Category
		{
			get { return _product.Category; }
			set
			{
				_product.Category = value;
				OnPropertyChanged(nameof(Category));

				if (Category == PizzaCategories.Pizza)
				{
					SizesVisibility = true;
					Console.WriteLine(_sizesVisibility);
				}
				else
				{
					SizesVisibility = false;
					Console.WriteLine(_sizesVisibility);
				}
			}
		}

		private bool _sizesVisibility { get; set; }
		public bool SizesVisibility
		{
			get { return _sizesVisibility; }
			set { _sizesVisibility = value; OnPropertyChanged(nameof(SizesVisibility)); }
		}

		//public PizzaSizes Size
		//{
		//	get { return _product.Size; }
		//	set
		//	{
		//		_product.Size = value;
		//		OnPropertyChanged(nameof(Size));
		//	}
		//}
		//private double _sliderValue { get; set; }
		//public double SliderValue
		//{
		//	get { return _sliderValue; }
		//	set
		//	{
		//		_sliderValue = value;
		//		OnPropertyChanged(nameof(SliderValue));
		//		Rating convertRating = ConvertSliderValueToRating(value);
		//		Rating = convertRating;
		//	}
		//}
		//public Rating Rating
		//{
		//	get { return _product.Rating; }
		//	set { _product.Rating = value; OnPropertyChanged(nameof(Rating)); }
		//}

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
			get { return _product.Price; }
			set { _product.Price = value; OnPropertyChanged(nameof(Price)); }
		}
		//private string _countString;
		//public string CountString
		//{
		//	get { return _countString; }
		//	set
		//	{
		//		_countString = value;
		//		OnPropertyChanged(nameof(CountString));
		//		int convertCount;
		//		if (int.TryParse(value, out convertCount))
		//		{
		//			Count = convertCount;
		//		}
		//	}
		//}
		//public int Count
		//{
		//	get { return _product.Count; }
		//	set { _product.Count = value; OnPropertyChanged(nameof(Count)); }
		//}

		//private Rating ConvertSliderValueToRating(double sliderValue)
		//{
		//	switch (sliderValue)
		//	{
		//		case 1:
		//			return Rating.One;
		//		case 2:
		//			return Rating.Two;
		//		case 3:
		//			return Rating.Three;
		//		case 4:
		//			return Rating.Four;
		//		case 5:
		//			return Rating.Five;
		//		default:
		//			return Rating.None;
		//	}
		//}
	}
}