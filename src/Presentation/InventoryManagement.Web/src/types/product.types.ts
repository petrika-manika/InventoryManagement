/**
 * Product Type Enumeration
 * Defines the different types of products in the inventory system
 */
export const ProductType = {
  AromaBombel: 1,
  AromaBottle: 2,
  AromaDevice: 3,
  SanitizingDevice: 4,
  Battery: 5,
} as const;

export type ProductTypeValue = (typeof ProductType)[keyof typeof ProductType];

/**
 * Product Type Display Labels
 * Maps product type IDs to user-friendly names
 */
export const ProductTypeLabels: Record<number, string> = {
  1: "Aroma Bombel",
  2: "Aroma Bottle",
  3: "Aroma Device",
  4: "Sanitizing Device",
  5: "Battery",
};

/**
 * Taste Type Enumeration
 * Defines available taste/scent types for aroma products
 */
export const TasteType = {
  Flower: 1,
  Sweet: 2,
  Fresh: 3,
  Fruit: 4,
} as const;

export type TasteTypeValue = (typeof TasteType)[keyof typeof TasteType];

/**
 * Taste Type Display Labels
 * Maps taste type IDs to user-friendly names
 */
export const TasteTypeLabels: Record<number, string> = {
  1: "Flower",
  2: "Sweet",
  3: "Fresh",
  4: "Fruit",
};

/**
 * Color Type Enumeration
 * Defines available colors for devices
 */
export const ColorType = {
  Red: 1,
  Blue: 2,
  Green: 3,
  Yellow: 4,
  Orange: 5,
  Purple: 6,
  Pink: 7,
  Brown: 8,
  Black: 9,
  White: 10,
  Gray: 11,
} as const;

export type ColorTypeValue = (typeof ColorType)[keyof typeof ColorType];

/**
 * Color Type Display Labels
 * Maps color type IDs to user-friendly names
 */
export const ColorTypeLabels: Record<number, string> = {
  1: "Red",
  2: "Blue",
  3: "Green",
  4: "Yellow",
  5: "Orange",
  6: "Purple",
  7: "Pink",
  8: "Brown",
  9: "Black",
  10: "White",
  11: "Gray",
};

/**
 * Device Plug Type Enumeration
 * Defines whether a device has a plug or not
 */
export const DevicePlugType = {
  WithPlug: 1,
  WithoutPlug: 2,
} as const;

export type DevicePlugTypeValue =
  (typeof DevicePlugType)[keyof typeof DevicePlugType];

/**
 * Device Plug Type Display Labels
 * Maps plug type IDs to user-friendly names
 */
export const DevicePlugTypeLabels: Record<number, string> = {
  1: "With Plug",
  2: "Without Plug",
};

/**
 * Battery Size Enumeration
 * Defines standard battery sizes (LR6 = AA, LR9 = AAA)
 */
export const BatterySize = {
  LR6: 1, // AA battery
  LR9: 2, // AAA battery
} as const;

export type BatterySizeValue = (typeof BatterySize)[keyof typeof BatterySize];

/**
 * Battery Size Display Labels
 * Maps battery size IDs to user-friendly names
 */
export const BatterySizeLabels: Record<number, string> = {
  1: "LR6 (AA)",
  2: "LR9 (AAA)",
};

/**
 * Base Product DTO
 * Common properties shared by all product types
 */
export interface ProductDto {
  /** Unique product identifier */
  id: string;
  /** Product name */
  name: string;
  /** Product description */
  description: string | null;
  /** Product type name (e.g., "AromaBombel") */
  productType: string;
  /** Product type ID */
  productTypeId: number;
  /** Product price */
  price: number;
  /** Currency code (e.g., "ALL") */
  currency: string;
  /** Product photo URL */
  photoUrl: string | null;
  /** Current stock quantity */
  stockQuantity: number;
  /** Whether product is active/available */
  isActive: boolean;
  /** Whether stock is below threshold */
  isLowStock: boolean;
  /** Creation timestamp */
  createdAt: string;
  /** Last update timestamp */
  updatedAt: string;
}

/**
 * Aroma Bombel Product DTO
 * Represents an aroma bombel product with taste property
 */
export interface AromaBombelProductDto extends ProductDto {
  /** Taste/scent name */
  taste: string | null;
  /** Taste type ID */
  tasteId: number | null;
}

/**
 * Aroma Bottle Product DTO
 * Represents an aroma bottle product with taste property
 */
export interface AromaBottleProductDto extends ProductDto {
  /** Taste/scent name */
  taste: string | null;
  /** Taste type ID */
  tasteId: number | null;
}

/**
 * Aroma Device Product DTO
 * Represents an aroma device with color, format, and coverage properties
 */
