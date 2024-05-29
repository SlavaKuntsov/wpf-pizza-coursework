using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

using CSharpFunctionalExtensions;

using Npgsql;

using NpgsqlTypes;

using Pizza.Abstractions;
using Pizza.Manager;
using Pizza.MVVM.Model;

using static Pizza.Abstractions.ProductAbstraction;

namespace Pizza.DataAccess.Repository
{
	public class OrderRepository
	{
		private string _connectionString;

		public OrderRepository(string connection)
		{
			_connectionString = connection;
		}


		//public Result<ProductModelNew> GetCustomerOrder(Guid customerId)
		//{
		//	Console.WriteLine("_______________ GetCustomerOrder ________________");
		//	ProductModelNew product = null;

		//	using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
		//	{
		//		connection.Open();

		//		using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_customer_order", connection))
		//		{
		//			command.CommandType = CommandType.StoredProcedure;
		//			command.Parameters.AddWithValue("customer_id", customerId);

		//			using (NpgsqlDataReader reader = command.ExecuteReader())
		//			{
		//				if (reader.Read()) // Читаем первую строку
		//				{
		//					int id = Convert.ToInt32(reader["product_id_"]);
		//					string name = Convert.ToString(reader["product_name"]);
		//					string description = Convert.ToString(reader["product_description"]);
		//					byte[] image = (byte[])reader["product_image"];
		//					double price = Convert.ToDouble(reader["product_price"]);
		//					bool inStock = Convert.ToBoolean(reader["product_instock"]);
		//					string category = Convert.ToString(reader["product_category_name"]);


		//					string error = reader.IsDBNull(reader.GetOrdinal("error")) ? "" : reader.GetString(reader.GetOrdinal("error"));

		//					if (!string.IsNullOrEmpty(error))
		//					{
		//						Console.WriteLine(error);
		//						return Result.Failure<ProductModelNew>(error);
		//					}

		//					Console.WriteLine("NAMEEEEEEE: " + name);

		//					product = new ProductModelNew(id, name, description, image, price, inStock, category);
		//				}
		//			}
		//		}
		//	}

		//	return Result.Success(product);
		//}


		//public async Task<Result<ObservableCollection<ProductModelNew>>> GetCustomerOrder(Guid customerId)
		//{
		//	Console.WriteLine("_______________ GetCustomerOrder ________________");
		//	ObservableCollection<OrderModel> orders = new ObservableCollection<OrderModel>();

		//	int orderId = -1;
		//	string orderStatus = "";
		//	string orderDate = "";
		//	string orderPrice = "";
		//	bool orderDelivery;
		//	//try
		//	//{
		//	using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
		//	{
		//		await connection.OpenAsync();

		//		using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_customer_order", connection))
		//		{
		//			command.CommandType = CommandType.StoredProcedure;

		//			command.Parameters.Add(new NpgsqlParameter("customer_id", NpgsqlDbType.Uuid) { Value = customerId });

		//			using (NpgsqlDataReader reader = (NpgsqlDataReader)await command.ExecuteReaderAsync())
		//			{
		//				while (await reader.ReadAsync())
		//				{
		//					orderId = reader.GetInt32(reader.GetOrdinal("order_id"));
		//					orderStatus = reader.GetString(reader.GetOrdinal("order_status"));
		//					orderDate = reader.GetString(reader.GetOrdinal("order_date"));
		//					orderPrice = reader.GetString(reader.GetOrdinal("order_price"));
		//					orderDelivery = reader.GetBoolean(reader.GetOrdinal("order_delivery"));

		//					string productName = reader.GetString(reader.GetOrdinal("product_name"));
		//					string productDescription = reader.GetString(reader.GetOrdinal("product_description"));
		//					byte[] imageBytes = reader.IsDBNull(reader.GetOrdinal("product_image")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("product_image"));
		//					double productPrice = reader.GetDouble(reader.GetOrdinal("product_price"));
		//					bool productInStock = reader.GetBoolean(reader.GetOrdinal("product_instock"));
		//					string productCategoryName = reader.GetString(reader.GetOrdinal("product_category_name"));
		//					string error = reader.IsDBNull(reader.GetOrdinal("error")) ? "" : reader.GetString(reader.GetOrdinal("error"));

