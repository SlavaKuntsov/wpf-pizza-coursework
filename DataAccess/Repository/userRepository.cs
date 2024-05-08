using System;

using CSharpFunctionalExtensions;

using Npgsql;
using System.Data;
using static Pizza.Abstractions.ProgramAbstraction;
using System.ComponentModel;
using System.Reflection;
using NpgsqlTypes;

namespace Pizza.Repository
{
	public class UserRepository : IUserRepository
	{
		private string _connectionString;

		public UserRepository(string connection)
		{
			_connectionString = connection;
		}

		public Result<Guid> LogIn(string email, string password)
		{
			Console.WriteLine("string email, string password:" + " " + email + " " + password);
			try
			{
				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					connection.Open();

					using (NpgsqlCommand command = new NpgsqlCommand("SELECT procedures.login(@email, @password)", connection))
					{
						command.Parameters.AddWithValue("email", email);
						command.Parameters.AddWithValue("password", password);

						NpgsqlDataReader reader = command.ExecuteReader();
						if (reader.HasRows && reader.Read())
						{
							Guid result = reader.GetGuid(0);
							Console.WriteLine("Пользователь авторизован. ID: " + result);
							return Result.Success(result);
						}
						return Result.Failure<Guid>("Неверные учетные данные пользователя1.");
					}
				}
			}
			catch (InvalidCastException)
			{
				Console.WriteLine("Неверные учетные данные пользователя2.");
				return Result.Failure<Guid>("Неверные учетные данные пользователя2.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR: " + ex.Message);
				return Result.Failure<Guid>("Ошибка запроса или соединения.");
			}
		}

		public Result<Guid> SignUp(AppRoles role, string name, string surname, string email, string password)
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

					// Выходные параметры
					command.Parameters.Add("@role_id", NpgsqlDbType.Uuid).Direction = ParameterDirection.Output;
					command.Parameters.Add("@error", NpgsqlDbType.Varchar, 100).Direction = ParameterDirection.Output;

					command.ExecuteNonQuery();

					string error = command.Parameters["@error"].Value.ToString();
					Console.WriteLine("Error: " + error);



					// Обработка результата
					if (!string.IsNullOrEmpty(error))
					{
						switch (error)
						{
							case "Invalid email format!":
								return Result.Failure<Guid>("Неверная форма почты!");
							case "Customer already exists!":
								return Result.Failure<Guid>("Такой Пользователь уже существует.");
							case "Manager already exists!":
								return Result.Failure<Guid>("Такой Менеджер уже существует.");
							case "Seller already exists!":
								return Result.Failure<Guid>("Такой Продавец уже существует.");
							case "Courier already exists!":
								return Result.Failure<Guid>("Такой Курьер уже существует.");
						}
					}
					else
					{
						Guid role_id = (Guid)command.Parameters["@role_id"].Value;
						Console.WriteLine("Role ID: " + role_id);
						return Result.Success(role_id);
					}
					return Result.Failure<Guid>("Ошибка запроса или соединения.1");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR: " + ex.Message);
				return Result.Failure<Guid>("Ошибка запроса или соединения.2");
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