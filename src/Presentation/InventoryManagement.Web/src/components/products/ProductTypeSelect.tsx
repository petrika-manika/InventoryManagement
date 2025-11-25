/**
 * ProductTypeSelect Component
 * Select dropdown for choosing product type with optional "All Types" filter option
 */

import React from "react";
import Select from "../common/Select";
import { ProductTypeLabels } from "../../types/product.types";

interface ProductTypeSelectProps {
  value: number | undefined;
  onChange: (value: number) => void;
  label?: string;
  error?: string;
  disabled?: boolean;
  showAll?: boolean;
}

const ProductTypeSelect: React.FC<ProductTypeSelectProps> = ({
  value,
  onChange,
  label = "Product Type",
  error,
  disabled = false,
  showAll = false,
}) => {
  // Create options array from ProductTypeLabels enum
  const options = Object.entries(ProductTypeLabels).map(([value, label]) => ({
    value: Number(value),
    label,
  }));

  // Add "All Types" option if showAll is true
  if (showAll) {
    options.unshift({ value: 0, label: "All Types" });
  }

  return (
    <Select
      label={label}
      options={options}
      value={value}
      onChange={(val) => onChange(Number(val))}
      error={error}
      disabled={disabled}
    />
  );
};

export default ProductTypeSelect;
