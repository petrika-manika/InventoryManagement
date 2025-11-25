import type { ReactNode } from "react";

/**
 * Badge component props
 */
export interface BadgeProps {
  /** Badge content */
  children: ReactNode;
  /** Visual style variant */
  variant?: "success" | "danger" | "warning" | "info";
  /** Badge size */
  size?: "sm" | "md";
}

/**
 * Badge Component
 * Displays status indicators with different colors and sizes
 *
 * @example
 * ```tsx
 * <Badge variant="success">Active</Badge>
 * <Badge variant="danger">Inactive</Badge>
 * <Badge variant="warning" size="md">Pending</Badge>
 * ```
 */
const Badge = ({ children, variant = "info", size = "sm" }: BadgeProps) => {
  // Base classes
  const baseClasses = "inline-flex items-center font-medium rounded-full";

  // Size-specific classes
  const sizeClasses = {
    sm: "px-2.5 py-0.5 text-xs",
    md: "px-3 py-1 text-sm",
  };

  // Variant-specific classes
  const variantClasses = {
    success: "bg-green-100 text-green-800",
    danger: "bg-red-100 text-red-800",
    warning: "bg-yellow-100 text-yellow-800",
    info: "bg-blue-100 text-blue-800",
  };

  const classes = [
    baseClasses,
    sizeClasses[size],
    variantClasses[variant],
  ].join(" ");

  return <span className={classes}>{children}</span>;
};

export default Badge;
