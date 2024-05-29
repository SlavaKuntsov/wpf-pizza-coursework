using System;
using System.Collections.Generic;

namespace Pizza.MVVM.Model
{
	public class OrderModel
	{
		public Guid Id { get; set; }
		public string Status { get; set; }
		public DateTime Date { get; set; }
		public double TotalPrice { get; set; }
		public bool IsDelivery { get; set; }
		public bool IsNotDelivery { get; set; }
		public string Address { get; set; }
		public Guid? SellerId { get; set; }
		public Guid? CourierId { get; set; }
		public bool SellerIdVisibility {  get; set; }
		public bool CourierIdVisibility {  get; set; }
		public string WhichButtonIsVisible { get; set; }
		public List<ProductModelNew> OrderProducts { get; set; } = new List<ProductModelNew>();

		public OrderModel(Guid id, string status, DateTime date, double price, bool isDelivery, string address, Guid? sellerId, Guid? courierId, List<ProductModelNew> orderProducts)
		{
			Id = id;
			Status = status;
			Date = date;
			TotalPrice = price;
			IsDelivery = isDelivery;
			IsNotDelivery = !isDelivery;
			Address = address;
			SellerId = sellerId;
			CourierId = courierId;
			OrderProducts = orderProducts;

			Console.WriteLine("NULL ID " + sellerId);
			Console.WriteLine("NULL ID " + courierId);

			if (sellerId != null)
			{
				SellerIdVisibility = true;
			}
			else
			{
				SellerIdVisibility = false;
			}

			if (courierId != null)
			{
				CourierIdVisibility = true;
			}
			else
			{
				CourierIdVisibility = false;
			}

			//switch (Status)
			//{
			//	case "Отменен":
			//		WhichButtonIsVisible = "";
			//		break;
			//	case "Готовится":
			//		CreatedOrderVisibility = false;
			//		ProcessedOrderVisibility = true;
			//		CanceledOrderVisibility = false;
			//		ReadyOrderVisibility = false;
			//		break;
			//	case "Оформлен":
			//		CreatedOrderVisibility = true;
			//		ProcessedOrderVisibility = false;
			//		CanceledOrderVisibility = false;
			//		ReadyOrderVisibility = false;
			//		break;
			//	case "Готов":
			//		CreatedOrderVisibility = false;
			//		ProcessedOrderVisibility = false;
			//		CanceledOrderVisibility = false;
			//		ReadyOrderVisibility = true;
			//		break;
			//	default:
			//		break;
			//}
		}
	}
}
