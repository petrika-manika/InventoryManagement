import { useState, useEffect } from "react";
import { toast } from "react-hot-toast";
import type { ProductDto, StockHistoryDto } from "../types/product.types";
import stockService from "../services/stockService";
import productService from "../services/productService";
import Input from "../components/common/Input";
import Select from "../components/common/Select";
import StockHistoryTable from "../components/products/StockHistoryTable";

export default function StockHistoryPage() {
  const [history, setHistory] = useState<StockHistoryDto[]>([]);
  const [filteredHistory, setFilteredHistory] = useState<StockHistoryDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState("");
  const [filterProductId, setFilterProductId] = useState<string>("");
  const [filterChangeType, setFilterChangeType] = useState<string>("");
  const [products, setProducts] = useState<ProductDto[]>([]);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      const [historyData, productsData] = await Promise.all([
        stockService.getStockHistory({}),
        productService.getAllProducts(),
      ]);
      setHistory(historyData);
      setProducts(productsData);
    } catch {
      toast.error("Failed to load stock history");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    let filtered = history;

    // Filter by search term
    if (searchTerm) {
      const term = searchTerm.toLowerCase();
      filtered = filtered.filter(
        (h) =>
          h.productName.toLowerCase().includes(term) ||
          (h.reason && h.reason.toLowerCase().includes(term)) ||
          h.changedByName.toLowerCase().includes(term)
      );
    }

    // Filter by product
    if (filterProductId) {
      filtered = filtered.filter((h) => h.productId === filterProductId);
    }

    // Filter by change type
    if (filterChangeType) {
      filtered = filtered.filter((h) => h.changeType === filterChangeType);
    }

    setFilteredHistory(filtered);
  }, [history, searchTerm, filterProductId, filterChangeType]);

  return (
    <div className="p-6">
      {/* Header */}
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Stock History</h1>
        <p className="text-gray-600 mt-1">
          Track all stock movements and adjustments
        </p>
      </div>

      {/* Filters */}
      <div className="bg-white rounded-lg shadow p-4 mb-6">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          {/* Search */}
          <Input
            type="text"
            placeholder="Search by product, reason, or user..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />

          {/* Filter by product */}
          <Select
            value={filterProductId}
            onChange={(value) => setFilterProductId(value as string)}
            options={[
              { value: "", label: "All Products" },
              ...products.map((p) => ({ value: p.id, label: p.name })),
            ]}
            placeholder="Filter by product"
          />

          {/* Filter by change type */}
          <Select
            value={filterChangeType}
            onChange={(value) => setFilterChangeType(value as string)}
            options={[
              { value: "", label: "All Types" },
              { value: "Added", label: "Added" },
              { value: "Removed", label: "Removed" },
              { value: "Adjusted", label: "Adjusted" },
            ]}
            placeholder="Filter by type"
          />
        </div>
      </div>

      {/* Stats */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-6">
        <div className="bg-white rounded-lg shadow p-6">
          <p className="text-sm font-medium text-gray-600">Total Changes</p>
          <p className="text-2xl font-bold text-gray-900">
            {filteredHistory.length}
          </p>
        </div>
        <div className="bg-white rounded-lg shadow p-6">
          <p className="text-sm font-medium text-gray-600">Stock Added</p>
          <p className="text-2xl font-bold text-green-600">
            {filteredHistory.filter((h) => h.changeType === "Added").length}
          </p>
        </div>
        <div className="bg-white rounded-lg shadow p-6">
          <p className="text-sm font-medium text-gray-600">Stock Removed</p>
          <p className="text-2xl font-bold text-red-600">
            {filteredHistory.filter((h) => h.changeType === "Removed").length}
          </p>
        </div>
        <div className="bg-white rounded-lg shadow p-6">
          <p className="text-sm font-medium text-gray-600">Adjustments</p>
          <p className="text-2xl font-bold text-blue-600">
            {filteredHistory.filter((h) => h.changeType === "Adjusted").length}
          </p>
        </div>
      </div>

      {/* History Table */}
      <div className="bg-white rounded-lg shadow">
        <StockHistoryTable history={filteredHistory} loading={loading} />
      </div>
    </div>
  );
}
