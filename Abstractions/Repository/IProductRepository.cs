using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pizza.MVVM.Model;

using CSharpFunctionalExtensions;

namespace Pizza.Abstractions
{
	public interface IProductRepository
	{
		Result<ObservableCollection<ProductModel>> GetAllProducts();
		ProductModel GetProductById(int id);

		void AddProduct(ProductModel product);
		Result<bool> UpdateProduct(ProductModel product);
		Result<bool> DeleteProduct(Guid id);

	}
}
