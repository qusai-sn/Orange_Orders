using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Orange_orders.Models;

public partial class OrderList
{
    public int OrderListId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    [JsonIgnore]
    public virtual User? CreatedByUser { get; set; }

    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
