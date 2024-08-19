using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Orange_orders.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<OrderList> OrderLists { get; set; } = new List<OrderList>();

    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
