/**
 * Formatting Utilities
 * Helper functions for formatting dates, currency, stock values, product attributes, and client data
 */

import {
  ProductTypeLabels,
  TasteTypeLabels,
  ColorTypeLabels,
  DevicePlugTypeLabels,
  BatterySizeLabels,
} from "../types/product.types";
import { ClientTypeLabels } from "../types/client.types";

/**
 * Format a number as currency with thousand separators
 * @param amount - The amount to format
 * @param currency - Currency code (default: 'ALL' for Albanian Lek)
 * @returns Formatted currency string
 * @example formatCurrency(1500, 'ALL') → '1,500 ALL'
 */
export function formatCurrency(
  amount: number,
  currency: string = "ALL"
): string {
  const formatter = new Intl.NumberFormat("en-US", {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
  });

  return `${formatter.format(amount)} ${currency}`;
}

/**
 * Format an ISO date string to a readable date
 * @param dateString - ISO date string
 * @returns Formatted date string
 * @example formatDate('2024-11-25T10:30:00Z') → 'Nov 25, 2024'
 */
export function formatDate(dateString: string): string {
  const date = new Date(dateString);
  return new Intl.DateTimeFormat("en-US", {
    year: "numeric",
    month: "short",
    day: "2-digit",
  }).format(date);
}

/**
 * Format an ISO date string to include time
 * @param dateString - ISO date string
 * @returns Formatted date and time string
 * @example formatDateTime('2024-11-25T10:30:00Z') → 'Nov 25, 2024 10:30'
 */
export function formatDateTime(dateString: string): string {
  const date = new Date(dateString);
  return new Intl.DateTimeFormat("en-US", {
    year: "numeric",
    month: "short",
    day: "2-digit",
    hour: "2-digit",
    minute: "2-digit",
    hour12: false,
  }).format(date);
}

/**
 * Format stock quantity change with sign
 * @param quantity - The quantity change (positive for additions, negative for removals)
 * @returns Formatted quantity string with sign
 * @example formatStockChange(50) → '+50'
 * @example formatStockChange(-20) → '-20'
 */
export function formatStockChange(quantity: number): string {
  if (quantity > 0) {
    return `+${quantity}`;
  }
  return quantity.toString();
}

/**
 * Get TailwindCSS color class based on stock quantity change
 * @param quantity - The quantity change
 * @returns TailwindCSS color class
 */
export function getStockChangeColor(quantity: number): string {
  if (quantity > 0) {
    return "text-green-600";
  }
  if (quantity < 0) {
    return "text-red-600";
  }
  return "text-gray-600";
}

/**
 * Get badge variant based on stock status
 * @param stockQuantity - Current stock quantity
 * @param isLowStock - Whether the product is marked as low stock
 * @returns Object with color and label for badge
 */
export function getStockBadgeVariant(
  stockQuantity: number,
  isLowStock: boolean
): { color: string; label: string } {
  if (stockQuantity === 0) {
    return { color: "red", label: "Out of Stock" };
  }
  if (isLowStock) {
    return { color: "yellow", label: "Low Stock" };
  }
  return { color: "green", label: "In Stock" };
}

/**
 * Get human-readable label for product type
 * @param typeId - Product type ID
 * @returns Product type label or 'Unknown'
 */
export function getProductTypeLabel(typeId: number): string {
  return ProductTypeLabels[typeId] || "Unknown";
}

/**
 * Get human-readable label for taste type
 * @param tasteId - Taste type ID (nullable)
 * @returns Taste type label or 'N/A'
 */
export function getTasteTypeLabel(tasteId: number | null): string {
  if (tasteId === null) {
    return "N/A";
  }
  return TasteTypeLabels[tasteId] || "N/A";
}

/**
 * Get human-readable label for color type
 * @param colorId - Color type ID (nullable)
 * @returns Color type label or 'N/A'
 */
export function getColorTypeLabel(colorId: number | null): string {
  if (colorId === null) {
    return "N/A";
  }
  return ColorTypeLabels[colorId] || "N/A";
}

/**
 * Get human-readable label for device plug type
 * @param plugTypeId - Plug type ID
 * @returns Plug type label or 'Unknown'
 */
export function getPlugTypeLabel(plugTypeId: number): string {
  return DevicePlugTypeLabels[plugTypeId] || "Unknown";
}

/**
 * Get human-readable label for battery size
 * @param sizeId - Battery size ID (nullable)
 * @returns Battery size label or 'N/A'
 */
export function getBatterySizeLabel(sizeId: number | null): string {
  if (sizeId === null) {
    return "N/A";
  }
  return BatterySizeLabels[sizeId] || "N/A";
}

// ==================== CLIENT FORMATTERS ====================

/**
 * Get human-readable label for client type
 * @param clientTypeId - Client type ID (1=Individual, 2=Business)
 * @returns Client type label or 'Unknown'
 */
export function getClientTypeLabel(clientTypeId: number): string {
  return ClientTypeLabels[clientTypeId] || "Unknown";
}

/**
 * Format phone number for display
 * @param phoneNumber - Phone number to format (optional)
 * @returns Formatted phone number or '-' if empty
 */
export function formatPhoneNumber(phoneNumber?: string): string {
  if (!phoneNumber || phoneNumber.trim() === "") {
    return "-";
  }
  return phoneNumber;
}

/**
 * Format NIPT (Tax Identification Number) for display
 * Formats NIPT as "K 1234 5678 L" for better readability
 * @param nipt - NIPT to format
 * @returns Formatted NIPT string
 * @example formatNIPT('K12345678L') → 'K 1234 5678 L'
 */
export function formatNIPT(nipt: string): string {
  if (!nipt || nipt.trim() === "") {
    return "-";
  }

  // Remove any existing spaces
  const cleaned = nipt.replace(/\s/g, "");

  // Check if NIPT matches Albanian format (Letter + 8 digits + Letter)
  // e.g., K12345678L
  const match = cleaned.match(/^([A-Z])(\d{4})(\d{4})([A-Z])$/i);

  if (match) {
    // Format as "K 1234 5678 L"
    return `${match[1].toUpperCase()} ${match[2]} ${
      match[3]
    } ${match[4].toUpperCase()}`;
  }

  // If doesn't match expected format, return as-is
  return nipt;
}
