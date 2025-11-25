/**
 * Select Component
 * Reusable dropdown select component with label, error handling, and react-hook-form compatibility
 */

import React from "react";

interface SelectProps {
  label?: string;
  options: Array<{ value: string | number; label: string }>;
  value: string | number | undefined;
  onChange: (value: string | number) => void;
  placeholder?: string;
  error?: string;
  disabled?: boolean;
  required?: boolean;
  className?: string;
}

const Select = React.forwardRef<HTMLSelectElement, SelectProps>(
  (
    {
      label,
      options,
      value,
      onChange,
      placeholder,
      error,
      disabled = false,
      required = false,
      className = "",
      ...props
    },
    ref
  ) => {
    const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
      const selectedValue = e.target.value;
      // Convert to number if the option value is a number
      const option = options.find(
        (opt) => opt.value.toString() === selectedValue
      );
      if (option) {
        onChange(option.value);
      } else if (selectedValue === "") {
        onChange("");
      }
    };

    const selectClasses = [
      "w-full px-3 py-2 border rounded-lg shadow-sm focus:outline-none focus:ring-2",
      error
        ? "border-red-300 focus:border-red-500 focus:ring-red-500"
        : "border-gray-300 focus:border-primary-500 focus:ring-primary-500",
      disabled ? "bg-gray-100 cursor-not-allowed opacity-60" : "",
      className,
    ]
      .filter(Boolean)
      .join(" ");

    return (
      <div>
        {label && (
          <label className="block text-sm font-medium text-gray-700 mb-1">
            {label}
            {required && <span className="text-red-500 ml-1">*</span>}
          </label>
        )}
        <select
          ref={ref}
          value={value ?? ""}
          onChange={handleChange}
          disabled={disabled}
          className={selectClasses}
          {...props}
        >
          {placeholder && (
            <option value="" disabled>
              {placeholder}
            </option>
          )}
          {options.map((option) => (
            <option key={option.value} value={option.value}>
              {option.label}
            </option>
          ))}
        </select>
        {error && <p className="mt-1 text-sm text-red-600">{error}</p>}
      </div>
    );
  }
);

Select.displayName = "Select";

export default Select;
