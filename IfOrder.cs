using Online_Shoppng.Models.Entity;
using System.Collections.Generic;

namespace Online_Shoppng.Models.Services
{
    public interface IfOrder
    {
        int AddOrder(Order order);
        int UpdateOrder(Order order);
        int DeleteOrder(int orderId);
        Order GetOrderById(int orderId);
        List<Order> GetAllOrders();
        List<OrdersMaster> GetOrdersByUsername(string username);
        int AddCartOrder(OrdersMaster master, List<Order> orders);
        List<Order> GetOrderItemsByOrderMasterId(int orderMasterId, string username);

    }
}
