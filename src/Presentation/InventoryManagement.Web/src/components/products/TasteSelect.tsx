/**
 * TasteSelect Component
 * Select dropdown for choosing taste type with optional "None" option
 */

import React from "react";
import Select from "../common/Select";
import { TasteTypeLabels } from "../../types/product.types";

interface TasteSelectProps {
  value: number | undefined;
  onChange: (value: number | undefined) => void;
  label?: string;
  error?: string;
  disabled?: boolean;
  allowNone?: boolean;
}

const TasteSelect: React.FC<TasteSelectProps> = ({
  value,
  onChange,
  label = "Taste",
  error,
  disabled = false,
  allowNone = false,
}) => {
  // Create options array from TasteTypeLabels enum
  const options = Object.entries(TasteTypeLabels).map(([value, label]) => ({
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

export default TasteSelect;
