using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using CSharpFunctionalExtensions;

namespace Pizza.Encrypt
{
	public class EncryptClass : IEncrypt
	{
		private string _key = "qq2DBL/6b5T0n3vG9HhGv05kP2IsqNdnwfEcOUuTXRU=";
		private string _filePath = "../../credentials.txt";

		public string EncryptData(string data)
		{
			byte[] encryptedBytes;

			using (var aesAlg = Aes.Create())
			{
				aesAlg.Key = Convert.FromBase64String(_key);
				aesAlg.GenerateIV();

				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					using (var swEncrypt = new StreamWriter(csEncrypt))
					{
						swEncrypt.Write(data);
					}

					encryptedBytes = msEncrypt.ToArray();
				}
			}

			string encryptedData = Convert.ToBase64String(encryptedBytes);
			AddEncryptedDataToFile(encryptedData);

			return encryptedData;
		}

		private string DecryptData(string encryptedData)
		{
			string decryptedData;

			byte[] cipherTextBytes = Convert.FromBase64String(encryptedData);

			using (var aesAlg = Aes.Create())
			{
				aesAlg.Key = Convert.FromBase64String(_key);
				aesAlg.IV = cipherTextBytes.Take(16).ToArray();

				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

				using (var msDecrypt = new MemoryStream(cipherTextBytes.Skip(16).ToArray()))
				using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				using (var srDecrypt = new StreamReader(csDecrypt))
				{
					decryptedData = srDecrypt.ReadToEnd();
				}
			}

			return decryptedData;
		}

		public void DeleteFile()
		{
			if (File.Exists(_filePath))
			{
				File.Delete(_filePath);
			}
		}

		private void AddEncryptedDataToFile(string encryptedData)
		{
			// Чтение существующего файла или создание нового файла, если он не существует
			List<string> encryptedList = File.Exists(_filePath) ? File.ReadAllLines(_filePath).ToList() : new List<string>();

			// Добавление зашифрованных данных в список
			encryptedList.Add(encryptedData);

			// Запись списка зашифрованных данных в файл
			File.WriteAllLines(_filePath, encryptedList);
		}

		public Result<string> GetDecryptedEmail()
		{
			var encryptedDataResult = ReadEncryptedDataFromFile();

			if (encryptedDataResult.IsFailure)
			{
				return Result.Failure<string>(encryptedDataResult.Error);
			}

			List<string> encryptedList = encryptedDataResult.Value;

			if (encryptedList.Count == 0)
			{
				return Result.Failure<string>("No encrypted email found.");
			}

			string encryptedEmail = encryptedList[0];
			string decryptedEmail = DecryptData(encryptedEmail);

			return Result.Success(decryptedEmail);
		}

		public Result<List<string>> ReadEncryptedDataFromFile()
		{
			// Проверка существования файла
			if (!File.Exists(_filePath))
			{
				return Result.Failure<List<string>>($"The file '{_filePath}' does not exist.");
			}

			// Чтение зашифрованных данных из файла
			List<string> encryptedList = File.ReadAllLines(_filePath).ToList();

			return Result.Success(encryptedList);
		}
	}
}