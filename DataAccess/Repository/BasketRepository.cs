using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Pizza.Abstractions;
using Pizza.MVVM.Model;

using CSharpFunctionalExtensions;

using Npgsql;
using System.Data;
using System.Threading.Tasks;
using NpgsqlTypes;

namespace Pizza.Repository
{
	public class BasketRepository : IBasketRepository
	{
		private string _connectionString;

		public BasketRepository(string connection)
		{
			_connectionString = connection;
		}

		public async Task<Result<ObservableCollection<ProductModelNew>>> GetBasket(Guid customerId)
		{
			Console.WriteLine("_______________ GetBasket ________________");
			ObservableCollection<ProductModelNew> basket = new ObservableCollection<ProductModelNew>();

			List<int> basketIds = new List<int>();
			List<int> productsIds = new List<int>();
			List<string> names = new List<string>();
			List<string> descriptions = new List<string>();
			List<byte[]> images = new List<byte[]>();
			List<double> prices = new List<double>();
			List<bool> inStocks = new List<bool>();
			List<int> counts = new List<int>();
			List<string> categories = new List<string>();
			List<string> propertyNames = new List<string>();
			List<string> propertyValues = new List<string>();

			try
			{

				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_customer_basket2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;

						// Добавляем параметр для передачи customer_id
						command.Parameters.AddWithValue("customer_id_", customerId);

						using (NpgsqlDataReader reader = (NpgsqlDataReader)await command.ExecuteReaderAsync())
						{
							int previousId = -1;
							while (await reader.ReadAsync())
							{
								// Создаем объект ProductModelNew для каждой записи в результате запроса
								int basket_id = Convert.ToInt32(reader["basket_id"]);
								if (basket_id != previousId)
								{
									productsIds.Add(Convert.ToInt32(reader["product_id"]));
									names.Add(Convert.ToString(reader["product_name"]));
									descriptions.Add(Convert.ToString(reader["product_description"]));
									images.Add((byte[])reader["product_image"]);
									prices.Add(Convert.ToDouble(reader["product_price"]));
									inStocks.Add(Convert.ToBoolean(reader["product_instock"]));
									counts.Add(Convert.ToInt32(reader["product_count"]));
									categories.Add(Convert.ToString(reader["product_category_name"]));

									// Обновляем предыдущий id
									previousId = basket_id;

									basketIds.Add(basket_id);
									Console.WriteLine("ONE");
								}

							}
						}
						using (NpgsqlCommand propertyCommand = new NpgsqlCommand("procedures.get_customer_basket_property", connection))
						{
							propertyCommand.CommandType = CommandType.StoredProcedure;

							// Перебор всех продуктов и получение их свойств
							for (int i = 0; i < basketIds.Count; i++)
							{
								propertyCommand.Parameters.Clear();
								propertyCommand.Parameters.AddWithValue("basket_id", basketIds[i]);

								using (NpgsqlDataReader propertyReader = (NpgsqlDataReader)await propertyCommand.ExecuteReaderAsync())
								{
									while (await propertyReader.ReadAsync())
									{
										string propertyName = Convert.ToString(propertyReader["product_property_name"]);
										string propertyValue = Convert.ToString(propertyReader["product_property_value"]);
										propertyNames.Add(propertyName);
										propertyValues.Add(propertyValue);

										Console.WriteLine("--------------- " + names[i]);
										Console.WriteLine("repository name: " + propertyName);
										Console.WriteLine("repository value: " + propertyValue);
										Console.WriteLine("---------------");
									}
									Console.WriteLine("PROP Count: " + propertyValues.Count);
									ProductModelNew product = new ProductModelNew(basketIds[i], names[i], descriptions[i], images[i], prices[i], inStocks[i], counts[i], categories[i], propertyNames, propertyValues);
									basket.Add(product);
								}
								propertyNames.Clear();
								propertyValues.Clear();
							}
						}
					}
				}
				return Result.Success(basket);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Result.Failure<ObservableCollection<ProductModelNew>>(ex.Message);
			}
		}

		public Result<ProductModelNew> GetBasketProductInfo(Guid customerId, int basketId)
		{
			Console.WriteLine("_______________ GetBasketProductInfo ________________");
			ProductModelNew basket = new ProductModelNew();

			int productId = -1;
			string productName = "";
			string productDescription = "";
			byte[] imageImage = new byte[0];
			double productPrice = 0;
			bool productInStock = false;
			int productCount = 1;
			string productCategoryName = "";

			//try
			//{

			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				connection.Open();

				using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_customer_basket_product", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					// Добавляем параметр для передачи customer_id
					command.Parameters.AddWithValue("customer_id_", customerId);
					command.Parameters.AddWithValue("basket_id_", basketId);

					using (NpgsqlDataReader reader = (NpgsqlDataReader)command.ExecuteReader())
					{
						while (reader.Read())
						{
							// Создаем объект ProductModelNew для каждой записи в результате запроса
							productId = reader.GetInt32(reader.GetOrdinal("product_id"));
							productName = reader.GetString(reader.GetOrdinal("product_name"));
							productDescription = reader.GetString(reader.GetOrdinal("product_description"));
							imageImage = reader.IsDBNull(reader.GetOrdinal("product_image")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("product_image"));
							productPrice = reader.GetDouble(reader.GetOrdinal("product_price"));
							productInStock = reader.GetBoolean(reader.GetOrdinal("product_instock"));
							productCount = reader.GetInt32(reader.GetOrdinal("product_count"));
							productCategoryName = reader.GetString(reader.GetOrdinal("product_category_name"));

							Console.WriteLine("ONE");
						}
					}
					using (NpgsqlCommand propertyCommand = new NpgsqlCommand("procedures.get_customer_basket_property", connection))
					{
						propertyCommand.CommandType = CommandType.StoredProcedure;

						// Перебор всех продуктов и получение их свойств
						propertyCommand.Parameters.Clear();
						propertyCommand.Parameters.AddWithValue("basket_id", basketId);

						List<string> propertyNames = new List<string>();
						List<string> propertyValues = new List<string>();

						using (NpgsqlDataReader propertyReader = (NpgsqlDataReader)propertyCommand.ExecuteReader())
						{
							while (propertyReader.Read())
							{
								string propertyName = Convert.ToString(propertyReader["product_property_name"]);
								string propertyValue = Convert.ToString(propertyReader["product_property_value"]);
								propertyNames.Add(propertyName);
								propertyValues.Add(propertyValue);

								Console.WriteLine("--------------- " + basketId);
								Console.WriteLine("repository name: " + propertyName);
								Console.WriteLine("repository value: " + propertyValue);
								Console.WriteLine("---------------");
							}
							Console.WriteLine("PROP Count: " + propertyValues.Count);
							basket = new ProductModelNew(basketId, productName, productDescription, imageImage, productPrice, productInStock, productCount, productCategoryName, propertyNames, propertyValues);

							Console.WriteLine("AAAAAAAAAAAAAAA SIZE " + basket.Size);
							Console.WriteLine("AAAAAAAAAAAAAAA SIZE " + basket.Type);
						}
						propertyNames.Clear();
						propertyValues.Clear();
					}
				}
			}
			return Result.Success(basket);

			//}
			//catch (Exception)
			//{

			//}
		}


		public async Task<Result> AddInBasket(ProductModelNew product, Guid customerId)
		{
			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				connection.Open();

				int basketId = -1;
				int productPropertyId = -1; // Инициализация переменной для хранения идентификатора свойства продукта

				// Вызываем хранимую процедуру для добавления продукта в корзину
				using (NpgsqlCommand basketCommand = new NpgsqlCommand("procedures.add_in_basket2", connection))
				{
					basketCommand.CommandType = CommandType.StoredProcedure;
					basketCommand.Parameters.AddWithValue("customer_id_", customerId);
					basketCommand.Parameters.AddWithValue("product_id_", product.Id); // Предполагается, что у объекта ProductModelNew есть свойство Id
					basketCommand.Parameters.AddWithValue("count", product.Count); // Предполагается, что у объекта ProductModelNew есть свойство Count

					// Добавляем выходной параметр для получения ошибки
					basketCommand.Parameters.Add("basket_id_", NpgsqlDbType.Integer, 100).Direction = ParameterDirection.Output;
					basketCommand.Parameters.Add("error", NpgsqlDbType.Varchar, 100).Direction = ParameterDirection.Output;

					await basketCommand.ExecuteNonQueryAsync();

					// Получаем значение ошибки из выходного параметра
					string error = basketCommand.Parameters["error"].Value.ToString();
					Console.WriteLine("************************* error: " + error);

					if (!string.IsNullOrEmpty(error))
					{
						return Result.Failure<string>(error);
					}
					basketId = (int)basketCommand.Parameters["basket_id_"].Value;
					Console.WriteLine("************************* basketId: " + basketId);
				}

				for (int i = 0; i < product.PropertyName.Count; i++)
				{
					Console.WriteLine("ЦИКЛ " + i);
					// Вызываем хранимую процедуру для добавления свойства продукта
					using (NpgsqlCommand propertyCommand = new NpgsqlCommand("procedures.add_property_for_product2", connection))
					{
						propertyCommand.CommandType = CommandType.StoredProcedure;
						propertyCommand.Parameters.AddWithValue("basket_id_", basketId); // Имя продукта в качестве имени свойства
						propertyCommand.Parameters.AddWithValue("name", product.PropertyName[i]); // Имя продукта в качестве имени свойства
						propertyCommand.Parameters.AddWithValue("value", product.PropertyValue[i]); // Описание продукта в качестве значения свойства

						// Добавляем выходной параметр для получения идентификатора добавленного свойства
						propertyCommand.Parameters.Add("inserted_property_id", NpgsqlDbType.Integer).Direction = ParameterDirection.Output;

						await propertyCommand.ExecuteNonQueryAsync();

						// Получаем идентификатор добавленного свойства из выходного параметра
						productPropertyId = (int)propertyCommand.Parameters["inserted_property_id"].Value;
					}

					// Проверяем, было ли добавлено свойство продукта
					if (productPropertyId == -1)
					{
						return Result.Failure<string>("Failed to add product property.");
					}
				}

			}

			return Result.Success();
		}


		public async Task<Result> UpdateProductInBasket(ProductModelNew product)
		{
			Console.WriteLine("_______________ UpdateProductInBasket ________________");
			//ProductModelNew basket = new ProductModelNew();

			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				connection.Open();

				for (int i = 0; i < product.PropertyName.Count; i++)
				{
					Console.WriteLine("ЦИКЛ " + i);
					// Вызываем хранимую процедуру для добавления свойства продукта
					using (NpgsqlCommand propertyCommand = new NpgsqlCommand("procedures.update_basket_product2", connection))
					{
						propertyCommand.CommandType = CommandType.StoredProcedure;
						propertyCommand.Parameters.AddWithValue("basket_id_", product.Id); // Имя продукта в качестве имени свойства
						propertyCommand.Parameters.AddWithValue("name", product.PropertyName[i]); // Имя продукта в качестве имени свойства
						propertyCommand.Parameters.AddWithValue("value", product.PropertyValue[i]); // Описание продукта в качестве значения свойства
						propertyCommand.Parameters.AddWithValue("count", product.Count);

						// Добавляем выходной параметр для получения идентификатора добавленного свойства
						propertyCommand.Parameters.Add("error", NpgsqlDbType.Varchar, 100).Direction = ParameterDirection.Output;

						await propertyCommand.ExecuteNonQueryAsync();

						// Получаем значение ошибки из выходного параметра
						string error = propertyCommand.Parameters["error"].Value.ToString();

						if (!string.IsNullOrEmpty(error))
						{
							return Result.Failure<string>(error);
						}
					}
				}
			}
			return Result.Success();
		}

		public async Task<Result> DeleteFromBasket(int basketId)
		{
			//try
			//{
			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (NpgsqlCommand command = new NpgsqlCommand("procedures.delete_from_basket", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("basket_id_", basketId);

					// Добавляем выходной параметр для получения ошибки
					command.Parameters.Add("error", NpgsqlDbType.Varchar, 100).Direction = ParameterDirection.Output;

					await command.ExecuteNonQueryAsync();

					// Получаем значение ошибки из выходного параметра
					string error = command.Parameters["error"].Value.ToString();

					if (!string.IsNullOrEmpty(error))
					{
						return Result.Failure<string>(error);
					}
				}
			}

			return Result.Success();
			//}
			//catch (Exception ex)
			//{
			//	return Result.Failure(ex.Message);
			//}
		}


		//public async Task<Result> AddInBasket2(ProductModelNew product, Guid customerId)
		//{
		//	string result = string.Empty;

		//	using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
		//	{
		//		connection.Open();
		//		using (NpgsqlCommand command = new NpgsqlCommand("procedures.add_in_basket", connection))
		//		{
		//			command.CommandType = CommandType.Text;
		//			command.CommandText = @"
		//                  SELECT *
		//				FROM procedures.add_in_basket(
		//					@CustomerId,
		//					@ProductId,
		//					@Count,
		//					@ProductProperties
		//				)
		//			";

		//			command.Parameters.AddWithValue("CustomerId", customerId);
		//			command.Parameters.AddWithValue("ProductId", product.Id);
		//			command.Parameters.AddWithValue("Count", product.Count);

		//			//foreach (var prop in product.Properties)
		//			//{
		//			//	productPropertiesTable.Rows.Add(DBNull.Value, DBNull.Value, prop.PropertyName, prop.PropertyValue);
		//			//}

		//			// Convert List<ProductProperty> to array of composite type
		//			var productPropertiesArray = new List<(string, string)>();

		//			for (int i = 0; i < product.PropertyName.Count; i++)
		//			{
		//				productPropertiesArray.Add((product.PropertyName[i], product.PropertyValue[i]));
		//			}

		//			var npgsqlArray = new NpgsqlParameter("ProductProperties", NpgsqlDbType.Array | NpgsqlDbType.Composite)
		//			{
		//				Value = productPropertiesArray.ToArray(),
		//				NpgsqlDbType = NpgsqlDbType.Array | NpgsqlDbType.Composite
		//			};
		//			command.Parameters.Add(npgsqlArray);

		//			using (var reader = await command.ExecuteReaderAsync())
		//			{
		//				if (await reader.ReadAsync())
		//				{
		//					result = reader.GetString(0);
		//				}
		//			}
		//		}
		//	}

		//	return Result.Success(result);
		//}
	}
}
