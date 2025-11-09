using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.OrderWorkerService.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<LineItem> Items { get; set; } = new();

        private Order() { }

        public Order(Guid id, string customerName, List<LineItem> items)
        {
            Id = id;
            CustomerName = customerName;
            CreatedAt = DateTime.Now;
            Items = items;
        }
    }
}
