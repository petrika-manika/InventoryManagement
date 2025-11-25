/**
 * SanitizingDeviceForm Component
 * Form for creating or editing Sanitizing Device products
 */

import React, { useState, useEffect } from "react";
import toast from "react-hot-toast";
import Modal from "../common/Modal";
import Input from "../common/Input";
import Textarea from "../common/Textarea";
import Button from "../common/Button";
import Select from "../common/Select";
import ColorSelect from "./ColorSelect";
import PlugTypeSelect from "./PlugTypeSelect";
import type {
  SanitizingDeviceProductDto,
  CreateSanitizingDeviceRequest,
  UpdateSanitizingDeviceRequest,
} from "../../types/product.types";

interface SanitizingDeviceFormProps {
  isOpen: boolean;
  onClose: () => void;
  product?: SanitizingDeviceProductDto;
  onSubmit: (
    data: CreateSanitizingDeviceRequest | UpdateSanitizingDeviceRequest
  ) => Promise<void>;
}

const SanitizingDeviceForm: React.FC<SanitizingDeviceFormProps> = ({
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
    colorId: product?.colorId || undefined,
    format: product?.format || "",
    programs: product?.programs || "",
    plugTypeId: product?.plugTypeId || undefined,
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
      colorId: product?.colorId || undefined,
      format: product?.format || "",
      programs: product?.programs || "",
      plugTypeId: product?.plugTypeId || undefined,
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
        format: formData.format?.trim() || undefined,
        programs: formData.programs?.trim() || undefined,
      };
      const submitData = product
        ? ({
            ...cleanedData,
            productId: product.id,
          } as UpdateSanitizingDeviceRequest)
        : (cleanedData as CreateSanitizingDeviceRequest);
      await onSubmit(submitData);
      toast.success(
        product
          ? "Sanitizing Device updated successfully"
          : "Sanitizing Device created successfully"
      );
      onClose();
    } catch (error) {
      toast.error(
        (error as Error).message || "Failed to save Sanitizing Device"
      );
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
      title={product ? "Edit Sanitizing Device" : "Create Sanitizing Device"}
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

          {/* Row 2: Currency, Color */}
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
          <ColorSelect
            value={formData.colorId}
            onChange={(value) => setFormData({ ...formData, colorId: value })}
            allowNone={true}
          />

          {/* Row 3: Plug Type, Photo URL */}
          <PlugTypeSelect
            value={formData.plugTypeId}
            onChange={(value) =>
              setFormData({ ...formData, plugTypeId: value })
            }
            error={errors.plugTypeId}
            allowNone={true}
          />
          <Input
            label="Photo URL"
            value={formData.photoUrl || ""}
            onChange={(e) =>
              setFormData({ ...formData, photoUrl: e.target.value })
            }
          />

          {/* Row 4: Format */}
          <Textarea
            label="Format"
            value={formData.format || ""}
            onChange={(e) =>
              setFormData({ ...formData, format: e.target.value })
            }
            rows={2}
            className="md:col-span-2"
          />

          {/* Row 5: Programs */}
          <Textarea
            label="Programs"
            value={formData.programs || ""}
            onChange={(e) =>
              setFormData({ ...formData, programs: e.target.value })
            }
            rows={2}
            className="md:col-span-2"
          />

          {/* Row 6: Description */}
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

export default SanitizingDeviceForm;
