using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Products.Queries.GetStockHistory;

/// <summary>
/// Handler for GetStockHistoryQuery.
/// Retrieves stock history records with optional filtering by product and date range.
/// </summary>
public sealed class GetStockHistoryQueryHandler : IRequestHandler<GetStockHistoryQuery, List<StockHistoryDto>>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetStockHistoryQueryHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetStockHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the GetStockHistoryQuery request.
    /// Retrieves stock history records with joins to products and users for complete information.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of StockHistoryDto objects.</returns>
    public async Task<List<StockHistoryDto>> Handle(GetStockHistoryQuery request, CancellationToken cancellationToken)
    {
        // Start with stock histories query
        var query = _context.StockHistories.AsQueryable();

        // Filter by product ID if provided
        if (request.ProductId.HasValue)
        {
            query = query.Where(sh => sh.ProductId == request.ProductId.Value);
        }

        // Filter by date range if provided
        if (request.FromDate.HasValue)
        {
            query = query.Where(sh => sh.ChangedAt >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(sh => sh.ChangedAt <= request.ToDate.Value);
        }

        // Join with Products and Users, order by date (newest first), and take specified number
        var stockHistories = await query
            .Join(
                _context.Products,
                sh => sh.ProductId,
                p => p.Id,
                (sh, p) => new { StockHistory = sh, ProductName = p.Name.Value })
            .Join(
                _context.Users,
                combined => combined.StockHistory.ChangedBy,
                u => u.Id,
                (combined, u) => new
                {
                    combined.StockHistory,
                    combined.ProductName,
                    ChangedByName = u.FullName
                })
            .OrderByDescending(x => x.StockHistory.ChangedAt)
            .Take(request.Take)
            .Select(x => new StockHistoryDto
            {
                Id = x.StockHistory.Id,
                ProductId = x.StockHistory.ProductId,
                ProductName = x.ProductName,
                QuantityChanged = x.StockHistory.QuantityChanged,
                QuantityAfter = x.StockHistory.QuantityAfter,
                ChangeType = x.StockHistory.ChangeType,
                Reason = x.StockHistory.Reason,
                ChangedBy = x.StockHistory.ChangedBy,
                ChangedByName = x.ChangedByName,
                ChangedAt = x.StockHistory.ChangedAt
            })
            .ToListAsync(cancellationToken);

        return stockHistories;
    }
}
