using Online_Shoppng.Models.Entity;
using Online_Shoppng.Models.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Online_Shoppng.Models.Repository
{
    public class CartRepo : IfCart
    {
        private readonly OnlineShoppingDBcontext _context;

        public CartRepo(OnlineShoppingDBcontext context)
        {
            _context = context;
        }

        public void AddToCart(Cart cart)
        {
            var existingItem = _context.Carts.FirstOrDefault(c =>
                c.Username == cart.Username &&
                c.ProductId == cart.ProductId &&
                c.Size == cart.Size
                
            );

            if (existingItem != null)
            {
                // 🔁 Increase quantity if already in cart
                existingItem.Quantity += cart.Quantity;
                existingItem.TotalAmount = existingItem.Quantity * existingItem.Price;
            }
            else
            {
                cart.AddedOn = DateTime.Now;
                _context.Carts.Add(cart);
            }

            _context.SaveChanges();
        }

        // 📦 Get cart items for logged-in user
        public List<Cart> GetCartByUsername(string username)
        {
            return _context.Carts
                .Include(c => c.Product)
                .Where(c => c.Username == username)
                .OrderByDescending(c => c.AddedOn)
                .ToList();
        }

        // ❌ Remove single cart item
        public void RemoveFromCart(int cartId)
        {
            var item = _context.Carts.Find(cartId);
            if (item != null)
            {
                _context.Carts.Remove(item);
                _context.SaveChanges();
            }
        }

        // 🧹 Clear cart after order
        public void ClearCart(string username)
        {
            var items = _context.Carts
                .Where(c => c.Username == username)
                .ToList();

            _context.Carts.RemoveRange(items);
            _context.SaveChanges();
        }

        public void UpdateQuantity(int cartId, int quantity)
        {
            var item = _context.Carts.Find(cartId);
            if (item != null)
            {
                item.Quantity = quantity;
                item.TotalAmount = item.Price * quantity;
                _context.SaveChanges();
            }
        }

    }
}
