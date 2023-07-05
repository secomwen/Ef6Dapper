using AutoMapper;
using OnePage.Data;
using OnePage.WebAPI.Models;

namespace OnePage.WebAPI
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderSummaryResponse>();
        }
    }
}