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

namespace Pizza.Repository
{
	public class ProductRepository : IProductRepository
	{
		private string _connectionString;

		public ProductRepository(string connection)
		{
			_connectionString = connection;
		}

		public Result<ObservableCollection<ProductModel>> GetAllProducts()
		{
			ObservableCollection<ProductModel> products = new ObservableCollection<ProductModel>();

			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				connection.Open();

				string sql = "SELECT * FROM product;";

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

		public void AddProduct(ProductModel product)
		{
			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				connection.Open();

				string sql = "INSERT INTO public.product (id, shortname, fullname, description, price, image, imageData, size, rating, count, instock, date, category) VALUES (@Id, @Name, @FullName, @Description, @Price, @Image, @ImageData, @Size, @Rating, @Count, @InStock, @Date, @Category)";

				using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
				//using (NpgsqlCommand command = new NpgsqlCommand("add_product", connection))
				{
					Console.WriteLine("))))))))))))))))))))))))))))))))))))))))))))))))))))");
					try
					{
						//command.CommandType = System.Data.CommandType.StoredProcedure;

						//var imageByte = ConvertImageToByteArray(product.ImageSource.ToString());

						command.Parameters.AddWithValue("Id", product.Id);
						command.Parameters.AddWithValue("Name", product.ShortName);
						command.Parameters.AddWithValue("FullName", product.FullName);
						command.Parameters.AddWithValue("Description", product.Description);
						command.Parameters.AddWithValue("Price", product.Price);
						command.Parameters.AddWithValue("Image", product.Image);
						command.Parameters.AddWithValue("ImageData", product.ImageData);
						command.Parameters.AddWithValue("Size", NpgsqlTypes.NpgsqlDbType.Varchar, product.Size.ToString());
						command.Parameters.AddWithValue("Rating", NpgsqlTypes.NpgsqlDbType.Varchar, product.Rating.ToString());
						command.Parameters.AddWithValue("Count", product.Count);
						command.Parameters.AddWithValue("InStock", product.InStock);
						command.Parameters.AddWithValue("Date", NpgsqlTypes.NpgsqlDbType.Date).Value = product.Date;
						command.Parameters.AddWithValue("Category", NpgsqlTypes.NpgsqlDbType.Varchar, product.Category.ToString());

						command.ExecuteNonQuery();

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine("Продукт успешно удален.");
						}
						else
						{
							Console.WriteLine("Продукт с указанным идентификатором не найден.");
							//return Result.Failure<bool>("Продукт с указанным идентификатором не найден.");
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("Ошибка выполнения запроса к базе данных: " + ex.Message);
						//return Result.Failure<bool>("Ошибка выполнения запроса к базе данных: " + ex.Message);
					}

				}
			}
		}

		public Result<bool> DeleteProduct(Guid id)
		{
			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				connection.Open();

				string sql = "DELETE FROM public.product WHERE id = @Id";


				using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
				{
					try
					{
						command.Parameters.AddWithValue("Id", id);

						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine("Продукт успешно удален.");
						}
						else
						{
							Console.WriteLine("Продукт с указанным идентификатором не найден.");
							return Result.Failure<bool>("Продукт с указанным идентификатором не найден.");
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("Ошибка выполнения запроса к базе данных: " + ex.Message);
						return Result.Failure<bool>("Ошибка выполнения запроса к базе данных: " + ex.Message);
					}

					command.ExecuteNonQuery();
				}
			}

			return Result.Success(true);
		}

		public Result<bool> UpdateProduct(ProductModel product)
		{
			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				connection.Open();

				string sql = @"UPDATE public.product 
					SET shortname = @Name, 
						fullname = @FullName, 
						description = @Description, 
						price = @Price, 
						image = @Image, 
						size = @Size, 
						rating = @Rating, 
						count = @Count, 
						instock = @InStock, 
						date = @Date, 
						category = @Category 	   
					WHERE id = @Id";

				using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
				{
					try
					{
						Console.WriteLine("DATE: " + product.Date);

						command.Parameters.AddWithValue("Id", product.Id);
						command.Parameters.AddWithValue("Name", product.ShortName);
						command.Parameters.AddWithValue("FullName", product.FullName);
						command.Parameters.AddWithValue("Description", product.Description);
						command.Parameters.AddWithValue("Price", product.Price);
						command.Parameters.AddWithValue("Image", product.Image);
						command.Parameters.AddWithValue("Size", NpgsqlTypes.NpgsqlDbType.Varchar, product.Size.ToString());
						command.Parameters.AddWithValue("Rating", NpgsqlTypes.NpgsqlDbType.Varchar, product.Rating.ToString());
						command.Parameters.AddWithValue("Count", product.Count);
						command.Parameters.AddWithValue("InStock", product.InStock);
						command.Parameters.AddWithValue("Date", NpgsqlTypes.NpgsqlDbType.Date).Value = product.Date;
						command.Parameters.AddWithValue("Category", NpgsqlTypes.NpgsqlDbType.Varchar, product.Category.ToString());

						command.ExecuteNonQuery();

						// Выполнение команды UPDATE
						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							Console.WriteLine("Продукт успешно изменен.");

							return Result.Success(true);
						}
						else
						{
							Console.WriteLine("Продукт с указанным идентификатором не найден.");
							return Result.Failure<bool>("Продукт с указанным идентификатором не найден.");
						}

					}
					catch (Exception ex)
					{
						Console.WriteLine("Ошибка выполнения запроса к базе данных: " + ex.Message);
						return Result.Failure<bool>("Ошибка выполнения запроса к базе данных: " + ex.Message);
					}

				}
			}
		}

		public ProductModel GetProductById(int id)
		{
			throw new NotImplementedException();
		}

	}
}
