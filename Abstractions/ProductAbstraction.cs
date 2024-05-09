using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza.Abstractions
{
	public class ProductAbstraction
	{
		public enum PizzaCategories
		{
			[Description("Pizza")]
			Pizza,
			[Description("Dessert")]
			Dessert,
			[Description("Drink")]
			Drink
		}

		public Dictionary<PizzaCategories, string> PizzaCategoriesDictionary = new Dictionary<PizzaCategories, string>
		{
			{ PizzaCategories.Pizza, "Пицца" },
			{ PizzaCategories.Dessert, "Десерт" },
			{ PizzaCategories.Drink, "Напиток" },
		};

		public enum Rating
		{
			None,
			One,
			Two,
			Three,
			Four,
			Five
		}

		public enum PizzaSizes
		{
			NotAPizza,
			Small,
			Medium,
			Big
		}

		public Dictionary<PizzaSizes, string> PizzaSizesDictionary = new Dictionary<PizzaSizes, string>
		{
			{ PizzaSizes.Small, "Маленькая" },
			{ PizzaSizes.Medium, "Средняя" },
			{ PizzaSizes.Big, "Большая" },
		};
	}
}
