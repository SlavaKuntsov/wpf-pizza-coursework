using System;

using CSharpFunctionalExtensions;

using Npgsql;
using System.Data;
using static Pizza.Abstractions.ProgramAbstraction;
using System.ComponentModel;
using System.Reflection;
using NpgsqlTypes;
using Pizza.MVVM.Model;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pizza.Repository
{
	public class UserRepository
	{
		private string _connectionString;

		public UserRepository(string connection)
		{
			_connectionString = connection;
		}

		public async Task<ObservableCollection<AuthPermissionModel>> GetUnauthorizedEmployees()
		{
			ObservableCollection<AuthPermissionModel> employees = new ObservableCollection<AuthPermissionModel>();

			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_unauthorized_employees", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					using (NpgsqlDataReader reader = (NpgsqlDataReader)await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							AuthPermissionModel employee = new AuthPermissionModel
							{
								Id = reader.GetGuid(0),
								Name = reader.GetString(1),
								Surname = reader.GetString(2),
							};

							employees.Add(employee);
						}
					}
				}
			}

			return employees;
		}

		public async Task<ObservableCollection<AuthPermissionModel>> GetAuthorizedEmployees()
		{
			ObservableCollection<AuthPermissionModel> employees = new ObservableCollection<AuthPermissionModel>();

			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_authorized_employees", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					using (NpgsqlDataReader reader = (NpgsqlDataReader)await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							AuthPermissionModel employee = new AuthPermissionModel
							{
								Id = reader.GetGuid(0),
								Name = reader.GetString(1),
								Surname = reader.GetString(2),
							};

							employees.Add(employee);
						}
					}
				}
			}

			return employees;
		}

		public async Task<Result> AuthorizeEmployee(Guid employeeId)
		{
			//try
			//{
				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					using (NpgsqlCommand command = new NpgsqlCommand("CALL procedures.authorize_employee(@employee_id)", connection))
					{
						//command.CommandType = System.Data.CommandType.StoredProcedure;
						command.Parameters.AddWithValue("employee_id", employeeId);

						await command.ExecuteNonQueryAsync();
					}
				}

				return Result.Success();
			//}
			//catch (Exception ex)
			//{
			//	// Обработка ошибок
			//	return Result.Failure(ex.Message);
			//}
		}

		public async Task<Result> DeauthorizeEmployee(Guid employeeId)
		{
			try
			{
				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					using (NpgsqlCommand command = new NpgsqlCommand("CALL procedures.deauthorize_employee(@employee_id)", connection))
					{
						//command.CommandType = System.Data.CommandType.StoredProcedure;
						command.Parameters.AddWithValue("employee_id", employeeId);

						await command.ExecuteNonQueryAsync();
					}
				}

				return Result.Success();
			}
			catch (Exception ex)
			{
				// Обработка ошибок
				return Result.Failure(ex.Message);
			}
		}

		public async Task<Result> DeleteEmployee(Guid employeeId)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				// Используем правильную команду для вызова процедуры
				using (var command = new NpgsqlCommand("CALL procedures.delete_employee(@employee_id)", connection))
				{
					// Параметры для процедуры
					command.Parameters.AddWithValue("employee_id", NpgsqlTypes.NpgsqlDbType.Uuid, employeeId);

					try
					{
						await command.ExecuteNonQueryAsync();
					}
					catch (Exception ex)
					{
						// Обработка исключений
						return Result.Failure(ex.Message);
					}
				}
			}
			return Result.Success();
		}

		public async Task<Result> UpdateCustomer(Guid customerId, string address)
		{
			try
			{
				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					using (NpgsqlCommand command = new NpgsqlCommand("CALL procedures.update_customer(@customer_id_, @address)", connection))
					{
						command.Parameters.AddWithValue("customer_id_", customerId);
						command.Parameters.AddWithValue("address", address);

						await command.ExecuteNonQueryAsync();
					}
				}

				return Result.Success();
			}
			catch (Exception ex)
			{
				// Обработка ошибок
				return Result.Failure(ex.Message);
			}
		}

		public Result<UserModel> LogIn(string email, string password)
		{
			Console.WriteLine("string email, string password:" + " " + email + " " + password);
			try
			{
				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					NpgsqlCommand command = new NpgsqlCommand("procedures.login", connection);
					command.CommandType = CommandType.StoredProcedure;


					command.Parameters.Add("@email", NpgsqlDbType.Varchar).Value = email;
					command.Parameters.Add("@password", NpgsqlDbType.Varchar).Value = password;


					command.Parameters.Add("@id", NpgsqlDbType.Uuid).Direction = ParameterDirection.Output;
					command.Parameters.Add("@name", NpgsqlDbType.Varchar, 100).Direction = ParameterDirection.Output;
					command.Parameters.Add("@surname", NpgsqlDbType.Varchar, 100).Direction = ParameterDirection.Output;
					command.Parameters.Add("@address", NpgsqlDbType.Varchar, 100).Direction = ParameterDirection.Output;
					command.Parameters.Add("@error", NpgsqlDbType.Varchar, 100).Direction = ParameterDirection.Output;

					command.ExecuteNonQuery();

					string error = command.Parameters["@error"].Value.ToString();

					if (!string.IsNullOrEmpty(error))
					{
						return Result.Failure<UserModel>(error);
					}
					else
					{
						Guid role_id = (Guid)command.Parameters["@id"].Value;
						string role_name = command.Parameters["@name"].Value.ToString();
						string role_surname = command.Parameters["@surname"].Value.ToString();
						string address = command.Parameters["@address"].Value.ToString();

						Console.WriteLine(role_id + " " + role_name + " " + role_surname);

						Console.WriteLine("Пользователь авторизован. ID: " + role_id);

						UserModel user = new UserModel
						{
							Id = role_id,
							Name = role_name,
							Surname = role_surname,
							Email = email,
							Password = password,
							Address = address
						};

						Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ADDRESS: " + user.Address);

						return Result.Success(user);
					}
				}
			}
			catch (InvalidCastException)
			{
				Console.WriteLine("Неверные учетные данные пользователя2.");
				return Result.Failure<UserModel>("Неверные учетные данные пользователя2.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR: " + ex.Message);
				return Result.Failure<UserModel>("Ошибка запроса или соединения.");
			}
		}

		public Result<UserModel> SignUp(AppRoles role, string name, string surname, string email, string password, string address)
		{
			try
			{
				string enumDescriptionRole = GetEnumDescription(role);

				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					NpgsqlCommand command = new NpgsqlCommand("procedures.signup", connection);
					command.CommandType = CommandType.StoredProcedure;

					// Входные параметры
					command.Parameters.Add("@role", NpgsqlDbType.Varchar).Value = enumDescriptionRole;
					command.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = name;
					command.Parameters.Add("@surname", NpgsqlDbType.Varchar).Value = surname;
					command.Parameters.Add("@email", NpgsqlDbType.Varchar).Value = email;
					command.Parameters.Add("@password", NpgsqlDbType.Varchar).Value = password;
					command.Parameters.Add("@address", NpgsqlDbType.Varchar).Value = address;

					// Выходные параметры
					command.Parameters.Add("@role_id", NpgsqlDbType.Uuid).Direction = ParameterDirection.Output;
					command.Parameters.Add("@address_", NpgsqlDbType.Varchar, 100).Direction = ParameterDirection.Output;
					command.Parameters.Add("@error", NpgsqlDbType.Varchar, 100).Direction = ParameterDirection.Output;

					command.ExecuteNonQuery();

					string error = command.Parameters["@error"].Value.ToString();

					if (!string.IsNullOrEmpty(error))
					{
						switch (error)
						{
							case "Invalid email format!":
								return Result.Failure<UserModel>("Неверная форма почты!");
							case "Customer already exists!":
								return Result.Failure<UserModel>("Такой Пользователь уже существует.");
							case "Manager already exists!":
								return Result.Failure<UserModel>("Такой Менеджер уже существует.");
							case "Seller already exists!":
								return Result.Failure<UserModel>("Такой Продавец уже существует.");
							case "Courier already exists!":
								return Result.Failure<UserModel>("Такой Курьер уже существует.");
							default:
								return Result.Failure<UserModel>("Ошибка запроса или соединения.1");
						}
					}
					else
					{
						Guid role_id = (Guid)command.Parameters["@role_id"].Value;
						string role_address = (string)command.Parameters["@address"].Value;

						Console.WriteLine("&&&&&&&&&&&&&&&");
						Console.WriteLine("Role ID: " + role_id);

						UserModel user = new UserModel
						{
							Id = role_id,
							Name = name,
							Surname = surname,
							Email = email,
							Password = password,
							Address = role_address
						};

						return Result.Success(user);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR: " + ex.Message);
				return Result.Failure<UserModel>("Ошибка запроса или соединения.2");
			}
		}

		public AppRoles GetUserRole(Guid id)
		{
			string roleName = null;

			using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
			{
				connection.Open();
				Console.WriteLine(_connectionString);
				using (NpgsqlCommand command = new NpgsqlCommand("SELECT procedures.get_user_role(@id)", connection))
				{
					command.Parameters.AddWithValue("id", id);
					roleName = (string)command.ExecuteScalar();
					Console.WriteLine("STR ROLE ID: " + id.ToString());
					Console.WriteLine("STR ROLE: " + roleName);
				}
			}

			return ConvertToAppRole(roleName);
		}

		private AppRoles ConvertToAppRole(string roleName)
		{
			foreach (AppRoles role in Enum.GetValues(typeof(AppRoles)))
			{
				DescriptionAttribute descriptionAttribute = typeof(AppRoles)
			.GetField(role.ToString())
			.GetCustomAttribute<DescriptionAttribute>();

				if (descriptionAttribute != null && descriptionAttribute.Description == roleName)
				{
					return role;
				}
			}

			// Обработка, если роль не найдена
			// Возвращаемое значение по умолчанию или выбрасывайте исключение, в зависимости от вашей логики
			return AppRoles.Customer; // Возвращаем значения по умолчанию
		}

		private string GetEnumDescription(Enum value)
		{
			FieldInfo field = value.GetType().GetField(value.ToString());
			DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

			return attribute != null ? attribute.Description : value.ToString();
		}
	}
}