		//					List<ProductModelNew> orderProducts = new List<ProductModelNew>();


		//					Console.WriteLine("NAME: " + productName);


		//					if (!string.IsNullOrEmpty(error))
		//					{
		//						Console.WriteLine(error);
		//						return Result.Failure<ObservableCollection<ProductModelNew>>("Неправильный номер страницы!");
		//					}
		//					else
		//					{
		//						ProductModelNew product = new ProductModelNew(productId, productName, productDescription, imageBytes, productPrice, productInStock, productCategoryName);
		//						orders.Add(product);
		//					}
		//				}
		//			}
		//		}
		//	}

		//	return Result.Success(orders);
		//	//}
		//	//catch (Exception ex)
		//	//{
		//	//	// Обработка ошибки
		//	//	return Result.Failure<ObservableCollection<ProductModelNew>>(ex.Message);
		//	//}
		//}
		public async Task<Result> AddCustomerOrder(Guid customerId, bool isDelivery)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (NpgsqlCommand command = new NpgsqlCommand("CALL procedures.add_order(@customer_id_, @delivery)", connection))
				{

					Console.WriteLine("_____________ " + customerId);
					Console.WriteLine("_____________ " + isDelivery);

					// Параметры для процедуры
					command.Parameters.AddWithValue("customer_id_", NpgsqlTypes.NpgsqlDbType.Uuid, customerId);
					command.Parameters.AddWithValue("delivery", NpgsqlTypes.NpgsqlDbType.Boolean, isDelivery);


					await command.ExecuteNonQueryAsync();
				}
			}
			return Result.Success();
		}

		public async Task<Result<ObservableCollection<OrderModel>>> GetCustomerOrders(Guid customerId)
		{
			var orders = new Dictionary<Guid, OrderModel>();

			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_customer_order", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.Add(new NpgsqlParameter("customer_id", NpgsqlDbType.Uuid) { Value = customerId });

					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							Guid orderId = reader.GetGuid(reader.GetOrdinal("order_id"));
							//string error = reader.IsDBNull(reader.GetOrdinal("error")) ? "" : reader.GetString(reader.GetOrdinal("error"));

							//if (!string.IsNullOrEmpty(error))
							//{
							//	Console.WriteLine(error);
							//	return Result.Failure<ObservableCollection<OrderModel>>(error);
							//}
							//else
							//{
							if (!orders.ContainsKey(orderId))
							{
								string orderStatus = reader.GetString(reader.GetOrdinal("order_status"));
								DateTime orderDate = reader.GetDateTime(reader.GetOrdinal("order_date"));
								double orderPrice = reader.GetDouble(reader.GetOrdinal("order_price"));
								bool orderDelivery = reader.GetBoolean(reader.GetOrdinal("order_delivery"));

								OrderModel order = new OrderModel(orderId, orderStatus, orderDate, orderPrice, orderDelivery, "", null, null, new List<ProductModelNew>());

								orders[orderId] = order;
							}

							string productName = reader.GetString(reader.GetOrdinal("product_name"));
							byte[] imageBytes = reader.IsDBNull(reader.GetOrdinal("product_image")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("product_image"));
							double productPrice = reader.GetDouble(reader.GetOrdinal("product_price"));
							int productCount = reader.GetInt32(reader.GetOrdinal("product_count"));

							//string productCategoryName = reader.GetString(reader.GetOrdinal("product_category_name"));

							ProductModelNew product = new ProductModelNew(productName, imageBytes, productPrice, productCount);

							orders[orderId].OrderProducts.Add(product);
						}
						//}
					}
				}
			}

			return new ObservableCollection<OrderModel>(orders.Values);

		}

		public async Task<Result<ObservableCollection<OrderModel>>> GetAllOrders()
		{
			var orders = new Dictionary<Guid, OrderModel>();

			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_all_orders", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					using (var reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							Guid orderId = reader.GetGuid(reader.GetOrdinal("order_id"));

							if (!orders.ContainsKey(orderId))
							{
								string orderStatus = reader.GetString(reader.GetOrdinal("order_status"));
								DateTime orderDate = reader.GetDateTime(reader.GetOrdinal("order_date"));
								double orderPrice = reader.GetDouble(reader.GetOrdinal("order_price"));
								bool orderDelivery = reader.GetBoolean(reader.GetOrdinal("order_delivery"));
								string orderAddress = reader.GetString(reader.GetOrdinal("customer_address"));
								Guid? sellerId = reader.IsDBNull(reader.GetOrdinal("fk_seller_id"))
									? (Guid?)null
									: reader.GetGuid(reader.GetOrdinal("fk_seller_id"));
								Guid? courierId = reader.IsDBNull(reader.GetOrdinal("fk_courier_id"))
									? (Guid?)null
									: reader.GetGuid(reader.GetOrdinal("fk_courier_id"));


								OrderModel order = new OrderModel(orderId, orderStatus, orderDate, orderPrice, orderDelivery, orderAddress, sellerId, courierId, new List<ProductModelNew>());

								orders[orderId] = order;
							}

							string productName = reader.GetString(reader.GetOrdinal("product_name"));
							byte[] imageBytes = reader.IsDBNull(reader.GetOrdinal("product_image")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("product_image"));
							double productPrice = reader.GetDouble(reader.GetOrdinal("product_price"));
							int productCount = reader.GetInt32(reader.GetOrdinal("product_count"));

							ProductModelNew product = new ProductModelNew(productName, imageBytes, productPrice, productCount);

							orders[orderId].OrderProducts.Add(product);
						}
					}
				}
			}

			return Result.Success(new ObservableCollection<OrderModel>(orders.Values));
		}

		public async Task<Result<ObservableCollection<OrderModel>>> GetCourierOrders()
		{
			var orders = new Dictionary<Guid, OrderModel>();
			try
			{

				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_courier_orders", connection))
					{
						command.CommandType = CommandType.StoredProcedure;

						using (var reader = await command.ExecuteReaderAsync())
						{
							while (await reader.ReadAsync())
							{
								Guid orderId = reader.GetGuid(reader.GetOrdinal("order_id"));

								if (!orders.ContainsKey(orderId))
								{
									string orderStatus = reader.GetString(reader.GetOrdinal("order_status"));
									DateTime orderDate = reader.GetDateTime(reader.GetOrdinal("order_date"));
									double orderPrice = reader.GetDouble(reader.GetOrdinal("order_price"));
									bool orderDelivery = reader.GetBoolean(reader.GetOrdinal("order_delivery"));
									string orderAddress = reader.GetString(reader.GetOrdinal("customer_address"));
									Guid? sellerId = reader.IsDBNull(reader.GetOrdinal("fk_seller_id"))
									? (Guid?)null
									: reader.GetGuid(reader.GetOrdinal("fk_seller_id"));
									Guid? courierId = reader.IsDBNull(reader.GetOrdinal("fk_courier_id"))
									? (Guid?)null
									: reader.GetGuid(reader.GetOrdinal("fk_courier_id"));


									OrderModel order = new OrderModel(orderId, orderStatus, orderDate, orderPrice, orderDelivery, orderAddress, sellerId, courierId, new List<ProductModelNew>());

									orders[orderId] = order;
								}

								string productName = reader.GetString(reader.GetOrdinal("product_name"));
								byte[] imageBytes = reader.IsDBNull(reader.GetOrdinal("product_image")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("product_image"));
								double productPrice = reader.GetDouble(reader.GetOrdinal("product_price"));
								int productCount = reader.GetInt32(reader.GetOrdinal("product_count"));

								ProductModelNew product = new ProductModelNew(productName, imageBytes, productPrice, productCount);

								orders[orderId].OrderProducts.Add(product);
							}
						}
					}
				}
				return Result.Success(new ObservableCollection<OrderModel>(orders.Values));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Result.Failure<ObservableCollection<OrderModel>>(ex.Message);
			}
		}


		public async Task<Result> CancelOrder(Guid orderId)
		{
			try
			{
				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					using (NpgsqlCommand command = new NpgsqlCommand("CALL procedures.cancel_order(@order_id_)", connection))
					{
						command.Parameters.AddWithValue("order_id_", orderId);

						await command.ExecuteNonQueryAsync();

						// Получаем значение ошибки из выходного параметра
						//string error = command.Parameters["error"].Value.ToString();

						//if (!string.IsNullOrEmpty(error))
						//{
						//	return Result.Failure<string>(error);
						//}
					}
				}

				return Result.Success();
			}
			catch (Exception ex)
			{
				return Result.Failure(ex.Message);
			}
		}

		public async Task<Result> AcceptOrder(Guid orderId, Guid customerId)
		{
			try
			{
				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					using (NpgsqlCommand command = new NpgsqlCommand("CALL procedures.accept_order(@order_id_, @seller_id_)", connection))
					{
						command.Parameters.AddWithValue("order_id_", orderId);
						command.Parameters.AddWithValue("seller_id_", customerId);

						await command.ExecuteNonQueryAsync();

						// Получаем значение ошибки из выходного параметра
						//string error = command.Parameters["error"].Value.ToString();

						//if (!string.IsNullOrEmpty(error))
						//{
						//	return Result.Failure<string>(error);
						//}
					}
				}

				return Result.Success();
			}
			catch (Exception ex)
			{
				return Result.Failure(ex.Message);
			}
		}

		public async Task<Result> CompleteOrder(Guid orderId, Guid customerId)
		{
			try
			{
				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					using (NpgsqlCommand command = new NpgsqlCommand("CALL procedures.complete_order(@order_id_, @seller_id_)", connection))
					{
						command.Parameters.AddWithValue("order_id_", orderId);
						command.Parameters.AddWithValue("seller_id_", customerId);

						await command.ExecuteNonQueryAsync();

						// Получаем значение ошибки из выходного параметра
						//string error = command.Parameters["error"].Value.ToString();

						//if (!string.IsNullOrEmpty(error))
						//{
						//	return Result.Failure<string>(error);
						//}
					}
				}

				return Result.Success();
			}
			catch (Exception ex)
			{
				return Result.Failure(ex.Message);
			}
		}

		public async Task<Result> DeliverOrder(Guid orderId, Guid courierId)
		{
			try
			{
				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					using (NpgsqlCommand command = new NpgsqlCommand("CALL procedures.deliver_order(@order_id_, @courier_id_)", connection))
					{
						command.Parameters.AddWithValue("order_id_", orderId);
						command.Parameters.AddWithValue("courier_id_", courierId);

						await command.ExecuteNonQueryAsync();

						// Получаем значение ошибки из выходного параметра
						//string error = command.Parameters["error"].Value.ToString();

						//if (!string.IsNullOrEmpty(error))
						//{
						//	return Result.Failure<string>(error);
						//}
					}
				}

				return Result.Success();
			}
			catch (Exception ex)
			{
				return Result.Failure(ex.Message);
			}
		}

		public async Task<Result> CompleteDeliverOrder(Guid orderId, Guid courierId)
		{
			try
			{
				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					using (NpgsqlCommand command = new NpgsqlCommand("CALL procedures.complete_deliver_order(@order_id_, @courier_id_)", connection))
					{
						command.Parameters.AddWithValue("order_id_", orderId);
						command.Parameters.AddWithValue("courier_id_", courierId);

						await command.ExecuteNonQueryAsync();

						// Получаем значение ошибки из выходного параметра
						//string error = command.Parameters["error"].Value.ToString();

						//if (!string.IsNullOrEmpty(error))
						//{
						//	return Result.Failure<string>(error);
						//}
					}
				}

				return Result.Success();
			}
			catch (Exception ex)
			{
				return Result.Failure(ex.Message);
			}
		}
	}
}
