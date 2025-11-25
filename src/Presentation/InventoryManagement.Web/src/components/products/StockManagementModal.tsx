import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { PlusIcon, MinusIcon, ClockIcon } from "@heroicons/react/24/outline";
import type { ProductDto, StockHistoryDto } from "../../types/product.types";
import stockService from "../../services/stockService";
import Modal from "../common/Modal";
import Button from "../common/Button";
import StockBadge from "./StockBadge";
import AddStockModal from "./AddStockModal";
import RemoveStockModal from "./RemoveStockModal";
import { formatDateTime } from "../../utils/formatters";

interface StockManagementModalProps {
  isOpen: boolean;
  onClose: () => void;
  product: ProductDto | null;
  onRefresh: () => void;
}

export default function StockManagementModal({
  isOpen,
  onClose,
  product,
  onRefresh,
}: StockManagementModalProps) {
  const navigate = useNavigate();
  const [recentHistory, setRecentHistory] = useState<StockHistoryDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [showAddModal, setShowAddModal] = useState(false);
  const [showRemoveModal, setShowRemoveModal] = useState(false);

  useEffect(() => {
    if (product && isOpen) {
      loadRecentHistory();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [product, isOpen]);

  const loadRecentHistory = async () => {
    if (!product) return;

    setLoading(true);
    try {
      const history = await stockService.getProductStockHistory(product.id, 5);
      setRecentHistory(history);
    } catch (error) {
      console.error("Failed to load stock history:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleSuccess = () => {
    loadRecentHistory();
    onRefresh();
  };

  const handleViewFullHistory = () => {
    if (product) {
      navigate(`/stock-history?productId=${product.id}`);
      onClose();
    }
  };

  if (!product) return null;

  return (
    <>
      <Modal
        isOpen={isOpen}
        onClose={onClose}
        title="Manage Stock"
        maxWidth="xl"
      >
        {/* Product Info */}
        <div className="bg-gray-50 p-6 rounded-lg mb-6">
          <div className="flex items-start justify-between mb-4">
            <div>
              <h3 className="text-xl font-bold text-gray-900 mb-1">
                {product.name}
              </h3>
              <p className="text-sm text-gray-600">{product.productType}</p>
            </div>
            <StockBadge
              quantity={product.stockQuantity}
              isLowStock={product.isLowStock}
              size="lg"
            />
          </div>
          <div className="text-sm text-gray-600">
            Last updated: {formatDateTime(product.updatedAt)}
          </div>
        </div>

        {/* Quick Actions */}
        <div className="grid grid-cols-3 gap-4 mb-6">
          <button
            onClick={() => setShowAddModal(true)}
            className="p-4 rounded-lg border-2 border-gray-200 hover:border-green-500 hover:bg-green-50 transition-all cursor-pointer text-center"
          >
            <PlusIcon className="h-8 w-8 mx-auto mb-2 text-green-600" />
            <div className="font-semibold text-gray-900">Add Stock</div>
            <div className="text-xs text-gray-600 mt-1">Increase inventory</div>
          </button>

          <button
            onClick={() => setShowRemoveModal(true)}
            className="p-4 rounded-lg border-2 border-gray-200 hover:border-red-500 hover:bg-red-50 transition-all cursor-pointer text-center"
          >
            <MinusIcon className="h-8 w-8 mx-auto mb-2 text-red-600" />
            <div className="font-semibold text-gray-900">Remove Stock</div>
            <div className="text-xs text-gray-600 mt-1">Decrease inventory</div>
          </button>

          <button
            onClick={handleViewFullHistory}
            className="p-4 rounded-lg border-2 border-gray-200 hover:border-blue-500 hover:bg-blue-50 transition-all cursor-pointer text-center"
          >
            <ClockIcon className="h-8 w-8 mx-auto mb-2 text-blue-600" />
            <div className="font-semibold text-gray-900">View Full History</div>
            <div className="text-xs text-gray-600 mt-1">All transactions</div>
          </button>
        </div>

        {/* Recent History */}
        <div>
          <div className="flex items-center justify-between mb-3">
            <h4 className="font-semibold text-gray-900">
              Recent Changes (Last 5)
            </h4>
            <button
              onClick={handleViewFullHistory}
              className="text-sm text-primary-600 hover:text-primary-700 font-medium"
            >
              View all history →
            </button>
          </div>

          {loading ? (
            <div className="flex justify-center py-8">
              <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary-600"></div>
            </div>
          ) : recentHistory.length === 0 ? (
            <div className="text-center py-8 bg-gray-50 rounded-lg">
              <p className="text-sm text-gray-600">No stock history yet</p>
            </div>
          ) : (
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200 text-sm">
                <thead className="bg-gray-50">
                  <tr>
                    <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Date
                    </th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Type
                    </th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Quantity
                    </th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Reason
                    </th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  {recentHistory.map((item) => (
                    <tr key={item.id}>
                      <td className="px-4 py-3 whitespace-nowrap text-gray-900">
                        {formatDateTime(item.changedAt)}
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap">
                        <span
                          className={`inline-flex items-center px-2 py-0.5 rounded text-xs font-medium ${
                            item.changeType === "Added"
                              ? "bg-green-100 text-green-800"
                              : "bg-red-100 text-red-800"
                          }`}
                        >
                          {item.changeType}
                        </span>
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap">
                        <span
                          className={`font-medium ${
                            item.changeType === "Added"
                              ? "text-green-600"
                              : "text-red-600"
                          }`}
                        >
                          {item.quantityChanged > 0 ? "+" : ""}
                          {item.quantityChanged}
                        </span>
                      </td>
                      <td className="px-4 py-3 text-gray-600">
                        {item.reason || "-"}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>

        {/* Footer */}
        <div className="flex justify-end mt-6">
          <Button variant="secondary" onClick={onClose}>
            Close
          </Button>
        </div>
      </Modal>

      {/* Nested Modals */}
      <AddStockModal
        isOpen={showAddModal}
        onClose={() => setShowAddModal(false)}
        product={product}
        onSuccess={handleSuccess}
      />

      <RemoveStockModal
        isOpen={showRemoveModal}
        onClose={() => setShowRemoveModal(false)}
        product={product}
        onSuccess={handleSuccess}
      />
    </>
  );
}
