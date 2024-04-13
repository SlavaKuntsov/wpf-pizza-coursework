using System;
using System.Collections.Generic;
using System.IO;

using Pizza.MVVM.Model;

using CSharpFunctionalExtensions;

using Newtonsoft.Json;


namespace Pizza.Manager
{
	public class JsonActionsManager
	{
		private const string dataPath = "../../Data/Products.json";
		public JsonActionsManager()
		{
		}

		public Result<List<ProductModel>> GetAllProducts()
		{
			JsonSerializerSettings options = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				NullValueHandling = NullValueHandling.Ignore
			};

			if (File.Exists(dataPath))
			{
				string jsonRead = File.ReadAllText(dataPath);
				List<ProductModel> products = JsonConvert.DeserializeObject<List<ProductModel>>(jsonRead, options);

				return Result.Success(products);
			}
			else
			{
				return Result.Failure<List<ProductModel>>("FILE NOT EXIST");
			}
		}

		public void WriteAllProducts(List<ProductModel> products)
		{
			JsonSerializerSettings options = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				NullValueHandling = NullValueHandling.Ignore
			};

			string newJson = JsonConvert.SerializeObject(products, options);

			File.WriteAllText(dataPath, newJson);
		}

		public void WriteNewProduct(ProductModel product)
		{
			JsonSerializerSettings options = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				NullValueHandling = NullValueHandling.Ignore
			};

			if (File.Exists(dataPath))
			{
				string jsonRead = File.ReadAllText(dataPath);
				List<ProductModel> products = JsonConvert.DeserializeObject<List<ProductModel>>(jsonRead, options);

				products.Add(product);

				string jsonWrite = JsonConvert.SerializeObject(products, Formatting.Indented);
				File.WriteAllText(dataPath, jsonWrite);
			}
			else
			{
				Console.WriteLine("FILE NOT EXIST");
			}
		}
	}
}
