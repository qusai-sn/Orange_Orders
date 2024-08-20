using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orange_orders.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

[Route("api/[controller]")]
[ApiController]
public class OrderListsController : ControllerBase
{
    private readonly OrangeOrdersContext _context;
    private readonly IMapper _mapper;

    public OrderListsController(OrangeOrdersContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Retrieve all OrderLists
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderListDTO>>> GetOrderLists()
    {
        var orderLists = await _context.OrderLists
            .Include(ol => ol.Orders)
                .ThenInclude(o => o.User)
            .Include(ol => ol.CreatedByUser)
            .ToListAsync();

        // Use AutoMapper to convert entities to DTOs
        var orderListDTOs = _mapper.Map<List<OrderListDTO>>(orderLists);

        return Ok(orderListDTOs);
    }

    // Retrieve a specific OrderList by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderListDTO>> GetOrderList(int id)
    {
        var orderList = await _context.OrderLists
            .Include(ol => ol.Orders)
                .ThenInclude(o => o.User)
            .Include(ol => ol.CreatedByUser)
            .FirstOrDefaultAsync(ol => ol.OrderListId == id);

        if (orderList == null)
        {
            return NotFound();
        }

        // Use AutoMapper to convert the entity to DTO
        var orderListDTO = _mapper.Map<OrderListDTO>(orderList);

        return Ok(orderListDTO);
    }

    // Add a new OrderList
    [HttpPost]
    public async Task<ActionResult<OrderListDTO>> AddOrderList([FromBody] OrderListDTO orderListDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Use AutoMapper to convert DTO to entity
        var orderList = _mapper.Map<OrderList>(orderListDto);

        orderList.CreatedAt = DateTime.Now;
        orderList.UpdatedAt = DateTime.Now;

        _context.OrderLists.Add(orderList);
        await _context.SaveChangesAsync();

        // Map back to DTO after saving
        var createdOrderListDto = _mapper.Map<OrderListDTO>(orderList);

        return CreatedAtAction(nameof(GetOrderList), new { id = orderList.OrderListId }, createdOrderListDto);
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
    public async Task<ActionResult<OrderDTO>> AddOrderToList(int orderListId, [FromBody] OrderDTO orderDto)
    {
        var orderList = await _context.OrderLists.FindAsync(orderListId);
        if (orderList == null)
            return NotFound("Order list not found.");

        // Use AutoMapper to convert DTO to entity
        var order = _mapper.Map<Order>(orderDto);

        order.CreatedAt = DateTime.Now;
        order.UpdatedAt = DateTime.Now;
        orderList.Orders.Add(order);

        await _context.SaveChangesAsync();

        // Map back to DTO after saving
        var createdOrderDto = _mapper.Map<OrderDTO>(order);

        return CreatedAtAction(nameof(GetOrder), new { orderListId, orderId = order.OrderId }, createdOrderDto);
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
    public async Task<IActionResult> UpdateOrder(int orderListId, int orderId, [FromBody] OrderDTO updatedOrderDto)
    {
        var orderList = await _context.OrderLists
                                      .Include(ol => ol.Orders)
                                      .FirstOrDefaultAsync(ol => ol.OrderListId == orderListId);

        if (orderList == null)
            return NotFound("Order list not found.");

        var order = orderList.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
            return NotFound("Order not found.");

        // Use AutoMapper to update the entity with values from DTO
        _mapper.Map(updatedOrderDto, order);

        order.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Retrieve a specific Order from an OrderList by ID
    [HttpGet("{orderListId}/orders/{orderId}")]
    public async Task<ActionResult<OrderDTO>> GetOrder(int orderListId, int orderId)
    {
        var orderList = await _context.OrderLists
                                      .Include(ol => ol.Orders)
                                      .FirstOrDefaultAsync(ol => ol.OrderListId == orderListId);

        if (orderList == null)
            return NotFound("Order list not found.");

        var order = orderList.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
            return NotFound("Order not found.");

        // Use AutoMapper to convert the entity to DTO
        var orderDto = _mapper.Map<OrderDTO>(order);

        return Ok(orderDto);
    }
}
