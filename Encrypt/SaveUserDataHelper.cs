using System;
using System.IO;

using Newtonsoft.Json;

using Pizza.Manager;
using Pizza.MVVM.Model;

namespace Pizza.Encrypt
{
	public class SaveUserDataHelper
	{
		private readonly string _filePath = "../../userData.json";

		public SaveUserDataHelper()
		{
		}

		public void SaveUserAuthData(Guid id, string email)
		{
			AuthManager authManager = AuthManager.Instance;
			authManager.Auth = true;

			var userData = new UserData
			{
				Id = id,
				Email = email
			};

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

		public UserData GetUserAuthData()
		{
			if (File.Exists(_filePath))
			{
				string json = File.ReadAllText(_filePath);
				return JsonConvert.DeserializeObject<UserData>(json);
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
