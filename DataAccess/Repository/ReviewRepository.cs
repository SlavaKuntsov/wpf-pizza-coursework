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

namespace Pizza.Repository
{
	public class ReviewRepository
	{
		private string _connectionString;

		public ReviewRepository(string connection)
		{
			_connectionString = connection;
		}
		public async Task<Result> AddReview(Guid customerId, string reviewText)
		{
			try
			{
				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					using (NpgsqlCommand command = new NpgsqlCommand("CALL procedures.add_review(@customer_id_, @review_text_)", connection))
					{
						command.Parameters.AddWithValue("customer_id_", customerId);
						command.Parameters.AddWithValue("review_text_", reviewText);

						await command.ExecuteNonQueryAsync();
					}
				}

				return Result.Success();
			}
			catch (Exception ex)
			{
				return Result.Failure(ex.Message);
			}
		}

		public async Task<Result<List<ReviewModel>>> GetAllReviews(Guid customerId)
		{
			try
			{
				using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					using (NpgsqlCommand command = new NpgsqlCommand("procedures.get_all_reviews", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Parameters.AddWithValue("customer_id_", customerId);

						using (var reader = await command.ExecuteReaderAsync())
						{
							List<ReviewModel> reviews = new List<ReviewModel>();

							while (await reader.ReadAsync())
							{
								ReviewModel review = new ReviewModel
								{
									Id = reader.GetInt32(reader.GetOrdinal("review_id")),
									Text = reader.GetString(reader.GetOrdinal("review_text")),
									CustomerId = reader.GetGuid(reader.GetOrdinal("customer_id")),
									CustomerName = reader.GetString(reader.GetOrdinal("customer_name")),
									CustomerSurname = reader.GetString(reader.GetOrdinal("customer_surname"))
								};

								reviews.Add(review);
							}

							return Result.Success(reviews);
						}
					}
				}
			}
			catch (Exception ex)
			{
				return Result.Failure<List<ReviewModel>>(ex.Message);
			}
		}

		public async Task<Result> DeleteReview(int reviewId)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				// Используем правильную команду для вызова процедуры
				using (var command = new NpgsqlCommand("CALL procedures.delete_review(@review_id_)", connection))
				{
					// Параметры для процедуры
					command.Parameters.AddWithValue("review_id_", NpgsqlTypes.NpgsqlDbType.Integer, reviewId);

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

		public async Task<Result> DeleteReviewFromCustomer(int reviewId, Guid customerId)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				// Используем правильную команду для вызова процедуры
				using (var command = new NpgsqlCommand("CALL procedures.delete_review_from_customer(@review_id_, @customer_id_)", connection))
				{
					// Параметры для процедуры
					command.Parameters.AddWithValue("review_id_", NpgsqlTypes.NpgsqlDbType.Integer, reviewId);
					command.Parameters.AddWithValue("customer_id_", NpgsqlTypes.NpgsqlDbType.Uuid, customerId);

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

	}
}