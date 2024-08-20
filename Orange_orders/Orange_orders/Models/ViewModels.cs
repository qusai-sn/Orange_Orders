using AutoMapper; // For using AutoMapper to map between entities and DTOs
using Orange_orders.Models;
using System.Collections.Generic; // For using generic collections like List<T>

namespace Orange_orders.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // DTO classes
    public class OrderListDTO
    {
        public int OrderListId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UserDTO CreatedByUser { get; set; }  // Include the user who created the list
        public List<OrderDTO> Orders { get; set; }
    }

    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UserDTO User { get; set; }  // Include the user related to the order
    }
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}

// AutoMapper profile configuration
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OrderList, OrderListDTO>()
            .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src => src.CreatedByUser))
            .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders))
            .ReverseMap();

        CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ReverseMap();

        CreateMap<User, UserDTO>().ReverseMap();
    }
}
