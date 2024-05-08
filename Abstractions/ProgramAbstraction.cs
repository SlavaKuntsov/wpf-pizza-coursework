using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Pizza.Abstractions
{
	public class ProgramAbstraction
	{
		public enum ProgramLanguages
		{
			Ru,
			En,
		}

		public Dictionary<ProgramLanguages, string> ProgramLanguagesDictionary = new Dictionary<ProgramLanguages, string>
		{
			{ ProgramLanguages.Ru, "ru" },
			{ ProgramLanguages.En, "en" },
		};

		public enum SortByProperty
		{
			Date,
			Name,
			Price,
		}
		public enum SortByPropertyAll
		{
			DateAsc,
			DateDesc,
			NameAsc,
			NameDesc,
			PriceAsc,
			PriceDesc
		}

		public enum AppRoles
		{
			[Description("Customer")]
			Customer,
			[Description("Manager")]
			Manager,
			[Description("Seller")]
			Seller,
			[Description("Courier")]
			Courier,
			[Description("Auth")]
			Auth
		}
	}
}
