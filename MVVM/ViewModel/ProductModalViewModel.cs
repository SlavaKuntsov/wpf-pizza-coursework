using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Pizza.Abstractions;
using Pizza.Manager;
using Pizza.MVVM.Model;
using Pizza.Utilities;

using static Pizza.Abstractions.ProductAbstraction;

namespace Pizza.MVVM.ViewModel
{
	public class ProductModalViewModel : BaseViewModel
	{
		AuthManager _authManager;
		DataManager _dataManager;
		public ICommand AddInBasketCommand { get; set; }
		public ICommand RemoveFromBasketCommand { get; set; }

		public ICommand DeleteCommand { get; set; }
		public ICommand EditCommand { get; set; }
		public ICommand SaveEditCommand { get; set; }

		public Dictionary<PizzaSizes, string> PizzaSizesDictionary { get; set; }
		public Dictionary<PizzaCategories, string> PizzaCategoryDictionary { get; set; }

		public ProductModalViewModel(ProductModelNew product, bool isCatalog)
		{
			_authManager = AuthManager.Instance;
			_dataManager = DataManager.Instance;

			//Product = product;
			AddedProduct = product;
			ChangedProduct = product.DeepCopy();

			Console.WriteLine("stock: " + AddedProduct.InStock);

			if (isCatalog)
			{
				AddedProduct.Size = PizzaSizes.Medium;
				AddedProduct.Type = PizzaTypes.Default;
			}

			foreach (var item in AddedProduct.PropertyValue)
			{
				Console.WriteLine("value: " + item);
			}

			CategoryString = AddedProduct.GetCategoryString(AddedProduct.Category);
			//SizeString = Product.GetSizeString(Product.ProductSize);

			//SliderValue = (double)ChangedProduct.Rating;
			//CategoryString = "123";

			var productAbstraction = new ProductAbstraction();
			PizzaSizesDictionary = productAbstraction.PizzaSizesDictionary;
			PizzaCategoryDictionary = productAbstraction.PizzaCategoriesDictionary;

			AddInBasketCommand = new RelayCommand(AddInBasket);
			RemoveFromBasketCommand = new RelayCommand(RemoveFromBasket);
			DeleteCommand = new RelayCommand(Delete);
			EditCommand = new RelayCommand(Edit);
			SaveEditCommand = new RelayCommand(SaveEdit);

			Size = AddedProduct.Size;
			Type = AddedProduct.Type;
			Count = AddedProduct.Count;
			switch (AddedProduct.InStock)
			{
				case true:
					InStock = PizzaInStock.Yes;
						break;
				case false:
					InStock = PizzaInStock.No;
					break;
			}

			IsEdit = false;
			Visible = true;
			Invisible = false;

			ManagerBoolVisibility = _authManager.ManagerBoolVisibility;
			CustomerBoolVisibility = !ManagerBoolVisibility;
			BasketBoolVisibility = _authManager.BasketBoolVisibility;
			CatalogBoolVisibility = !BasketBoolVisibility;
		}

		private async void RemoveFromBasket(object obj)
		{
			await _dataManager.DeleteFromBasket(AddedProduct.Id);
		}

		private void UpdateBasketProduct()
		{
			_dataManager.UpdateProductInBasket(AddedProduct);
		}

		private async void AddInBasket(object obj)
		{
			Console.WriteLine("AddedProduct: " + AddedProduct.Id);
			Console.WriteLine("AddedProduct: " + AddedProduct.Name);
			Console.WriteLine("AddedProduct: " + AddedProduct.Description);
			Console.WriteLine("AddedProduct: " + AddedProduct.Price);
			Console.WriteLine("AddedProduct: " + AddedProduct.Count);
			Console.WriteLine("AddedProduct: " + AddedProduct.Size);
			Console.WriteLine("AddedProduct: " + AddedProduct.Type);

			Console.WriteLine("size " + AddedProduct.Size.GetDescription());
			Console.WriteLine("type " + AddedProduct.Type.GetDescription());

			AddedProduct.PropertyName.Add(PizzaProperties.Size.GetDescription());
			AddedProduct.PropertyName.Add(PizzaProperties.Type.GetDescription());

			AddedProduct.PropertyValue.Add(AddedProduct.Size.GetDescription());
			AddedProduct.PropertyValue.Add(AddedProduct.Type.GetDescription());

			await _dataManager.AddInBasket(AddedProduct);

			AddedProduct.PropertyName.Clear();
			AddedProduct.PropertyValue.Clear();
		}

