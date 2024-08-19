using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orange_orders.Models; // Adjust the namespace as needed
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

[Route("api/[controller]")]
[ApiController]
public class OrderListsController : ControllerBase
{
    private readonly OrangeOrdersContext _context;

    public OrderListsController(OrangeOrdersContext context)
    {
        _context = context;
    }

    // Retrieve all OrderLists
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderList>>> GetOrderLists()
    {
        var orderLists = await _context.OrderLists
                                       .Include(ol => ol.Orders)
                                       .ToListAsync();

        return Ok(orderLists);
    }

    // Retrieve a specific OrderList by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderList>> GetOrderList(int id)
    {
        var orderList = await _context.OrderLists
                                      .Include(ol => ol.Orders)
                                      .FirstOrDefaultAsync(ol => ol.OrderListId == id);

        if (orderList == null)
        {
            return NotFound();
        }

        return orderList;
    }

    // Add a new OrderList
    [HttpPost]
    public async Task<ActionResult<OrderList>> AddOrderList([FromBody] OrderList orderList)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _context.Users.FindAsync(orderList.CreatedByUserId);
        if (user == null)
            return BadRequest("Invalid CreatedByUserId.");

        orderList.CreatedAt = DateTime.Now;
        orderList.UpdatedAt = DateTime.Now;

        _context.OrderLists.Add(orderList);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrderList), new { id = orderList.OrderListId }, orderList);
    }

    // Delete an OrderList by ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderList(int id)
    {
        var orderList = await _context.OrderLists.FindAsync(id);
        if (orderList == null)
            return NotFound();

        _context.OrderLists.Remove(orderList);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Add an Order to an existing OrderList
    [HttpPost("{orderListId}/orders")]
    public async Task<ActionResult<Order>> AddOrderToList(int orderListId, [FromBody] Order order)
    {
        var orderList = await _context.OrderLists.FindAsync(orderListId);
        if (orderList == null)
            return NotFound("Order list not found.");

        var user = await _context.Users.FindAsync(order.UserId);
        if (user == null)
            return BadRequest("Invalid UserId.");

        order.CreatedAt = DateTime.Now;
        order.UpdatedAt = DateTime.Now;
        orderList.Orders.Add(order);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { orderListId, orderId = order.OrderId }, order);
    }

    // Delete an Order from an OrderList
    [HttpDelete("{orderListId}/orders/{orderId}")]
    public async Task<IActionResult> DeleteOrder(int orderListId, int orderId)
    {
        var orderList = await _context.OrderLists
                                      .Include(ol => ol.Orders)
                                      .FirstOrDefaultAsync(ol => ol.OrderListId == orderListId);

        if (orderList == null)
            return NotFound("Order list not found.");

        var order = orderList.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
            return NotFound("Order not found.");

        orderList.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Update an Order in an OrderList
    [HttpPut("{orderListId}/orders/{orderId}")]
    public async Task<IActionResult> UpdateOrder(int orderListId, int orderId, [FromBody] Order updatedOrder)
    {
        var orderList = await _context.OrderLists
                                      .Include(ol => ol.Orders)
                                      .FirstOrDefaultAsync(ol => ol.OrderListId == orderListId);

        if (orderList == null)
            return NotFound("Order list not found.");

        var order = orderList.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
            return NotFound("Order not found.");

        var user = await _context.Users.FindAsync(updatedOrder.UserId);
        if (user == null)
            return BadRequest("Invalid UserId.");

        // Update order properties
        order.OrderName = updatedOrder.OrderName;
        order.Description = updatedOrder.Description;
        order.Price = updatedOrder.Price;
        order.IsPaid = updatedOrder.IsPaid;
        order.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Retrieve a specific Order from an OrderList by ID
    [HttpGet("{orderListId}/orders/{orderId}")]
    public async Task<ActionResult<Order>> GetOrder(int orderListId, int orderId)
    {
        var orderList = await _context.OrderLists
                                      .Include(ol => ol.Orders)
                                      .FirstOrDefaultAsync(ol => ol.OrderListId == orderListId);

        if (orderList == null)
            return NotFound("Order list not found.");

        var order = orderList.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
            return NotFound("Order not found.");

        return order;
    }
}
