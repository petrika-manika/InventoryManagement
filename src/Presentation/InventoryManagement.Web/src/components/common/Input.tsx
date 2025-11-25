import { forwardRef } from "react";
import type { InputHTMLAttributes } from "react";

/**
 * Input component props
 * Extends standard HTML input attributes
 */
export interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  /** Label text displayed above the input */
  label?: string;
  /** Error message displayed below the input */
  error?: string;
  /** Helper text displayed below the input when no error */
  helperText?: string;
}

/**
 * Reusable Input Component with label, error, and helper text support
 * Uses forwardRef for compatibility with form libraries like react-hook-form
 *
 * @example
 * ```tsx
 * <Input
 *   label="Email"
 *   type="email"
 *   placeholder="Enter your email"
 *   error={errors.email?.message}
 *   {...register('email')}
 * />
 * ```
 */
const Input = forwardRef<HTMLInputElement, InputProps>(
  ({ label, error, helperText, className = "", ...props }, ref) => {
    // Base input styles
    const baseClasses =
      "w-full px-3 py-2 border rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-0 transition-colors";

    // Conditional styles based on error state
    const stateClasses = error
      ? "border-red-300 focus:border-red-500 focus:ring-red-500"
      : "border-gray-300 focus:border-primary-500 focus:ring-primary-500";

    const inputClasses = [baseClasses, stateClasses, className]
      .filter(Boolean)
      .join(" ");

    return (
      <div className="mb-4">
        {label && (
          <label className="block text-sm font-medium text-gray-700 mb-1">
            {label}
          </label>
        )}
        <input ref={ref} className={inputClasses} {...props} />
        {error && <p className="mt-1 text-sm text-red-600">{error}</p>}
        {helperText && !error && (
          <p className="mt-1 text-sm text-gray-500">{helperText}</p>
        )}
      </div>
    );
  }
);

Input.displayName = "Input";

export default Input;
