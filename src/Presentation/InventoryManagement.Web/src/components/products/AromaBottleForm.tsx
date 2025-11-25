/**
 * AromaBottleForm Component
 * Form for creating or editing Aroma Bottle products
 */

import React, { useState, useEffect } from "react";
import toast from "react-hot-toast";
import Modal from "../common/Modal";
import Input from "../common/Input";
import Textarea from "../common/Textarea";
import Button from "../common/Button";
import Select from "../common/Select";
import TasteSelect from "./TasteSelect";
import type {
  AromaBottleProductDto,
  CreateAromaBottleRequest,
  UpdateAromaBottleRequest,
} from "../../types/product.types";

interface AromaBottleFormProps {
  isOpen: boolean;
  onClose: () => void;
  product?: AromaBottleProductDto;
  onSubmit: (
    data: CreateAromaBottleRequest | UpdateAromaBottleRequest
  ) => Promise<void>;
}

const AromaBottleForm: React.FC<AromaBottleFormProps> = ({
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
    tasteId: product?.tasteId || undefined,
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
      tasteId: product?.tasteId || undefined,
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
      };
      const submitData = product
        ? ({
            ...cleanedData,
            productId: product.id,
          } as UpdateAromaBottleRequest)
        : (cleanedData as CreateAromaBottleRequest);
      await onSubmit(submitData);
      toast.success(
        product
          ? "Aroma Bottle updated successfully"
          : "Aroma Bottle created successfully"
      );
      onClose();
    } catch (error) {
      toast.error((error as Error).message || "Failed to save Aroma Bottle");
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
      title={product ? "Edit Aroma Bottle" : "Create Aroma Bottle"}
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

          {/* Row 2: Currency, Taste */}
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
          <TasteSelect
            value={formData.tasteId}
            onChange={(value) => setFormData({ ...formData, tasteId: value })}
            allowNone={true}
          />

          {/* Row 3: Photo URL */}
          <Input
            label="Photo URL"
            value={formData.photoUrl || ""}
            onChange={(e) =>
              setFormData({ ...formData, photoUrl: e.target.value })
            }
            className="md:col-span-2"
          />

          {/* Row 4: Description */}
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

export default AromaBottleForm;
