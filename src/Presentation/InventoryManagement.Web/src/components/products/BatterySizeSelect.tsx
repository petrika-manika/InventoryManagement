/**
 * BatterySizeSelect Component
 * Select dropdown for choosing battery size with optional "None" option
 */

import React from "react";
import Select from "../common/Select";
import { BatterySizeLabels } from "../../types/product.types";

interface BatterySizeSelectProps {
  value: number | undefined;
  onChange: (value: number | undefined) => void;
  label?: string;
  error?: string;
  disabled?: boolean;
  allowNone?: boolean;
}

const BatterySizeSelect: React.FC<BatterySizeSelectProps> = ({
  value,
  onChange,
  label = "Battery Size",
  error,
  disabled = false,
  allowNone = false,
}) => {
  // Create options array from BatterySizeLabels enum
  const options = Object.entries(BatterySizeLabels).map(([value, label]) => ({
    value: Number(value),
    label,
  }));

  const handleChange = (val: string | number) => {
    if (val === "" || val === undefined) {
      onChange(undefined);
    } else {
      onChange(Number(val));
    }
  };

  return (
    <Select
      label={label}
      options={options}
      value={value}
      onChange={handleChange}
      placeholder={allowNone ? "None" : undefined}
      error={error}
      disabled={disabled}
    />
  );
};

export default BatterySizeSelect;
