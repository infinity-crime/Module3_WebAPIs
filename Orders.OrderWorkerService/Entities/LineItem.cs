using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Orders.OrderWorkerService.Entities
{
    public class LineItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        private LineItem() { }

        public LineItem(Guid id, Guid orderId, string productName, decimal price, int quantity)
        {
            Id = id;
            OrderId = orderId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }
    }
}
