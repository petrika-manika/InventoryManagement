import { useState } from "react";
import { Link } from "react-router-dom";
import {
  ExclamationTriangleIcon,
  ChevronDownIcon,
  ChevronUpIcon,
} from "@heroicons/react/24/outline";
import type { ProductDto } from "../../types/product.types";
import StockBadge from "./StockBadge";
import Button from "../common/Button";

interface LowStockAlertProps {
  products: ProductDto[];
  threshold?: number; // default 10
  onAddStock?: (product: ProductDto) => void;
}

export default function LowStockAlert({
  products,
  threshold = 10,
  onAddStock,
}: LowStockAlertProps) {
  const [isExpanded, setIsExpanded] = useState(true);

  const lowStockProducts = products
    .filter((p) => p.stockQuantity <= threshold && p.stockQuantity > 0)
    .sort((a, b) => a.stockQuantity - b.stockQuantity); // Lowest first

  const outOfStockProducts = products.filter((p) => p.stockQuantity === 0);

  // Don't render if no alerts
  if (lowStockProducts.length === 0 && outOfStockProducts.length === 0) {
    return null;
  }

  const maxDisplay = 5;
  const displayOutOfStock = outOfStockProducts.slice(0, maxDisplay);
  const moreOutOfStock = outOfStockProducts.length - maxDisplay;
  const displayLowStock = lowStockProducts.slice(0, maxDisplay);
  const moreLowStock = lowStockProducts.length - maxDisplay;

  const totalAlerts = outOfStockProducts.length + lowStockProducts.length;

  return (
    <div className="bg-yellow-50 border-l-4 border-yellow-400 p-4 mb-6">
      <div className="flex items-start">
        <div className="flex-shrink-0">
          <ExclamationTriangleIcon className="h-6 w-6 text-yellow-400" />
        </div>
        <div className="ml-3 flex-1">
          <div className="flex items-center justify-between mb-2">
            <h3 className="text-lg font-medium text-yellow-800">
              ⚠️ Stock Alerts ({totalAlerts})
            </h3>
            <div className="flex items-center gap-2">
              <Link
                to="/stock-history"
                className="text-sm text-yellow-700 hover:text-yellow-800 font-medium underline"
              >
                View All History
              </Link>
              <button
                onClick={() => setIsExpanded(!isExpanded)}
                className="text-yellow-700 hover:text-yellow-800"
              >
                {isExpanded ? (
                  <ChevronUpIcon className="h-5 w-5" />
                ) : (
                  <ChevronDownIcon className="h-5 w-5" />
                )}
              </button>
            </div>
          </div>

          {isExpanded && (
            <div className="mt-3 space-y-3">
              {/* Out of Stock Section */}
              {outOfStockProducts.length > 0 && (
                <div className="bg-red-50 border border-red-200 p-3 rounded">
                  <h4 className="text-sm font-semibold text-red-800 mb-2">
                    Out of Stock ({outOfStockProducts.length})
                  </h4>
                  <div className="space-y-2">
                    {displayOutOfStock.map((product) => (
                      <div
                        key={product.id}
                        className="flex justify-between items-center py-2 border-b border-red-200 last:border-b-0"
                      >
                        <div className="flex items-center gap-3 flex-1">
                          <span className="text-sm font-medium text-gray-900">
                            {product.name}
                          </span>
                          <StockBadge
                            quantity={product.stockQuantity}
                            isLowStock={product.isLowStock}
                            size="sm"
                          />
                        </div>
                        {onAddStock && (
                          <Button
                            size="sm"
                            className="bg-green-600 hover:bg-green-700"
                            onClick={() => onAddStock(product)}
                          >
                            Add Stock
                          </Button>
                        )}
                      </div>
                    ))}
                    {moreOutOfStock > 0 && (
                      <p className="text-xs text-red-600 pt-2">
                        ... and {moreOutOfStock} more out of stock
                      </p>
                    )}
                  </div>
                </div>
              )}

              {/* Low Stock Section */}
              {lowStockProducts.length > 0 && (
                <div className="bg-yellow-50 border border-yellow-200 p-3 rounded">
                  <h4 className="text-sm font-semibold text-yellow-800 mb-2">
                    Low Stock ({lowStockProducts.length})
                  </h4>
                  <div className="space-y-2">
                    {displayLowStock.map((product) => (
                      <div
                        key={product.id}
                        className="flex justify-between items-center py-2 border-b border-yellow-200 last:border-b-0"
                      >
                        <div className="flex items-center gap-3 flex-1">
                          <span className="text-sm font-medium text-gray-900">
                            {product.name}
                          </span>
                          <StockBadge
                            quantity={product.stockQuantity}
                            isLowStock={product.isLowStock}
                            size="sm"
                          />
                        </div>
                        {onAddStock && (
                          <Button
                            size="sm"
                            className="bg-green-600 hover:bg-green-700"
                            onClick={() => onAddStock(product)}
                          >
                            Add Stock
                          </Button>
                        )}
                      </div>
                    ))}
                    {moreLowStock > 0 && (
                      <p className="text-xs text-yellow-700 pt-2">
                        ... and {moreLowStock} more low stock items
                      </p>
                    )}
                  </div>
                </div>
              )}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
