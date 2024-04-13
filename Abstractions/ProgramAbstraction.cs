using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
