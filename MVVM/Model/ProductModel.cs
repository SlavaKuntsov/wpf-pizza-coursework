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
	public class ProductModelNew
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

		public ProductModelNew() { }

		

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
