﻿namespace ECommerceAPI.Application.Features.Queries.OrderQueries.GetOrderById
{
    public class GetOrderByIdAsyncQueryResponse
    {
        public string? Id { get; set; }
        public string? Address { get; set; }
        public object? BasketItems { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Description { get; set; }
        public string? OrderCode { get; set; }
    }
}