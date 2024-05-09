using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using CSharpFunctionalExtensions;

using static Pizza.Abstractions.ProductAbstraction;

namespace Pizza.MVVM.Model
{
	public class ProductPreviewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public byte[] Image { get; set; }
		//public ImageSource Image { get; set; }
		//public byte[] ImageData { get; set; }
		public double Price { get; set; }
		public bool InStock { get; set; }
		public PizzaCategories Category { get; set; }

		//public ProductPreviewModel(int id, string name, string description, ImageSource image, double price, bool inStock, PizzaCategories category)
		//{
		//	Id = id;
		//	Name = name;
		//	Description = description;
		//	Image = image;
		//	Price = price;
		//	InStock = inStock;
		//	Category = category;
		//}

		public ProductPreviewModel(int id, string name, string description, byte[] image, double price, bool inStock, PizzaCategories category)
		{
			Id = id;
			Name = name;
			Description = description;
			Image = image;
			Price = price;
			InStock = inStock;
			Category = category;
		}

		public static Result<ProductPreviewModel> Create(int id, string name, string description, byte[] image, double price, bool inStock, string category)
		{
			//string imagePath = Path.GetFullPath("../../Assets/pizza/bav.png");

			//Bitmap image1 = new Bitmap(imagePath);

			//// Преобразование изображения в массив байтов
			//byte[] byteArray = ImageToByteArray(image1);

			ProductPreviewModel product = new ProductPreviewModel(id, name, description, image, price, inStock, ConvertToAppRole(category));
			//ProductPreviewModel product = new ProductPreviewModel(id, name, description, ConvertByteArrayToImage(byteArray), price, inStock, ConvertToAppRole(category));

			return product;
		}

		static byte[] ImageToByteArray(Image image)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				return ms.ToArray();
			}
		}

		private static PizzaCategories ConvertToAppRole(string categoryName)
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


		public static ImageSource ConvertByteArrayToImage(byte[] byteArray)
		{
			if (byteArray == null || byteArray.Length == 0)
				return null;

			using (MemoryStream memoryStream = new MemoryStream(byteArray))
			{
				using (Bitmap bitmap = new Bitmap(memoryStream))
				{
					IntPtr hBitmap = bitmap.GetHbitmap();
					if (hBitmap != IntPtr.Zero) // Проверка на действительный дескриптор
					{
						try
						{
							return Imaging.CreateBitmapSourceFromHBitmap(
								hBitmap,
								IntPtr.Zero,
								Int32Rect.Empty,
								BitmapSizeOptions.FromEmptyOptions());
						}
						finally
						{
							DeleteObject(hBitmap); // Очистка ресурсов GDI
						}
					}
					else
					{
						throw new ArgumentException("Недопустимый параметр: hBitmap");
					}
				}
			}
		}

		[System.Runtime.InteropServices.DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);
	}
}