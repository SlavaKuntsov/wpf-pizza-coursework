using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Pizza.Manager;

using CSharpFunctionalExtensions;

using Newtonsoft.Json;

using static Pizza.Abstractions.ProductAbstraction;

namespace Pizza.MVVM.Model
{
	public class ProductModel
	{
		public Guid Id { get; set; }
		public string ShortName { get; set; }
		public string FullName { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public string Image { get; set; }
		[JsonIgnore]
		public ImageSource ImageSource { get; set; }
		public byte[] ImageData { get; set; }
		public PizzaCategories Category { get; set; }
		//public string CategoryString { get; set; }
		public PizzaSizes Size { get; set; }
		//public string SizeString { get; set; }
		public Rating Rating { get; set; }
		[JsonIgnore]
		public List<int> StringRating { get; set; } = new List<int>();
		public int Count { get; set; }
		public bool InStock { get; set; }
		public string Date { get; set; }

		LocalizationManager _localizationManager;

		public ProductModel() { }

		public ProductModel(Guid id, string shortName, string fullName, string description, double price, string imageName, ImageSource imageSource, byte[] imageData, PizzaCategories category, PizzaSizes size, Rating rating, int count)
		{
			Id = id;
			ShortName = shortName;
			FullName = fullName;
			Description = description;
			Price = price;
			Image = imageName;
			ImageSource = imageSource;
			ImageData = imageData;
			Category = category;
			Size = size;
			Rating = rating;
			Count = count;
			InStock = count > 0;
			Date = string.Format("{0:dd/MM/yyyy}", DateTime.Now);

			switch (rating)
			{
				case Rating.None:
					break;
				case Rating.One:
					StringRating.Add(1);
					break;
				case Rating.Two:
					StringRating.AddRange(new int[] { 1, 2 });
					break;
				case Rating.Three:
					StringRating.AddRange(new int[] { 1, 2, 3 });
					break;
				case Rating.Four:
					StringRating.AddRange(new int[] { 1, 2, 3, 4 });
					break;
				case Rating.Five:
					StringRating.AddRange(new int[] { 1, 2, 3, 4, 5 });
					break;
			}
		}

		public ProductModel(Guid id, string shortName, string fullName, string description, double price, string imageName, ImageSource imageSource, byte[] imageData, PizzaCategories category, PizzaSizes size, Rating rating, int count, string date)
		{
			Id = id;
			ShortName = shortName;
			FullName = fullName;
			Description = description;
			Price = price;
			Image = imageName;
			ImageSource = imageSource;
			ImageData = imageData;
			Category = category;
			Size = size;
			Rating = rating;
			Count = count;
			InStock = count > 0;
			Date = string.Format("{0:dd/MM/yyyy}", DateTime.Now);

			switch (rating)
			{
				case Rating.None:
					break;
				case Rating.One:
					StringRating.Add(1);
					break;
				case Rating.Two:
					StringRating.AddRange(new int[] { 1, 2 });
					break;
				case Rating.Three:
					StringRating.AddRange(new int[] { 1, 2, 3 });
					break;
				case Rating.Four:
					StringRating.AddRange(new int[] { 1, 2, 3, 4 });
					break;
				case Rating.Five:
					StringRating.AddRange(new int[] { 1, 2, 3, 4, 5 });
					break;
			}
		}

		public ProductModel(Guid id, string shortName, string fullName, string description, double price, string imageName, ImageSource imageSource, PizzaCategories category, PizzaSizes size, Rating rating, int count, string date)
		{
			Id = id;
			ShortName = shortName;
			FullName = fullName;
			Description = description;
			Price = price;
			Image = imageName;
			ImageSource = imageSource;
			Category = category;
			Size = size;
			Rating = rating;
			Count = count;
			InStock = count > 0;
			Date = date;

			switch (rating)
			{
				case Rating.None:
					break;
				case Rating.One:
					StringRating.Add(1);
					break;
				case Rating.Two:
					StringRating.AddRange(new int[] { 1, 2 });
					break;
				case Rating.Three:
					StringRating.AddRange(new int[] { 1, 2, 3 });
					break;
				case Rating.Four:
					StringRating.AddRange(new int[] { 1, 2, 3, 4 });
					break;
				case Rating.Five:
					StringRating.AddRange(new int[] { 1, 2, 3, 4, 5 });
					break;
			}
		}

		public static Result<ProductModel> Create(Guid id, string shortName, string fullName, string description, double price, string imageName, PizzaCategories category, PizzaSizes size, Rating rating, int count)
		{
			Console.WriteLine("create rating: " + rating);
			if (
				string.IsNullOrWhiteSpace(shortName) ||
				string.IsNullOrWhiteSpace(fullName) ||
				string.IsNullOrEmpty(description))
			{
				return Result.Failure<ProductModel>("Name, FullName or Description can not be empty");
			}

			if (price <= 0 || count <= 0)
			{
				return Result.Failure<ProductModel>("Price or Count must be greater than 0");
			}

			if (string.IsNullOrWhiteSpace(imageName))
			{
				imageName = "default.png";
			}

			string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

			string assetsDirectory = Path.Combine(currentDirectory, "..", "..", "Assets", "pizza");
			string imagePath = Path.Combine(assetsDirectory, imageName);


			if (string.IsNullOrEmpty(imageName))
			{
				return Result.Failure<ProductModel>("ImageName is can not be empty");
			}
			if (!Directory.Exists(assetsDirectory))
			{
				return Result.Failure<ProductModel>("Directory not exist");
			}

			ImageSource imageSource = new BitmapImage(new Uri(imagePath));

			string path = new Uri(imagePath).LocalPath;

			byte[] imageBytes;

			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileStream))
				{
					imageBytes = binaryReader.ReadBytes((int)fileStream.Length);
				}
			}

