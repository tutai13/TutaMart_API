using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TutaMart_API.Data;
using TutaMart_API.DTOs;
using TutaMart_API.Models;

namespace TutaMart_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    OrderDate = request.OrderDate,
                    CustomerName = request.CustomerName,
                    CustomerPhone = request.CustomerPhone,
                    PaymentMethod = request.PaymentMethod,
                    Subtotal = request.Subtotal,
                    Tax = request.Tax,
                    DiscountAmount = request.DiscountAmount,
                    DiscountType = request.DiscountType,
                    DiscountValue = request.DiscountValue,
                    TotalAmount = request.TotalAmount,
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // Lưu để có OrderId

                foreach (var detailDto in request.OrderDetails)
                {
                    var product = await _context.Products.FindAsync(detailDto.ProductId);
                    if (product == null)
                        return NotFound($"Sản phẩm ID {detailDto.ProductId} không tồn tại.");

                    if (product.Quantity < detailDto.Quantity)
                        return BadRequest($"Sản phẩm {product.ProductName} chỉ còn {product.Quantity} trong kho.");

                    // Cập nhật tồn kho
                    product.Quantity -= detailDto.Quantity;

                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = detailDto.ProductId,
                        Quantity = detailDto.Quantity,
                        UnitPrice = detailDto.UnitPrice,
                        TotalPrice = detailDto.TotalPrice
                    };

                    _context.OrderDetails.Add(orderDetail);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var orderWithDetails = await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

                var response = new OrderResponse
                {
                    OrderId = orderWithDetails.OrderId,
                    OrderDate = orderWithDetails.OrderDate,
                    CustomerName = orderWithDetails.CustomerName,
                    PaymentMethod = orderWithDetails.PaymentMethod,
                    Subtotal = orderWithDetails.Subtotal,
                    Tax = orderWithDetails.Tax,
                    DiscountAmount = orderWithDetails.DiscountAmount,
                    TotalAmount = orderWithDetails.TotalAmount,
                    OrderDetails = orderWithDetails.OrderDetails.Select(od => new OrderDetailResponse
                    {
                        ProductId = od.ProductId,
                        ProductName = od.Product.ProductName,     // ← đây là tên sản phẩm
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice,
                        TotalPrice = od.TotalPrice
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetOrder), new { id = orderWithDetails.OrderId }, response);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // Optional: GET chi tiết đơn
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();
            return order;
        }
    }
}
