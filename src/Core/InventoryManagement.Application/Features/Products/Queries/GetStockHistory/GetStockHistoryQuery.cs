using InventoryManagement.Application.Common.Models;
using MediatR;

namespace InventoryManagement.Application.Features.Products.Queries.GetStockHistory;

/// <summary>
/// Query to retrieve stock history records.
/// Can be filtered by product, date range, and limited by number of records.
/// </summary>
public sealed record GetStockHistoryQuery : IRequest<List<StockHistoryDto>>
{
    /// <summary>
    /// Gets the optional product ID to filter by.
    /// If null, returns history for all products.
    /// </summary>
    public Guid? ProductId { get; init; }

    /// <summary>
    /// Gets the optional start date for filtering.
    /// If null, no start date filter is applied.
    /// </summary>
    public DateTime? FromDate { get; init; }

    /// <summary>
    /// Gets the optional end date for filtering.
    /// If null, no end date filter is applied.
    /// </summary>
    public DateTime? ToDate { get; init; }

    /// <summary>
    /// Gets the maximum number of records to return.
    /// Defaults to 50.
    /// </summary>
    public int Take { get; init; } = 50;
}
