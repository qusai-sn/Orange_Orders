using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Orange_orders.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public bool IsPaid { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? UserId { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }

    [JsonIgnore]
    public virtual ICollection<OrderList> OrderLists { get; set; } = new List<OrderList>();
}
