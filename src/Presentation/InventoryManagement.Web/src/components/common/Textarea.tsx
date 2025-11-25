/**
 * Textarea Component
 * Reusable textarea component with label, error handling, and react-hook-form compatibility
 */

import React from "react";

interface TextareaProps
  extends React.TextareaHTMLAttributes<HTMLTextAreaElement> {
  label?: string;
  error?: string;
  helperText?: string;
}

const Textarea = React.forwardRef<HTMLTextAreaElement, TextareaProps>(
  (
    {
      label,
      error,
      helperText,
      disabled = false,
      required = false,
      rows = 3,
      className = "",
      ...props
    },
    ref
  ) => {
    const textareaClasses = [
      "w-full px-3 py-2 border rounded-lg shadow-sm focus:outline-none focus:ring-2 resize-y",
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
        <textarea
          ref={ref}
          disabled={disabled}
          required={required}
          rows={rows}
          className={textareaClasses}
          {...props}
        />
        {helperText && !error && (
          <p className="mt-1 text-sm text-gray-500">{helperText}</p>
        )}
        {error && <p className="mt-1 text-sm text-red-600">{error}</p>}
      </div>
    );
  }
);

Textarea.displayName = "Textarea";

export default Textarea;
