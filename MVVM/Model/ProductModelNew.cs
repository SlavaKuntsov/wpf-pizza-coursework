using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
		public int Count { get; set; } = 1;
		public double SumPrice { get; set; }
		public PizzaCategories Category { get; set; }
		public List<string> PropertyName = new List<string>();
		public List<string> PropertyValue = new List<string>();
		public PizzaSizes Size { get; set; }
		public PizzaTypes Type { get; set; }

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
			foreach (string propName in propertyValue)
			{
				PropertyValue.Add(propName);
			}
		}

		// для функции получение из бд
		public ProductModelNew(int id, string name, string description, byte[] image, double price, bool inStock, int count, string category, List<string> propertyName, List<string> propertyValue)
		{
			Id = id;
			Name = name;
			Description = description;
			ImageSource = ConvertByteArrayToImageSource(image);
			Price = price;
			InStock = inStock;
			Count = count;
			Category = ConvertToCategory(category);
			//Console.WriteLine("propName: " + propName);
			//Console.WriteLine("propValue: " + propValue);

			//foreach (var item in propertyName)
			//{
			//PropertyName.Add(item);
			//	Console.WriteLine("propertyName: " + item);
			//}
			//foreach (var item in propertyValue)
			//{
			//PropertyValue.Add(item);
			//	Console.WriteLine("propertyValue: " + item);
			//}
			foreach (string propName in propertyName)
			{
				PropertyName.Add(propName);
			}
			foreach (string propName in propertyValue)
			{
				PropertyValue.Add(propName);
			}
			if (Category == PizzaCategories.Pizza)
				if (propertyName.Count != 0)
				{
					for (int i = 0; i < propertyName.Count; i++)
					{
						//Console.WriteLine("i: " + i);
						//Console.WriteLine("PropertyName[i]: " + propertyName[i]);
						Console.WriteLine("model name i: " + propertyName[i]);
						Console.WriteLine("model value i: " + propertyValue[i]);
						switch (propertyValue[i])
						{
							case "Маленькая":
								Size = PizzaSizes.Small;
								break;
							case "Средняя":
								Size = PizzaSizes.Medium;
								break;
							case "Большая":
								Size = PizzaSizes.Big;
								break;
						}
						switch (propertyValue[i])
						{
							case "Обычное":
								Type = PizzaTypes.Default;
								break;
							case "Тонкое":
								Type = PizzaTypes.Thin;
								break;
						}
					}
				}
			Console.WriteLine("MODEL SIZE: " + Size);
			Console.WriteLine("MODEL TYPE: " + Type);
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
			Console.WriteLine("КАТЕГОРИЯ ДО:" + category);
			Console.WriteLine("КАТЕГОРИЯ после:" + ConvertToCategory(category));
		}

		public ProductModelNew(string name, byte[] image, double price, int count)
		{
			Name = name;
			ImageSource = ConvertByteArrayToImageSource(image);
			Price = price;
			Count = count;
			SumPrice = price * count;
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
				if (bitmapImage == null)
				{
					return Result.Failure<byte[]>("BitmapImage is null.");
				}

				byte[] byteArray;

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

		public ProductModelNew DeepCopy()
		{
			if (this.Image == null && this.ImageSource is BitmapImage bitmapImage)
			{
				var conversionResult = ConvertBitmapImageToByteArray(bitmapImage);
				if (conversionResult.IsSuccess)
				{
					this.Image = conversionResult.Value;
				}
				else
				{
					// Обработка ошибки, если произошла ошибка конвертации
					Console.WriteLine($"Ошибка конвертации изображения: {conversionResult.Error}");
				}
			}

			ProductModelNew copiedProduct = new ProductModelNew
			{
				Id = this.Id,
				Name = this.Name,
				Description = this.Description,
				Image = this.Image,
				ImageSource = this.ImageSource,
				Price = this.Price,
				InStock = this.InStock,
				Count = this.Count,
				Category = this.Category,
				Size = this.Size,
				Type = this.Type,
				PropertyName = this.PropertyName,
				PropertyValue = this.PropertyValue
			};

			return copiedProduct;
		}
	}
}
