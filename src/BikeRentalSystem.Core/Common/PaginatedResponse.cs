﻿namespace BikeRentalSystem.Core.Common;

public class PaginatedResponse<TEntity>
{
    public List<TEntity> Items { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public PaginatedResponse()
    {
        Items = new List<TEntity>();
    }

    public PaginatedResponse(List<TEntity> items, int count, int? pageNumber, int? pageSize)
    {
        Items = items;
        TotalItems = count;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }
}
