using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using CSharpFunctionalExtensions;

using static Pizza.Abstractions.ProductAbstraction;

namespace Pizza.MVVM.Model
{
	public class ProductModelNew
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public byte[] Image { get; set; }
		public ImageSource ImageSource { get; set; }
		public double Price { get; set; }
		public bool InStock { get; set; }
		public PizzaCategories Category { get; set; }
		public List<string> PropertyName = new List<string>();
		public List<string> PropertyValue = new List<string>();

		public ProductModelNew() { }

		// для Create функции
		private ProductModelNew(string name, string description, byte[] image, double price, bool inStock, PizzaCategories category, List<string> propertyName, List<string> propertyValue)
		{
			Name = name;
			Description = description;
			Image = image;
			Price = price;
			InStock = inStock;
			Category = category;
			foreach (string propName in propertyName)
			{
				PropertyName.Add(propName);
			}
			foreach (string propValue in propertyValue)
			{
				PropertyValue.Add(propValue);
			}
		}

		// для функции получение из бд
		public ProductModelNew(int id, string name, string description, byte[] image, double price, bool inStock, string category)
		{
			Id = id;
			Name = name;
			Description = description;
			ImageSource = ConvertByteArrayToImageSource(image);
			Price = price;
			InStock = inStock;
			Category = ConvertToCategory(category);
		}

		// для функции добавление нового продукта
		public static Result<ProductModelNew> Create(string name, string description, BitmapImage image, double price, bool inStock, PizzaCategories category, List<string> propertyName, List<string> propertyValue)
		{
			var byteImage = ConvertBitmapImageToByteArray(image);

			if (byteImage.IsFailure)
			{
				return Result.Failure<ProductModelNew>(byteImage.Error);
			}

			ProductModelNew product = new ProductModelNew(name, description, byteImage.Value, price, inStock, category,  propertyName, propertyValue);

			return Result.Success(product);
		}

		public string GetCategoryString(PizzaCategories category)
		{
			switch (category)
			{
				case PizzaCategories.Pizza:
					return "Пицца";
				case PizzaCategories.Dessert:
					return "Десерт";
				case PizzaCategories.Drink:
					return "Напиток";
				default:
					return "Пицца";
			}
		}

		public string GetSizeString(PizzaSizes size)
		{
			switch (size)
			{
				case PizzaSizes.Small:
					return "Маленькая";
				case PizzaSizes.Medium:
					return "Средняя";
				case PizzaSizes.Big:
					return "Большая";
				default:
					return "Маленькая";
			}
		}

		public static Result<byte[]> ConvertBitmapImageToByteArray(BitmapImage bitmapImage)
		{
			try
			{
				byte[] byteArray = null;

				// Инициализация кодировщика PNG
				PngBitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

				// Создание памяти для потока и запись изображения
				using (MemoryStream stream = new MemoryStream())
				{
					encoder.Save(stream);
					byteArray = stream.ToArray();
				}

				return Result.Success(byteArray);
			}
			catch (Exception ex)
			{
				return Result.Failure<byte[]>(ex.Message);
			}

		}

		public static ImageSource ConvertByteArrayToImageSource(byte[] byteArray)
		{
			BitmapImage bitmapImage = new BitmapImage();
			using (MemoryStream stream = new MemoryStream(byteArray))
			{
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.StreamSource = stream;
				bitmapImage.EndInit();
			}
			return bitmapImage;
		}

		private static PizzaCategories ConvertToCategory(string categoryName)
		{
			foreach (PizzaCategories category in Enum.GetValues(typeof(PizzaCategories)))
			{
				DescriptionAttribute descriptionAttribute = typeof(PizzaCategories)
			.GetField(category.ToString())
			.GetCustomAttribute<DescriptionAttribute>();

				if (descriptionAttribute != null && descriptionAttribute.Description == categoryName)
				{
					return category;
				}
			}

			// Обработка, если категория не найдена
			// Возвращаемое значение по умолчанию или выбрасывайте исключение, в зависимости от вашей логики
			return PizzaCategories.Pizza; // Возвращаем значение по умолчанию
		}
	}
}
