using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pizza.DataAccess.Repository;
using Pizza.Repository;

namespace Pizza.DataAccess
{
	internal class UnitOfWork : IDisposable
	{
		private readonly string _connectionString;

		private ProductRepository _productRepository;
		private BasketRepository _basketRepository;
		private OrderRepository _orderRepository;
		private UserRepository _userRepository;
		private ReviewRepository _reviewRepository;
		
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

		public OrderRepository Order
		{
			get
			{
				if (_orderRepository == null)
					_orderRepository = new OrderRepository(_connectionString);
				return _orderRepository;
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

		public ReviewRepository Review
		{
			get
			{
				if (_reviewRepository == null)
					_reviewRepository = new ReviewRepository(_connectionString);
				return _reviewRepository;
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
