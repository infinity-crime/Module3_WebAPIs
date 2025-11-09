using Orders.OrderWorkerService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.OrderWorkerService.Services
{
    public interface IOrderService
    {
        void AddOrder(Order order);
        List<Order> GetAllOrders();
    }
}
