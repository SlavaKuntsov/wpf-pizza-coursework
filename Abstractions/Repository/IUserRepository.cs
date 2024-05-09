using System;

using CSharpFunctionalExtensions;

using Pizza.MVVM.Model;

using static Pizza.Abstractions.ProgramAbstraction;

namespace Pizza.Repository
{
	public interface IUserRepository
	{
		Result<UserModel> LogIn(string email, string password);
		Result<UserModel> SignUp(AppRoles role, string name, string surname, string email, string password);
		AppRoles GetUserRole(Guid id);
	}
}