			byte[] imageData = imageBytes;

			ProductModel product =  new ProductModel(id, shortName, fullName, description, price, imageName, imageSource, imageData, category, size, rating, count);

			return Result.Success(product);
		}
		public static Result<ProductModel> Create(Guid id, string shortName, string fullName, string description, double price, string imageName, byte[] imageData, PizzaCategories category, PizzaSizes size, Rating rating, int count)
		{
			Console.WriteLine("create rating: " + rating);
			if (
				string.IsNullOrWhiteSpace(shortName) ||
				string.IsNullOrWhiteSpace(fullName) ||
				string.IsNullOrEmpty(description))
			{
				return Result.Failure<ProductModel>("Name, FullName or Description can not be empty");
			}

			if (price <= 0 || count <= 0)
			{
				return Result.Failure<ProductModel>("Price or Count must be greater than 0");
			}

			if (string.IsNullOrWhiteSpace(imageName))
			{
				imageName = "default.png";
			}

			string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

			string assetsDirectory = Path.Combine(currentDirectory, "..", "..", "Assets", "pizza");
			string imagePath = Path.Combine(assetsDirectory, imageName);


			if (string.IsNullOrEmpty(imageName))
			{
				return Result.Failure<ProductModel>("ImageName is can not be empty");
			}
			if (!Directory.Exists(assetsDirectory))
			{
				return Result.Failure<ProductModel>("Directory not exist");
			}

			ImageSource imageSource = new BitmapImage(new Uri(imagePath));

			ProductModel product =  new ProductModel(id, shortName, fullName, description, price, imageName, imageSource, imageData, category, size, rating, count);

			return Result.Success(product);
		}

		public static Result<ProductModel> Create(Guid id, string shortName, string fullName, string description, double price, string imageName, byte[] imageData, PizzaCategories category, PizzaSizes size, Rating rating, int count, string date)
		{
			Console.WriteLine("create rating: " + rating);
			if (
				string.IsNullOrWhiteSpace(shortName) ||
				string.IsNullOrWhiteSpace(fullName) ||
				string.IsNullOrEmpty(description))
			{
				return Result.Failure<ProductModel>("Name, FullName or Description can not be empty");
			}

			if (price <= 0 || count <= 0)
			{
				return Result.Failure<ProductModel>("Price or Count must be greater than 0");
			}

			if (string.IsNullOrWhiteSpace(imageName))
			{
				imageName = "default.png";
			}

			string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

			string assetsDirectory = Path.Combine(currentDirectory, "..", "..", "Assets", "pizza");
			string imagePath = Path.Combine(assetsDirectory, imageName);


			if (string.IsNullOrEmpty(imageName))
			{
				return Result.Failure<ProductModel>("ImageName is can not be empty");
			}
			if (!Directory.Exists(assetsDirectory))
			{
				return Result.Failure<ProductModel>("Directory not exist");
			}

			ImageSource imageSource = new BitmapImage(new Uri(imagePath));

			ProductModel product =  new ProductModel(id, shortName, fullName, description, price, imageName, imageSource, imageData, category, size, rating, count, date);

			return Result.Success(product);
		}

		public static Result<ProductModel> Create(Guid id, string shortName, string fullName, string description, double price, string imageName, PizzaCategories category, PizzaSizes size, Rating rating, int count, string date)
		{


			Console.WriteLine("create rating: " + rating);
			if (
				string.IsNullOrWhiteSpace(shortName) ||
				string.IsNullOrWhiteSpace(fullName) ||
				string.IsNullOrEmpty(description))
			{
				return Result.Failure<ProductModel>("Name, FullName or Description can not be empty");
			}

			if (price <= 0 || count <= 0)
			{
				return Result.Failure<ProductModel>("Price or Count must be greater than 0");
			}

			if (string.IsNullOrWhiteSpace(imageName))
			{
				imageName = "default.png";
			}

			string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

			string assetsDirectory = Path.Combine(currentDirectory, "..", "..", "Assets", "pizza");
			string imagePath = Path.Combine(assetsDirectory, imageName);


			if (string.IsNullOrEmpty(imageName))
			{
				return Result.Failure<ProductModel>("ImageName is can not be empty");
			}
			if (!Directory.Exists(assetsDirectory))
			{
				return Result.Failure<ProductModel>("Directory not exist");
			}

			ImageSource imageSource = new BitmapImage(new Uri(imagePath));

			string path = new Uri(imagePath).LocalPath;

			byte[] imageBytes;

			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileStream))
				{
					imageBytes = binaryReader.ReadBytes((int)fileStream.Length);
				}
			}

			byte[] imageData = imageBytes;

			ProductModel product =  new ProductModel(id, shortName, fullName, description, price, imageName, imageSource, imageData, category, size, rating, count, date);

			return Result.Success(product);
		}

		public ProductModel DeepCopy()
		{
			ProductModel copiedProduct = new ProductModel
			{
				Id = this.Id,
				ShortName = this.ShortName,
				FullName = this.FullName,
				Description = this.Description,
				Price = this.Price,
				Image = this.Image,
				ImageSource = this.ImageSource,
				ImageData = this.ImageData,
				Category = this.Category,
				//CategoryString = this.CategoryString,
				Size = this.Size,
				//SizeString = this.SizeString,
				Rating = this.Rating,
				StringRating = new List<int>(this.StringRating),
				Count = this.Count,
				InStock = this.InStock,
				Date = this.Date
			};

			return copiedProduct;
		}

		public string GetCategoryString(PizzaCategories category)
		{
			//_localizationManager = LocalizationManager.Instance;

			//switch (_localizationManager.CurrentLanguage.Name.ToLower())
			//{
			//	case "ru-ru":

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

			//		break;
			//	case "en-us":

			//		switch (category)
			//		{
			//			case PizzaCategories.Pizza:
			//				return "Pizza";
			//			case PizzaCategories.Dessert:
			//				return "Dessert";
			//			case PizzaCategories.Drink:
			//				return "Drink";
			//		}

			//		break;
			//	default:
			//		break;
			//}

			//return "";
		}

		public string GetSizeString(PizzaSizes size)
		{
			//_localizationManager = LocalizationManager.Instance;

			//switch (_localizationManager.CurrentLanguage.Name.ToLower())
			//{
			//	case "ru-ru":

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

			//		break;
			//	case "en-us":

			//		switch (size)
			//		{
			//			case PizzaSizes.Small:
			//				return "Small";
			//			case PizzaSizes.Medium:
			//				return "Medium";
			//			case PizzaSizes.Big:
			//				return "Big";
			//		}

			//		break;
			//	default:
			//		break;
			//}


			//return "";
		}

		public byte[] ConvertImageToByteArray(string imagePath)
		{
			string path = new Uri(imagePath).LocalPath;
			Console.WriteLine("!!!!!!!!!!!!!!     " + path);

			byte[] imageBytes;

			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileStream))
				{
					imageBytes = binaryReader.ReadBytes((int)fileStream.Length);
				}
			}

			return imageBytes;
		}
	}
}