export interface AromaDeviceProductDto extends ProductDto {
  /** Device color name */
  color: string | null;
  /** Color type ID */
  colorId: number | null;
  /** Device format description */
  format: string | null;
  /** Device programs description */
  programs: string | null;
  /** Plug type name */
  plugType: string;
  /** Plug type ID */
  plugTypeId: number;
  /** Coverage area in square meters */
  squareMeter: number | null;
}

/**
 * Sanitizing Device Product DTO
 * Represents a sanitizing device (no squareMeter property)
 */
export interface SanitizingDeviceProductDto extends ProductDto {
  /** Device color name */
  color: string | null;
  /** Color type ID */
  colorId: number | null;
  /** Device format description */
  format: string | null;
  /** Device programs description */
  programs: string | null;
  /** Plug type name */
  plugType: string;
  /** Plug type ID */
  plugTypeId: number;
}

/**
 * Battery Product DTO
 * Represents a battery product with type, size, and brand
 */
export interface BatteryProductDto extends ProductDto {
  /** Battery type (e.g., "Lithium Ion") */
  type: string | null;
  /** Battery size name (e.g., "LR6") */
  size: string | null;
  /** Battery size ID */
  sizeId: number | null;
  /** Battery brand */
  brand: string | null;
}

/**
 * Create Aroma Bombel Request
 * Data required to create a new aroma bombel product
 */
export interface CreateAromaBombelRequest {
  name: string;
  description?: string;
  price: number;
  currency: string;
  photoUrl?: string;
  tasteId?: number;
}

/**
 * Create Aroma Bottle Request
 * Data required to create a new aroma bottle product
 */
export interface CreateAromaBottleRequest {
  name: string;
  description?: string;
  price: number;
  currency: string;
  photoUrl?: string;
  tasteId?: number;
}

/**
 * Create Aroma Device Request
 * Data required to create a new aroma device product
 */
export interface CreateAromaDeviceRequest {
  name: string;
  description?: string;
  price: number;
  currency: string;
  photoUrl?: string;
  colorId?: number;
  format?: string;
  programs?: string;
  plugTypeId?: number;
  squareMeter?: number;
}

/**
 * Create Sanitizing Device Request
 * Data required to create a new sanitizing device product
 */
export interface CreateSanitizingDeviceRequest {
  name: string;
  description?: string;
  price: number;
  currency: string;
  photoUrl?: string;
  colorId?: number;
  format?: string;
  programs?: string;
  plugTypeId?: number;
}

/**
 * Create Battery Request
 * Data required to create a new battery product
 */
export interface CreateBatteryRequest {
  name: string;
  description?: string;
  price: number;
  currency: string;
  photoUrl?: string;
  type?: string;
  sizeId?: number;
  brand?: string;
}

/**
 * Update Aroma Bombel Request
 * Data required to update an existing aroma bombel product
 */
export interface UpdateAromaBombelRequest extends CreateAromaBombelRequest {
  productId: string;
}

/**
 * Update Aroma Bottle Request
 * Data required to update an existing aroma bottle product
 */
export interface UpdateAromaBottleRequest extends CreateAromaBottleRequest {
  productId: string;
}

/**
 * Update Aroma Device Request
 * Data required to update an existing aroma device product
 */
export interface UpdateAromaDeviceRequest extends CreateAromaDeviceRequest {
  productId: string;
}

/**
 * Update Sanitizing Device Request
 * Data required to update an existing sanitizing device product
 */
export interface UpdateSanitizingDeviceRequest
  extends CreateSanitizingDeviceRequest {
  productId: string;
}

/**
 * Update Battery Request
 * Data required to update an existing battery product
 */
export interface UpdateBatteryRequest extends CreateBatteryRequest {
  productId: string;
}

/**
 * Add Stock Request
 * Data required to add stock quantity to a product
 */
export interface AddStockRequest {
  /** Product ID to add stock to */
  productId: string;
  /** Quantity to add (must be positive) */
  quantity: number;
  /** Optional reason for adding stock */
  reason?: string;
}

/**
 * Remove Stock Request
 * Data required to remove stock quantity from a product
 */
export interface RemoveStockRequest {
  /** Product ID to remove stock from */
  productId: string;
  /** Quantity to remove (must be positive) */
  quantity: number;
  /** Optional reason for removing stock */
  reason?: string;
}

/**
 * Stock History DTO
 * Represents a stock change record in the history
 */
export interface StockHistoryDto {
  /** History record ID */
  id: string;
  /** Product ID */
  productId: string;
  /** Product name at time of change */
  productName: string;
  /** Quantity changed (positive for add, negative for remove) */
  quantityChanged: number;
  /** Stock quantity after change */
  quantityAfter: number;
  /** Type of change (Added, Removed, Adjusted) */
  changeType: string;
  /** Reason for stock change */
  reason: string | null;
  /** User ID who made the change */
  changedBy: string;
  /** Full name of user who made the change */
  changedByName: string;
  /** Timestamp of change */
  changedAt: string;
}
