using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Media;

using Pizza.Abstractions;
using Pizza.MVVM.Model;

using CSharpFunctionalExtensions;

using Npgsql;

using static Pizza.Abstractions.ProductAbstraction;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace Pizza.Repository
{
	public class BasketRepository : IBasketRepository
	{
		private string _connectionString;

		public BasketRepository(string connection)
		{
			_connectionString = connection;
		}

		public async Task<Result<ObservableCollection<ProductPreviewModel>>> GetAllProductsPreview()
		{
			Console.WriteLine("_______________ GetAllProductsPreview ________________");
			ObservableCollection<ProductPreviewModel> products = new ObservableCollection<ProductPreviewModel>();

			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				connection.Open();

				using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_all_products_preview", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					using (NpgsqlDataReader reader = (NpgsqlDataReader)await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							int productId = reader.GetInt32(reader.GetOrdinal("product_id"));
							string productName = reader.GetString(reader.GetOrdinal("product_name"));
							string productDescription = reader.GetString(reader.GetOrdinal("product_description"));
							byte[] imageBytes = reader.IsDBNull(reader.GetOrdinal("product_image")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("product_image"));
							double productPrice = reader.GetDouble(reader.GetOrdinal("product_price"));
							bool productInStock = reader.GetBoolean(reader.GetOrdinal("product_instock"));
							string productCategoryName = reader.GetString(reader.GetOrdinal("product_category_name"));

							Console.WriteLine("NAME: " + productName);

							var product = ProductPreviewModel.Create(productId, productName, productDescription, imageBytes, productPrice, productInStock, productCategoryName);

							products.Add(product.Value);
						}
					}
				}
			}

			return Result.Success(products);
		}

		public Result<ObservableCollection<ProductModel>> GetBasket()
		{
			ObservableCollection<ProductModel> products = new ObservableCollection<ProductModel>();

			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				connection.Open();

				string sql = "SELECT p.* FROM product p INNER JOIN basket b ON p.id = b.fk_product_id;";

				using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
				{
					try
					{
						using (NpgsqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								Guid id = reader.GetGuid(reader.GetOrdinal("id"));
								string shortName = reader.GetString(reader.GetOrdinal("shortname"));
								string fullName = reader.GetString(reader.GetOrdinal("fullname"));
								string description = reader.GetString(reader.GetOrdinal("description"));
								double price = reader.GetDouble(reader.GetOrdinal("price"));
								string image = reader.GetString(reader.GetOrdinal("image"));
								byte[] imageData = new byte[reader.GetBytes(reader.GetOrdinal("imagedata"), 0, null, 0, int.MaxValue)];
								reader.GetBytes(reader.GetOrdinal("imagedata"), 0, imageData, 0, imageData.Length);
								string categoryString = reader.GetString(reader.GetOrdinal("category"));
								PizzaCategories category = (PizzaCategories)Enum.Parse(typeof(PizzaCategories), categoryString);
								string sizeString = reader.GetString(reader.GetOrdinal("size"));
								PizzaSizes size = (PizzaSizes)Enum.Parse(typeof(PizzaSizes), sizeString);
								string ratingString = reader.GetString(reader.GetOrdinal("rating"));
								Rating rating = (Rating)Enum.Parse(typeof(Rating), ratingString);
								int count = reader.GetInt32(reader.GetOrdinal("count"));
								bool inStock = reader.GetBoolean(reader.GetOrdinal("instock"));
								string date = reader.GetString(reader.GetOrdinal("date"));

								var product = ProductModel.Create(id, shortName, fullName, description, price, image, imageData, category, size, rating, count, date);

								Console.WriteLine($"Id: {id}, Name: {shortName}, FullName: {fullName}, Description: {description}, Price: {price}, Image: {image}, Category: {category}, Size: {size}, Rating: {rating}, Count: {count}, InStock: {inStock}, Date: {date}");

								if (product.IsFailure)
								{
									throw new Exception("ProductModel Create Failed");
								}

								products.Add(product.Value);
							}
						}
					}
					catch (SqlException ex)
					{
						Console.WriteLine("Ошибка выполнения запроса к базе данных: " + ex.Message);
						return Result.Failure<ObservableCollection<ProductModel>>("Ошибка выполнения запроса к базе данных: " + ex.Message);
					}
				}
			}

			return Result.Success(products);
		}


		public void AddInBasket(Guid id)
		{
			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				connection.Open();

				string sql1 = "INSERT INTO basket (fk_product_id) VALUES (@id)";

				using (NpgsqlCommand command1 = new NpgsqlCommand(sql1, connection))
				{
					try
					{
						command1.Parameters.Clear();
						command1.Parameters.AddWithValue("@id", id);
						Console.WriteLine($"{id}");

						command1.ExecuteNonQuery();
					}
					catch (SqlException ex)
					{
						Console.WriteLine("Ошибка выполнения запроса к базе данных: " + ex.Message);
					}
				}
			}
		}
	}
}
