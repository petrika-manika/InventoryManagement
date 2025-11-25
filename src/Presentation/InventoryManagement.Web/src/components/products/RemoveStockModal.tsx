import { useState, useEffect } from "react";
import { toast } from "react-hot-toast";
import { ExclamationTriangleIcon } from "@heroicons/react/24/outline";
import type { ProductDto } from "../../types/product.types";
import stockService from "../../services/stockService";
import Modal from "../common/Modal";
import Input from "../common/Input";
import Textarea from "../common/Textarea";
import Button from "../common/Button";
import StockBadge from "./StockBadge";

interface RemoveStockModalProps {
  isOpen: boolean;
  onClose: () => void;
  product: ProductDto | null;
  onSuccess: () => void;
}

export default function RemoveStockModal({
  isOpen,
  onClose,
  product,
  onSuccess,
}: RemoveStockModalProps) {
  const [formData, setFormData] = useState({
    quantity: 0,
    reason: "",
  });
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [loading, setLoading] = useState(false);
  const [showWarning, setShowWarning] = useState(false);

  const validate = () => {
    if (!product) return false;

    const newErrors: Record<string, string> = {};

    if (!formData.quantity || formData.quantity <= 0) {
      newErrors.quantity = "Quantity must be greater than 0";
    }
    if (formData.quantity > product.stockQuantity) {
      newErrors.quantity = `Cannot remove more than available stock (${product.stockQuantity})`;
    }

    setErrors(newErrors);

    // Check if will be low stock
    const remaining = product.stockQuantity - formData.quantity;
    setShowWarning(remaining > 0 && remaining < 10);

    return Object.keys(newErrors).length === 0;
  };

  // Validate on quantity change
  useEffect(() => {
    if (formData.quantity > 0) {
      validate();
    } else {
      setShowWarning(false);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formData.quantity]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!product || !validate()) return;

    setLoading(true);
    try {
      await stockService.removeStock({
        productId: product.id,
        quantity: formData.quantity,
        reason: formData.reason || undefined,
      });

      toast.success(`Removed ${formData.quantity} units from stock`);
      onSuccess();
      onClose();

      // Reset
      setFormData({ quantity: 0, reason: "" });
      setErrors({});
      setShowWarning(false);
    } catch (error) {
      const message =
        error instanceof Error ? error.message : "Failed to remove stock";
      toast.error(message);
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    onClose();
    // Reset form on close
    setFormData({ quantity: 0, reason: "" });
    setErrors({});
    setShowWarning(false);
  };

  if (!product) return null;

  const remaining = product.stockQuantity - formData.quantity;
  const isInvalid = remaining < 0;

  return (
    <Modal
      isOpen={isOpen}
      onClose={handleClose}
      title="Remove Stock"
      maxWidth="md"
    >
      <form onSubmit={handleSubmit}>
        {/* Product Info */}
        <div className="bg-gray-50 p-4 rounded-lg mb-4">
          <div className="font-bold text-gray-900 mb-2">{product.name}</div>
          <div className="flex items-center gap-2">
            <span className="text-sm text-gray-600">Current Stock:</span>
            <StockBadge
              quantity={product.stockQuantity}
              isLowStock={product.isLowStock}
              size="sm"
            />
          </div>
        </div>

        {/* Form Fields */}
        <div className="space-y-4 mb-4">
          <Input
            label="Quantity to Remove"
            type="number"
            value={formData.quantity || ""}
            onChange={(e) =>
              setFormData({ ...formData, quantity: Number(e.target.value) })
            }
            error={errors.quantity}
            placeholder="Enter quantity"
            min={1}
            max={product.stockQuantity}
            required
          />

          <Textarea
            label="Reason (Optional)"
            value={formData.reason}
            onChange={(e) =>
              setFormData({ ...formData, reason: e.target.value })
            }
            placeholder="e.g., Damaged goods, Customer return, Inventory adjustment"
            rows={3}
          />
        </div>

        {/* Stock Calculation Display */}
        {formData.quantity > 0 && (
          <div className="bg-red-50 border border-red-200 p-3 rounded-lg mb-4">
            <div className="text-sm text-red-900 font-medium">
              Current: {product.stockQuantity} - Removing: {formData.quantity} =
              Remaining: {remaining}
            </div>
          </div>
        )}

        {/* Error if removing more than available */}
        {isInvalid && formData.quantity > 0 && (
          <div className="bg-red-50 border border-red-200 p-3 rounded-lg mb-4 flex items-start gap-2">
            <span className="text-red-600 text-xl">❌</span>
            <div className="flex-1">
              <p className="text-sm text-red-800 font-medium">
                Cannot remove more than available stock
              </p>
            </div>
          </div>
        )}

        {/* Warning if will be low stock */}
        {showWarning && !isInvalid && (
          <div className="bg-yellow-50 border border-yellow-200 p-3 rounded-lg mb-4 flex items-start gap-2">
            <ExclamationTriangleIcon className="h-5 w-5 text-yellow-600 flex-shrink-0" />
            <div className="flex-1">
              <p className="text-sm text-yellow-800 font-medium">
                Warning: Stock will be low after removal
              </p>
            </div>
          </div>
        )}

        {/* Footer Buttons */}
        <div className="flex justify-end gap-3 mt-6">
          <Button
            type="button"
            variant="secondary"
            onClick={handleClose}
            disabled={loading}
          >
            Cancel
          </Button>
          <Button
            type="submit"
            className="bg-red-600 hover:bg-red-700"
            isLoading={loading}
            disabled={isInvalid}
          >
            Remove Stock
          </Button>
        </div>
      </form>
    </Modal>
  );
}
