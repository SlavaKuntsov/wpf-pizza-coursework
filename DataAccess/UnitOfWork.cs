using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pizza.Repository;

namespace Pizza.DataAccess
{
	internal class UnitOfWork : IDisposable
	{
		private string _connectionString;

		private ProductRepository _productRepository;
		private BasketRepository _basketRepository;
		private UserRepository _userRepository;
		
		public UnitOfWork(string connectionString)
		{
			_connectionString = connectionString;
		}

		public ProductRepository Product
		{
			get
			{
				if (_productRepository == null)
					_productRepository = new ProductRepository(_connectionString);
				return _productRepository;
			}
		}

		public BasketRepository Basket
		{
			get
			{
				if (_basketRepository == null)
					_basketRepository = new BasketRepository(_connectionString);
				return _basketRepository;
			}
		}

		public UserRepository User
		{
			get
			{
				if (_userRepository == null)
					_userRepository = new UserRepository(_connectionString);
				return _userRepository;
			}
		}

		private bool disposed = false;

		public virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					Console.WriteLine("DISPOSED");
					//_context.Dispose();
				}
				this.disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
