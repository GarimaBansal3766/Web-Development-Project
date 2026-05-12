using Online_Shoppng.Models.Entity;
using Online_Shoppng.Models.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Online_Shoppng.Models.Repository
{
    public class OrderRepo : IfOrder
    {
        private  OnlineShoppingDBcontext _context;

        public OrderRepo(OnlineShoppingDBcontext context)
        {
            _context = context;
        }

        // ADD new order
        public int AddOrder(Order order)
        {
            _context.OrderDetails.Add(order);
            return _context.SaveChanges(); // returns 1 if success
        }
       
        // UPDATE an order
        public int UpdateOrder(Order order)
        {
            var existing = _context.OrderDetails.Find(order.OrderId);
            if (existing == null)
            {
                return 0; // not found
            }

            _context.Entry(existing).CurrentValues.SetValues(order);
            return _context.SaveChanges();
        }

        // DELETE an order
        public int DeleteOrder(int orderId)
        {
            var order = _context.OrderDetails.Find(orderId);
            if (order == null)
            {
                return 0;
            }

            _context.OrderDetails.Remove(order);
            return _context.SaveChanges();
        }

        // GET a single order by ID
        public Order? GetOrderById(int orderId)
        {
            return _context.OrderDetails
                .Include(o => o.Product)
                .Include(o => o.Login)
                .FirstOrDefault(o => o.OrderId == orderId);
        }

        // GET all orders
        public List<Order> GetAllOrders()
        {
            return _context.OrderDetails
                .Include(o => o.Product)
                .Include(o => o.Login)
                .ToList();
        }

        public int AddCartOrder(OrdersMaster master, List<Order> orders)
        {
            // 1️⃣ Save MASTER order
            // 1️⃣ Save master FIRST
            _context.OrdersMaster.Add(master);
            _context.SaveChanges(); // master.OrderMasterId generated

            // 2️⃣ Save order details
            foreach (var order in orders)
            {
                order.OrderMasterId = master.OrderMasterId;
                order.TrackingNumber = "TRK" + DateTime.Now.Ticks;
                _context.OrderDetails.Add(order);
            }
                    // 3️⃣ Save all order details
            return _context.SaveChanges();

        }
        public List<OrdersMaster> GetOrdersByUsername(string username)
        {
            return _context.OrdersMaster
                .Where(o => o.Username == username)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }
        public List<Order> GetOrderItemsByOrderMasterId(int orderMasterId, string username)
        {
            return _context.OrderDetails
                .Include(o => o.Product)
                .Where(o => o.OrderMasterId == orderMasterId
                            && o.Username == username)
                .ToList();
        }


    }
}
