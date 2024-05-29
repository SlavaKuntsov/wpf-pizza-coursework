using System;

using static Pizza.Abstractions.ProgramAbstraction;

namespace Pizza.MVVM.Model
{
	public class UserModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Address { get; set; }
		public AppRoles Role { get; set; }
	}
}
