using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Pizza.Encrypt
{
	public interface IEncrypt
	{
		string EncryptData(string data);
		//string DecryptData(string encryptedData);
		Result<string> GetDecryptedEmail();
		Result<List<string>> ReadEncryptedDataFromFile();
		void DeleteFile();
	}
}