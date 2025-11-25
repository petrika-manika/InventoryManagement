import Badge from "../common/Badge";

interface StockBadgeProps {
  quantity: number;
  isLowStock: boolean;
  size?: "sm" | "md" | "lg"; // default 'md'
  showQuantity?: boolean; // default true
}

export default function StockBadge({
  quantity,
  isLowStock,
  size = "md",
  showQuantity = true,
}: StockBadgeProps) {
  // Map StockBadge size to Badge size (Badge only supports sm/md, so lg maps to md)
  const badgeSize = size === "lg" ? "md" : size;

  let variant: "success" | "danger" | "warning" = "success";
  let label = "";

  if (quantity === 0) {
    variant = "danger";
    label = "Out of Stock";
  } else if (isLowStock) {
    variant = "warning";
    label = showQuantity ? `Low Stock (${quantity})` : "Low Stock";
  } else {
    variant = "success";
    label = showQuantity ? `In Stock (${quantity})` : "In Stock";
  }

  return (
    <Badge variant={variant} size={badgeSize}>
      {label}
    </Badge>
  );
}