		private async void Delete(object obj)
		{
			Console.WriteLine("DELETE &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
			await _dataManager.DeleteProduct(AddedProduct.Id);

			Console.WriteLine("Элемент успешно удален из JSON.");
		}

		private void Edit(object obj)
		{
			_isEdit = !_isEdit;

			Visible = !Visible;
			Invisible = !Invisible;

			Console.WriteLine($"{_isEdit}, {_visible}, {_invisible}");


			Console.WriteLine("изменить");

			Console.WriteLine("product: " + AddedProduct.Name);
		}

		private async void SaveEdit(object obj)
		{
			//DataManager.Instance.EditProduct(_changedProduct);
			Console.WriteLine("CHANHE-----------------");
			await _dataManager.EditProduct(ChangedProduct);
			//Console.WriteLine("измененно и сохраненно");
		}

		//private ProductModelNew _product { get; set; }
		//public ProductModelNew Product
		//{
		//	get { return _product; }
		//	set { _product = value; OnPropertyChanged(nameof(Product)); }
		//}

		private ProductModelNew _Addedproduct { get; set; }
		public ProductModelNew AddedProduct
		{
			get { return _Addedproduct; }
			set { _Addedproduct = value; OnPropertyChanged(nameof(AddedProduct)); }
		}

		private bool _managerBoolVisibility { get; set; }
		public bool ManagerBoolVisibility
		{
			get { return _managerBoolVisibility; }
			set { _managerBoolVisibility = value; OnPropertyChanged(nameof(ManagerBoolVisibility)); }
		}
		private bool _customerBoolVisibility { get; set; }
		public bool CustomerBoolVisibility
		{
			get { return _customerBoolVisibility; }
			set { _customerBoolVisibility = value; OnPropertyChanged(nameof(CustomerBoolVisibility)); }
		}
		private bool _basketBoolVisibility { get; set; }
		public bool BasketBoolVisibility
		{
			get { return _basketBoolVisibility; }
			set { _basketBoolVisibility = value; OnPropertyChanged(nameof(BasketBoolVisibility)); }
		}
		private bool _catalogBoolVisibility { get; set; }
		public bool CatalogBoolVisibility
		{
			get { return _catalogBoolVisibility; }
			set { _catalogBoolVisibility = value; OnPropertyChanged(nameof(CatalogBoolVisibility)); }
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

		private ProductModelNew _changedProduct { get; set; }
		public ProductModelNew ChangedProduct
		{
			get { return _changedProduct; }
			set { _changedProduct = value; OnPropertyChanged(nameof(ChangedProduct)); }
		}

		private bool _sizesVisibility { get; set; }
		public bool SizesVisibility
		{
			get { return _sizesVisibility; }
			set { _sizesVisibility = value; OnPropertyChanged(nameof(SizesVisibility)); }
		}

		private PizzaSizes _size { get; set; }
		public PizzaSizes Size
		{
			get { return _size; }
			set
			{
				_size = value;

				AddedProduct.Size = value;

				switch (value)
				{
					case PizzaSizes.Small:
						UpdatePropertyValue(PizzaProperties.Size.GetDescription(), PizzaSizes.Small.GetDescription());
						break;
					case PizzaSizes.Medium:
						UpdatePropertyValue(PizzaProperties.Size.GetDescription(), PizzaSizes.Medium.GetDescription());
						break;
					case PizzaSizes.Big:
						UpdatePropertyValue(PizzaProperties.Size.GetDescription(), PizzaSizes.Big.GetDescription());
						break;
				}

				UpdateBasketProduct();
				
				OnPropertyChanged(nameof(Size));
			}
		}

		private PizzaTypes _type { get; set; }
		public PizzaTypes Type
		{
			get { return _type; }
			set
			{
				_type = value;
				AddedProduct.Type = value;

				switch (value)
				{
					case PizzaTypes.Default:
						UpdatePropertyValue(PizzaProperties.Type.GetDescription(), PizzaTypes.Default.GetDescription());
						break;
					case PizzaTypes.Thin:
						UpdatePropertyValue(PizzaProperties.Type.GetDescription(), PizzaTypes.Thin.GetDescription());
						break;
				}

				UpdateBasketProduct();
				OnPropertyChanged(nameof(Type));
			}
		}

		private int _count { get; set; }
		public int Count
		{
			get { return _count; }
			set
			{
				_count = value;
				AddedProduct.Count = value;
				Console.WriteLine("###################         COUNT: " + value);
				UpdateBasketProduct();
				OnPropertyChanged(nameof(Count));
			}
		}

		private PizzaInStock _inStock { get; set; }
		public PizzaInStock InStock
		{
			get { return _inStock; }
			set
			{
				_inStock = value;
				switch (value)
				{
					case PizzaInStock.Yes:
						ChangedProduct.InStock = true;
						break;
					case PizzaInStock.No:
						ChangedProduct.InStock = false;
						break;
				}
				OnPropertyChanged(nameof(InStock));
			}
		}

		public void UpdatePropertyValue(string propertyName, string newValue)
		{
			// Находим индекс элемента в списке PropertyName
			int index = AddedProduct.PropertyName.FindIndex(name => name == propertyName);

			// Если элемент найден, обновляем соответствующее значение в списке PropertyValue
			if (index != -1)
			{
				AddedProduct.PropertyValue[index] = newValue;
			}
			else
			{
				// Элемент с указанным именем свойства не найден
				Console.WriteLine($"Элемент с именем свойства '{propertyName}' не найден.");
			}
		}



		//private double _sliderValue { get; set; }
		//public double SliderValue
		//{
		//	get { return _sliderValue; }
		//	set
		//	{
		//		_sliderValue = value;
		//		OnPropertyChanged(nameof(SliderValue));
		//		Rating convertRating = ConvertSliderValueToRating(value);
		//		ChangedProduct.Rating = convertRating;
		//	}
		//}
		//public Rating _rating { get; set; }
		//public Rating Rating
		//{
		//	get { return _rating; }
		//	set { _rating = value; OnPropertyChanged(nameof(Rating)); }
		//}
		//private string _priceString;
		//public string PriceString
		//{
		//	get { return _priceString; }
		//	set
		//	{
		//		_priceString = value;
		//		OnPropertyChanged(nameof(PriceString));
		//		double convertPrice;
		//		if (double.TryParse(value, out convertPrice))
		//		{
		//			Price = convertPrice;
		//		}
		//	}
		//}
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
		//public string Count
		//{
		//	get { return Product.Count.ToString(); }
		//	set { Product.Count = int.Parse(value); OnPropertyChanged(nameof(Count)); }
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
