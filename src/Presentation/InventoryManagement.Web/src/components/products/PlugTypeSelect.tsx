/**
 * PlugTypeSelect Component
 * Select dropdown for choosing device plug type with optional "None" option
 */

import React from "react";
import Select from "../common/Select";
import { DevicePlugTypeLabels } from "../../types/product.types";

interface PlugTypeSelectProps {
  value: number | undefined;
  onChange: (value: number | undefined) => void;
  label?: string;
  error?: string;
  disabled?: boolean;
  required?: boolean;
  allowNone?: boolean;
}

const PlugTypeSelect: React.FC<PlugTypeSelectProps> = ({
  value,
  onChange,
  label = "Plug Type",
  error,
  disabled = false,
  required = false,
  allowNone = false,
}) => {
  // Create options array from DevicePlugTypeLabels enum
  const options = Object.entries(DevicePlugTypeLabels).map(
    ([value, label]) => ({
      value: Number(value),
      label,
    })
  );

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
      required={required}
    />
  );
};

export default PlugTypeSelect;
