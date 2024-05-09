//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using _4.MVVM.Model;

//using FluentValidation;

//namespace Pizza.Utilities
//{
//	public class ProductModelValidator : AbstractValidator<ProductModel>
//	{
//		public ProductModelValidator()
//		{
//			RuleFor(model => model.Name).NotEmpty().WithMessage("Поле 'Название (краткое)' обязательно для заполнения.");
//			RuleFor(model => model.FullName).NotEmpty().WithMessage("Поле 'Название (полное)' обязательно для заполнения.");
//			RuleFor(model => model.Description).NotEmpty().WithMessage("Поле 'Описание' обязательно для заполнения.");
//			RuleFor(model => model.Price).GreaterThan(0).WithMessage("Поле 'Цена' должно быть больше нуля.");
//			RuleFor(model => model.Count).GreaterThan(0).WithMessage("Поле 'Количество' должно быть больше нуля.");
//		}
//	}
//}
