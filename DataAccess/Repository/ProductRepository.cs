using System;
using System.Collections.ObjectModel;

using Pizza.Abstractions;
using Pizza.MVVM.Model;

using CSharpFunctionalExtensions;

using Npgsql;
using System.Data;
using System.Threading.Tasks;
using NpgsqlTypes;
using Pizza.Manager;

namespace Pizza.Repository
{
	public class ProductRepository : IProductRepository
	{
		private string _connectionString;

		public ProductRepository(string connection)
		{
			_connectionString = connection;
		}

		public async Task<Result<ObservableCollection<ProductModelNew>>> GetAllProductsPreview()
		{
			Console.WriteLine("_______________ GetAllProductsPreview ________________");
			//MessageBox.Show("GetAllProductsPreview");
			ObservableCollection<ProductModelNew> products = new ObservableCollection<ProductModelNew>();

			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				connection.Open();

				using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_all_products_preview_limit", connection))
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
							//if (productName == "img")
							//{
							//var product = ProductPreviewModel.Create(productId, productName, productDescription, imageBytes, productPrice, productInStock, productCategoryName);
							//products.Add(product.Value);
							ProductModelNew product = new ProductModelNew(productId, productName, productDescription, imageBytes, productPrice, productInStock, productCategoryName);
							products.Add(product);
							//}



							//var product = ProductPreviewModel.Create(productId, productName, productDescription, imageBytes, productPrice, productInStock, productCategoryName);

						}
					}
				}
			}

			return Result.Success(products);
		}

		public async Task<Result<ObservableCollection<ProductModelNew>>> GetProductPage()
		{
			Console.WriteLine("_______________ GetProductPage ________________");
			ObservableCollection<ProductModelNew> products = new ObservableCollection<ProductModelNew>();
			//try
			//{
			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_paged_data", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					CatalogManager catalogManager = CatalogManager.Instance;

					Console.WriteLine("catalogManager.CurrentPage: " + catalogManager.CurrentPage);

					command.Parameters.Add(new NpgsqlParameter("page_number", NpgsqlDbType.Integer) { Value = catalogManager.CurrentPage });
					command.Parameters.Add(new NpgsqlParameter("page_size", NpgsqlDbType.Integer) { Value = catalogManager.ProductOnPage });

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
							string error = reader.IsDBNull(reader.GetOrdinal("error")) ? "" : reader.GetString(reader.GetOrdinal("error"));


							Console.WriteLine("NAME: " + productName);


							if (!string.IsNullOrEmpty(error))
							{
								Console.WriteLine(error);
								return Result.Failure<ObservableCollection<ProductModelNew>>("Неправильный номер страницы!");
							}
							else
							{
								ProductModelNew product = new ProductModelNew(productId, productName, productDescription, imageBytes, productPrice, productInStock, productCategoryName);
								products.Add(product);
							}
						}
					}
				}
			}

			return Result.Success(products);
			//}
			//catch (Exception ex)
			//{
			//	// Обработка ошибки
			//	return Result.Failure<ObservableCollection<ProductModelNew>>(ex.Message);
			//}
		}


		public Result<int> GetProductsCount()
		{
			//try
			//{
				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_products_count", connection))
					{
						command.CommandType = CommandType.StoredProcedure;

						int count = (int) command.ExecuteScalar();
						Console.WriteLine("count: " + count);
						return Result.Success(count);
					}
				}
			//}
			//catch (Exception ex)
			//{
			//	// Обработка ошибок
			//	return Result.Failure<int>(ex.Message);
			//}
		}


		public async Task<Result> AddProduct(ProductModelNew product)
		{
			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				//for (int i = 1; i <= 10000; i++)
				//{
				using (var transaction = connection.BeginTransaction())
				{
					int product_id;
					using (NpgsqlCommand command = new NpgsqlCommand("procedures.add_product", connection))
					{
						command.CommandType = CommandType.StoredProcedure;

						Console.WriteLine(Convert.ToBase64String(product.Image));

						command.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Varchar) { Value = product.Name });
						command.Parameters.Add(new NpgsqlParameter("description", NpgsqlDbType.Text) { Value = product.Description });
						command.Parameters.Add(new NpgsqlParameter("image", NpgsqlDbType.Bytea) { Value = product.Image });
						command.Parameters.Add(new NpgsqlParameter("price", NpgsqlDbType.Double) { Value = product.Price });
						command.Parameters.Add(new NpgsqlParameter("instock", NpgsqlDbType.Boolean) { Value = product.InStock });
						command.Parameters.Add(new NpgsqlParameter("category", NpgsqlDbType.Varchar) { Value = product.GetCategoryString(product.Category) });

						var idParam = new NpgsqlParameter("id", NpgsqlDbType.Integer);
						idParam.Direction = ParameterDirection.Output;
						command.Parameters.Add(idParam);

						var errorParam = new NpgsqlParameter("error", NpgsqlDbType.Varchar, 100);
						errorParam.Direction = ParameterDirection.Output;
						command.Parameters.Add(errorParam);

						await command.ExecuteNonQueryAsync();

						string error = errorParam.Value.ToString();

						if (!string.IsNullOrEmpty(error))
						{
							Console.WriteLine(error);
							switch (error)
							{
								case "Category not found!":
									return Result.Failure<UserModel>("Такой категории не существует!");
								case "Product already exists!":
									return Result.Failure<UserModel>("Такой Продукт уже существует.");
								default:
									return Result.Failure<UserModel>("Ошибка запроса или соединения.1");
							}
						}
						else
						{
							product_id = (int)idParam.Value;
							//productId = (int)command.Parameters["@id"].Value;

							Console.WriteLine("&&&&&&&&&&&&&&&");
							Console.WriteLine("productId: " + product_id);
						}
					}
					for (int j = 0; j < product.PropertyName.Count; j++)
					{
						using (NpgsqlCommand command = new NpgsqlCommand("CALL procedures.add_property_for_product(@product_id_, @name, @value)", connection))
						{
							command.Parameters.Add(new NpgsqlParameter("@product_id_", NpgsqlDbType.Integer) { Value = product_id });
							command.Parameters.Add(new NpgsqlParameter("@name", NpgsqlDbType.Varchar) { Value = product.PropertyName[j] });
							command.Parameters.Add(new NpgsqlParameter("@value", NpgsqlDbType.Varchar) { Value = product.PropertyValue[j] });

							await command.ExecuteNonQueryAsync();
						}
					}
					// Здесь вы можете выполнить дополнительные операции или вызвать другие хранимые процедуры.
					// Например, в вашем случае вызов `procedures.add_property_for_product`.

					transaction.Commit();
					Console.WriteLine("Transaction committed successfully.");
				}
				//}
			}
			return Result.Success();
		}


		//public async Task<Result> AddProduct(ProductModelNew product)
		//{
		//	Console.WriteLine("Name: " + product.Name);
		//	Console.WriteLine("Description: " + product.Description);
		//	Console.WriteLine("Price: " + product.Price);
		//	Console.WriteLine("Image: " + Convert.ToBase64String(product.Image));
		//	Console.WriteLine("InStock: " + product.InStock);
		//	//try
		//	//{
		//	using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
		//	{
		//		await connection.OpenAsync();

		//		// Начать транзакцию
		//		using (var transaction = connection.BeginTransaction())
		//		{
		//			//try
		//			//{
		//			// Выполнить первую функцию add_product
		//			int productId;
		//			string error = "";

		//			using (var command = new NpgsqlCommand("procedures.add_product", connection))
		//			{
		//				command.CommandType = CommandType.StoredProcedure;

		//				//command.Parameters.AddWithValue("@name", product.Name);	
		//				//command.Parameters.AddWithValue("@description", product.Description);
		//				//command.Parameters.AddWithValue("@image", product.Image);
		//				//command.Parameters.AddWithValue("@price", product.Price);
		//				//command.Parameters.AddWithValue("@inStock", product.InStock);
		//				//command.Parameters.AddWithValue("@category", product.GetCategoryString(product.Category));
		//				command.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = product.Name;
		//				command.Parameters.Add("@description", NpgsqlDbType.Text).Value = product.Description;
		//				command.Parameters.Add("@image", NpgsqlDbType.Bytea).Value = product.Image;
		//				command.Parameters.Add("@price", NpgsqlDbType.Double).Value = product.Price;
		//				command.Parameters.Add("@inStock", NpgsqlDbType.Boolean).Value = product.InStock;
		//				command.Parameters.Add("@category", NpgsqlDbType.Varchar).Value = (string)product.GetCategoryString(product.Category);

		//				command.Parameters.Add("@id", NpgsqlDbType.Integer).Direction = ParameterDirection.Output;
		//				command.Parameters.Add("@error", NpgsqlDbType.Varchar, 100).Direction = ParameterDirection.Output;

		//				await command.ExecuteNonQueryAsync();

		//				error = command.Parameters["@error"].Value.ToString();

		//				if (!string.IsNullOrEmpty(error))
		//				{
		//					switch (error)
		//					{
		//						case "Category not found!":
		//							return Result.Failure<UserModel>("Такой категории не существует!");
		//						case "Product already exists!":
		//							return Result.Failure<UserModel>("Такой Продукт уже существует.");
		//						default:
		//							return Result.Failure<UserModel>("Ошибка запроса или соединения.1");
		//					}
		//				}
		//				else
		//				{
		//					productId = (int)command.Parameters["@id"].Value;

		//					Console.WriteLine("&&&&&&&&&&&&&&&");
		//					Console.WriteLine("productId: " + productId);
		//				}
		//			}


		//			//NpgsqlCommand command1 = new NpgsqlCommand("select procedures.add_product", connection);
		//			//command1.CommandType = CommandType.StoredProcedure;

		//			//command1.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = product.Name;
		//			//command1.Parameters.Add("@description", NpgsqlDbType.Text).Value = product.Description;
		//			//command1.Parameters.Add("@image", NpgsqlDbType.Bytea).Value = product.Image;
		//			//command1.Parameters.Add("@price", NpgsqlDbType.Double).Value = product.Price;
		//			//command1.Parameters.Add("@inStock", NpgsqlDbType.Boolean).Value = product.InStock;
		//			//command1.Parameters.Add("@category", NpgsqlDbType.Varchar).Value = (string)product.GetCategoryString(product.Category);

		//			//command1.Parameters.Add("@id", NpgsqlDbType.Integer).Direction = ParameterDirection.Output;
		//			//command1.Parameters.Add("@error", NpgsqlDbType.Varchar, 100).Direction = ParameterDirection.Output;

		//			////await connection.OpenAsync();
		//			//command1.ExecuteNonQuery();
		//			//await command1.ExecuteNonQueryAsync();

		//			//string error = command1.Parameters["@error"].Value.ToString();



		//			// Выполнить вторую функцию add_property_for_product
		//			for (int j = 0; j < product.PropertyValue.Count; j++)
		//			{
		//				var command2 = new NpgsqlCommand("CALL procedures.add_property_for_product(@product_id, @name, @value)", connection);
		//				command2.CommandType = CommandType.StoredProcedure;
		//				command2.Parameters.Add("@product_id", NpgsqlDbType.Integer).Value = productId;
		//				command2.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = product.PropertyName[j];
		//				command2.Parameters.Add("@value", NpgsqlDbType.Varchar).Value = product.PropertyValue[j];

		//				await command2.ExecuteNonQueryAsync();
		//			}

		//			// Завершить транзакцию
		//			transaction.Commit();
		//			Console.WriteLine("Transaction committed successfully.");
		//			//}
		//			//catch (Exception ex)
		//			//{
		//			//	// Если произошла ошибка, откатить транзакцию
		//			//	transaction.Rollback();
		//			//	Console.WriteLine("Transaction rolled back. Error: " + ex.Message);
		//			//}
		//		}
		//	}
		//	return Result.Success();
		//	//}
		//	//catch (Exception ex)
		//	//{
		//	//	// Обработка ошибок
		//	//	return Result.Failure("Failed to add product: " + ex.Message);
		//	//}
		//}




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

	public class Prop
	{
		public string PropName { get; set; }
		public string PropValue { get; set; }
	}
}
