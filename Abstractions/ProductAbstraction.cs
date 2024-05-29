using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pizza.Abstractions
{
	public class ProductAbstraction
	{
		public enum PizzaCategories
		{
			[Description("Пицца")]
			Pizza,
			[Description("Десерт")]
			Dessert,
			[Description("Напиток")]
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
			[Description("Не продукт")]
			NotAPizza,
			[Description("Маленькая")]
			Small,
			[Description("Средняя")]
			Medium,
			[Description("Большая")]
			Big
		}

		public Dictionary<PizzaSizes, string> PizzaSizesDictionary = new Dictionary<PizzaSizes, string>
		{
			{ PizzaSizes.Small, "Маленькая" },
			{ PizzaSizes.Medium, "Средняя" },
			{ PizzaSizes.Big, "Большая" },
		};

		public enum PizzaTypes
		{
			[Description("Тонкое")]
			Thin,
			[Description("Обычное")]
			Default
		}

		public enum PizzaProperties
		{
			[Description("Размер")]
			Size,
			[Description("Тесто")]
			Type
		}

		public enum PizzaInStock
		{
			[Description("Да")]
			Yes,
			[Description("Нет")]
			No
		}

		public enum PizzaIsDelivery
		{
			[Description("Да")]
			Yes,
			[Description("Нет")]
			No
		}
	}

	public static class EnumExtensions
	{
		public static string GetDescription(this Enum value)
		{
			FieldInfo field = value.GetType().GetField(value.ToString());
			DescriptionAttribute attribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));
			return attribute == null ? value.ToString() : attribute.Description;
		}
	}

}
