using System;

using CSharpFunctionalExtensions;

using static Pizza.Abstractions.ProgramAbstraction;

namespace Pizza.Repository
{
	public interface IUserRepository
	{
		Result<Guid> LogIn(string email, string password);
		Result<Guid> SignUp(AppRoles role, string name, string surname, string email, string password);
	}
}