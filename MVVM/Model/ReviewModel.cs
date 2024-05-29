using System;

namespace Pizza.MVVM.Model
{
	public class ReviewModel
	{
		public  int Id { get; set; }
		public string Text { get; set; }
		public Guid CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string CustomerSurname { get; set; }

		//public ReviewModel(int id, string text, Guid customerId)
		//{
		//	Id = id;
		//	Text = text;
		//	CustomerId = customerId;
		//}
	}
}
