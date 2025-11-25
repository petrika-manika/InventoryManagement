/**
 * BatteryForm Component
 * Form for creating or editing Battery products
 */

import React, { useState, useEffect } from "react";
import toast from "react-hot-toast";
import Modal from "../common/Modal";
import Input from "../common/Input";
import Textarea from "../common/Textarea";
import Button from "../common/Button";
import Select from "../common/Select";
import BatterySizeSelect from "./BatterySizeSelect";
import type {
  BatteryProductDto,
  CreateBatteryRequest,
  UpdateBatteryRequest,
} from "../../types/product.types";

interface BatteryFormProps {
  isOpen: boolean;
  onClose: () => void;
  product?: BatteryProductDto;
  onSubmit: (
    data: CreateBatteryRequest | UpdateBatteryRequest
  ) => Promise<void>;
}

const BatteryForm: React.FC<BatteryFormProps> = ({
  isOpen,
  onClose,
  product,
  onSubmit,
}) => {
  const [formData, setFormData] = useState({
    name: product?.name || "",
    description: product?.description || "",
    price: product?.price || "",
    currency: product?.currency || "ALL",
    photoUrl: product?.photoUrl || "",
    type: product?.type || "",
    sizeId: product?.sizeId || undefined,
    brand: product?.brand || "",
  });

  const [errors, setErrors] = useState<Record<string, string>>({});
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setFormData({
      name: product?.name || "",
      description: product?.description || "",
      price: product?.price || "",
      currency: product?.currency || "ALL",
      photoUrl: product?.photoUrl || "",
      type: product?.type || "",
      sizeId: product?.sizeId || undefined,
      brand: product?.brand || "",
    });
    setErrors({});
  }, [product, isOpen]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    // Validate
    const newErrors: Record<string, string> = {};
    if (!formData.name || formData.name.length < 2) {
      newErrors.name = "Name must be at least 2 characters";
    }
    const priceValue =
      typeof formData.price === "string"
        ? parseFloat(formData.price)
        : formData.price;
    if (!formData.price || isNaN(priceValue) || priceValue <= 0) {
      newErrors.price = "Price must be greater than 0";
    }
    if (!formData.currency) {
      newErrors.currency = "Currency is required";
    }

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    setLoading(true);
    try {
      const cleanedData = {
        ...formData,
        description: formData.description?.trim() || undefined,
        photoUrl: formData.photoUrl?.trim() || undefined,
        type: formData.type?.trim() || undefined,
        brand: formData.brand?.trim() || undefined,
      };
      const submitData = product
        ? ({ ...cleanedData, productId: product.id } as UpdateBatteryRequest)
        : (cleanedData as CreateBatteryRequest);
      await onSubmit(submitData);
      toast.success(
        product
          ? "Battery updated successfully"
          : "Battery created successfully"
      );
      onClose();
    } catch (error) {
      toast.error((error as Error).message || "Failed to save Battery");
    } finally {
      setLoading(false);
    }
  };

  const currencyOptions = [
    { value: "ALL", label: "ALL (Albanian Lek)" },
    { value: "EUR", label: "EUR (Euro)" },
    { value: "USD", label: "USD (US Dollar)" },
  ];

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title={product ? "Edit Battery" : "Create Battery"}
      maxWidth="xl"
    >
      <form onSubmit={handleSubmit}>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          {/* Row 1: Name, Price */}
          <Input
            label="Name"
            value={formData.name}
            onChange={(e) => setFormData({ ...formData, name: e.target.value })}
            error={errors.name}
            required
          />
          <Input
            label="Price"
            type="number"
            value={formData.price}
            onChange={(e) =>
              setFormData({
                ...formData,
                price: e.target.value === "" ? "" : Number(e.target.value),
              })
            }
            error={errors.price}
            min={0}
            step={0.01}
            required
          />

          {/* Row 2: Currency, Battery Type */}
          <Select
            label="Currency"
            options={currencyOptions}
            value={formData.currency}
            onChange={(value) =>
              setFormData({ ...formData, currency: value as string })
            }
            error={errors.currency}
            required
          />
          <Input
            label="Battery Type"
            value={formData.type || ""}
            onChange={(e) => setFormData({ ...formData, type: e.target.value })}
            placeholder="e.g., Lithium Ion, Alkaline"
          />

          {/* Row 3: Battery Size, Brand */}
          <BatterySizeSelect
            value={formData.sizeId}
            onChange={(value) => setFormData({ ...formData, sizeId: value })}
            allowNone={true}
          />
          <Input
            label="Brand"
            value={formData.brand || ""}
            onChange={(e) =>
              setFormData({ ...formData, brand: e.target.value })
            }
            placeholder="e.g., Duracell, Energizer"
          />

          {/* Row 4: Photo URL */}
          <Input
            label="Photo URL"
            value={formData.photoUrl || ""}
            onChange={(e) =>
              setFormData({ ...formData, photoUrl: e.target.value })
            }
            className="md:col-span-2"
          />

          {/* Row 5: Description */}
          <Textarea
            label="Description"
            value={formData.description || ""}
            onChange={(e) =>
              setFormData({ ...formData, description: e.target.value })
            }
            rows={3}
            className="md:col-span-2"
          />
        </div>

        {/* Footer Buttons */}
        <div className="flex justify-end gap-3 mt-6">
          <Button type="button" variant="secondary" onClick={onClose}>
            Cancel
          </Button>
          <Button type="submit" isLoading={loading}>
            {product ? "Update" : "Create"}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default BatteryForm;
