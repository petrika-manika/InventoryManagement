import { useState } from "react";
import { toast } from "react-hot-toast";
import type { ProductDto } from "../../types/product.types";
import stockService from "../../services/stockService";
import Modal from "../common/Modal";
import Input from "../common/Input";
import Textarea from "../common/Textarea";
import Button from "../common/Button";
import StockBadge from "./StockBadge";

interface AddStockModalProps {
  isOpen: boolean;
  onClose: () => void;
  product: ProductDto | null;
  onSuccess: () => void; // Callback to refresh products
}

export default function AddStockModal({
  isOpen,
  onClose,
  product,
  onSuccess,
}: AddStockModalProps) {
  const [formData, setFormData] = useState({
    quantity: 0,
    reason: "",
  });
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [loading, setLoading] = useState(false);

  const validate = () => {
    const newErrors: Record<string, string> = {};

    if (!formData.quantity || formData.quantity <= 0) {
      newErrors.quantity = "Quantity must be greater than 0";
    }
    if (formData.quantity > 1000) {
      newErrors.quantity = "Quantity cannot exceed 1000";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!product || !validate()) return;

    setLoading(true);
    try {
      await stockService.addStock({
        productId: product.id,
        quantity: formData.quantity,
        reason: formData.reason || undefined,
      });

      toast.success(`Added ${formData.quantity} units to stock`);
      onSuccess();
      onClose();

      // Reset form
      setFormData({ quantity: 0, reason: "" });
      setErrors({});
    } catch (error) {
      const message =
        error instanceof Error ? error.message : "Failed to add stock";
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
  };

  if (!product) return null;

  const newStock = product.stockQuantity + formData.quantity;

  return (
    <Modal
      isOpen={isOpen}
      onClose={handleClose}
      title="Add Stock"
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
            label="Quantity to Add"
            type="number"
            value={formData.quantity || ""}
            onChange={(e) =>
              setFormData({ ...formData, quantity: Number(e.target.value) })
            }
            error={errors.quantity}
            placeholder="Enter quantity"
            min={1}
            max={1000}
            required
          />

          <Textarea
            label="Reason (Optional)"
            value={formData.reason}
            onChange={(e) =>
              setFormData({ ...formData, reason: e.target.value })
            }
            placeholder="e.g., New shipment, Stock replenishment"
            rows={3}
          />
        </div>

        {/* Stock Calculation Display */}
        {formData.quantity > 0 && (
          <div className="bg-blue-50 border border-blue-200 p-3 rounded-lg mb-4">
            <div className="text-sm text-blue-900 font-medium">
              Current: {product.stockQuantity} + Adding: {formData.quantity} =
              New Stock: {newStock}
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
            className="bg-green-600 hover:bg-green-700"
            isLoading={loading}
          >
            Add Stock
          </Button>
        </div>
      </form>
    </Modal>
  );
}
