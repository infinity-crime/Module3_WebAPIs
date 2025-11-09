using Orders.OrderWorkerService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.OrderWorkerService.Services
{
    public class OrderService : IOrderService
    {
        private readonly List<Order> _orders = new();

        public void AddOrder(Order order)
        {
            _orders.Add(order);
        }

        public List<Order> GetAllOrders()
        {
            return _orders;
        }
    }
}
