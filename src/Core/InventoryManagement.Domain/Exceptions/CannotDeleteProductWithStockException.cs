namespace InventoryManagement.Domain.Exceptions
{
    public sealed class CannotDeleteProductWithStockException : DomainException
    {
        public string ProductName { get; }
        public int StockQuantity { get; }
        public CannotDeleteProductWithStockException(string productName, int stockQuantity)
            : base($"Cannot delete product '{productName}'. Current stock: {stockQuantity}. " +
                   "Remove all stock before deleting.")
        {
            ProductName = productName;
            StockQuantity = stockQuantity;
        }
    }
}
