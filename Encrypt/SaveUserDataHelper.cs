using System;
using System.IO;

using Newtonsoft.Json;

using Pizza.Manager;
using Pizza.MVVM.Model;

using static Pizza.Abstractions.ProgramAbstraction;

namespace Pizza.Encrypt
{
	public class SaveUserDataHelper
	{
		private readonly string _filePath = "../../userData.json";

		//AuthManager _authManager = AuthManager.Instance;
		public SaveUserDataHelper()
		{
			//_authManager = AuthManager.Instance;
		}

		public void SaveUserAuthData(Guid id, string name, string surname, string email, string password)
		{
			AuthManager authManager = AuthManager.Instance;
			authManager.Auth = true;

			var userData = new UserModel
			{
				Id = id,
				Name = name,
				Surname = surname,
				Email = email,
				Password = password,
			};

			//_authManager.User = userData;

			string json = JsonConvert.SerializeObject(userData, Formatting.Indented);

			if (File.Exists(_filePath))
			{
				File.WriteAllText(_filePath, json);
			}
			else
			{
				using (var file = File.CreateText(_filePath))
				{
					file.Write(json);
				}
			}
		}

		public UserModel GetUserAuthData()
		{
			if (File.Exists(_filePath))
			{
				string json = File.ReadAllText(_filePath);

				UserModel user = JsonConvert.DeserializeObject<UserModel>(json);

				//AuthManager authManager = AuthManager.Instance;
				//authManager.User = user;

				return user;
			}

			return null;
		}

		public void ClearUserData()
		{
			File.WriteAllText(_filePath, string.Empty);

			AuthManager authManager = AuthManager.Instance;
			authManager.Auth = false;
		}
	}

	//public void WriteEmailToFile(string email)
	//	{
	//		string storedEmail = ReadEmailFromFile();

	//		if (string.IsNullOrEmpty(storedEmail))
	//		{
	//			// запрос к бд
	//			File.WriteAllText(_filePath, email);

	//			AuthManager authManager = AuthManager.Instance;
	//			authManager.Auth = true;
	//			//return true;
	//		}
	//	}

	//	public string ReadEmailFromFile()
	//	{
	//		if (File.Exists(_filePath))
	//		{
	//			return File.ReadAllText(_filePath);
	//		}

	//		return string.Empty;
	//	}
	//	public void ClearFile()
	//	{
	//		if (File.Exists(_filePath))
	//		{
	//			File.Delete(_filePath);

	//			AuthManager authManager = AuthManager.Instance;

	//			authManager.Auth = false;
	//		}
	//	}
}
