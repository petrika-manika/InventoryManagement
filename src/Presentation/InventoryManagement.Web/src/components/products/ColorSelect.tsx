/**
 * ColorSelect Component
 * Select dropdown for choosing color type with optional "None" option
 */

import React from "react";
import Select from "../common/Select";
import { ColorTypeLabels } from "../../types/product.types";

interface ColorSelectProps {
  value: number | undefined;
  onChange: (value: number | undefined) => void;
  label?: string;
  error?: string;
  disabled?: boolean;
  allowNone?: boolean;
}

const ColorSelect: React.FC<ColorSelectProps> = ({
  value,
  onChange,
  label = "Color",
  error,
  disabled = false,
  allowNone = false,
}) => {
  // Create options array from ColorTypeLabels enum
  const options = Object.entries(ColorTypeLabels).map(([value, label]) => ({
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

export default ColorSelect;
