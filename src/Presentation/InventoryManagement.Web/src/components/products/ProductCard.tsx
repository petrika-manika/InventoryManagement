/**
 * ProductCard Component
 * Displays a single product in a card layout with image, details, and action buttons
 */

import React from "react";
import type { ProductDto } from "../../types/product.types";
import {
  formatCurrency,
  getProductTypeLabel,
  getTasteTypeLabel,
  getColorTypeLabel,
} from "../../utils/formatters";
import {
  PencilIcon,
  TrashIcon,
  ChartBarIcon,
  CubeIcon,
} from "@heroicons/react/24/outline";
import StockBadge from "./StockBadge";

interface ProductCardProps {
  product: ProductDto;
  onEdit: (product: ProductDto) => void;
  onDelete: (product: ProductDto) => void;
  onManageStock: (product: ProductDto) => void;
}

const ProductCard: React.FC<ProductCardProps> = ({
  product,
  onEdit,
  onDelete,
  onManageStock,
}) => {
  // Truncate description if longer than 100 characters
  const truncatedDescription =
    product.description && product.description.length > 100
      ? `${product.description.substring(0, 100)}...`
      : product.description;

  // Check if product has taste (for AromaBombel and AromaBottle)
  const hasTaste =
    "tasteId" in product &&
    (product as { tasteId?: number | null }).tasteId !== null;
  const tasteId = hasTaste ? (product as { tasteId: number }).tasteId : null;

  // Check if product has color (for devices)
  const hasColor =
    "colorId" in product &&
    (product as { colorId?: number | null }).colorId !== null;
  const colorId = hasColor ? (product as { colorId: number }).colorId : null;

  return (
    <div className="bg-white rounded-lg shadow hover:shadow-xl transition-shadow cursor-pointer border border-gray-200 overflow-hidden">
      {/* Product Image Section */}
      <div className="h-48 w-full bg-gray-200 flex items-center justify-center">
        {product.photoUrl ? (
          <img
            src={product.photoUrl}
            alt={product.name}
            className="h-full w-full object-cover"
          />
        ) : (
          <CubeIcon className="h-16 w-16 text-gray-400" />
        )}
      </div>

      {/* Product Info Section */}
      <div className="p-4 space-y-3">
        {/* Product Name */}
        <h3 className="text-lg font-bold text-gray-900">{product.name}</h3>

        {/* Badges Row */}
        <div className="flex flex-wrap gap-2 items-center">
          {/* Product Type Badge */}
          <span className="px-2 py-1 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
            {getProductTypeLabel(product.productTypeId)}
          </span>

          {/* Taste Badge (if applicable) */}
          {hasTaste && (
            <span className="px-2 py-1 rounded-full text-xs font-medium bg-green-100 text-green-800">
              {getTasteTypeLabel(tasteId)}
            </span>
          )}

          {/* Color Badge (if applicable) */}
          {hasColor && (
            <span className="px-2 py-1 rounded-full text-xs font-medium bg-purple-100 text-purple-800">
              {getColorTypeLabel(colorId)}
            </span>
          )}

          {/* Stock Badge */}
          <StockBadge
            quantity={product.stockQuantity}
            isLowStock={product.isLowStock}
            size="sm"
          />
        </div>

        {/* Price */}
        <div className="text-xl font-bold text-primary-600">
          {formatCurrency(product.price, product.currency)}
        </div>

        {/* Description */}
        {truncatedDescription && (
          <p className="text-sm text-gray-600">{truncatedDescription}</p>
        )}

        {/* Action Buttons */}
        <div className="flex gap-2 mt-4 pt-4 border-t border-gray-200">
          <button
            onClick={() => onEdit(product)}
            className="flex-1 flex items-center justify-center gap-1 px-3 py-2 rounded-lg font-medium transition-colors text-sm bg-blue-500 hover:bg-blue-600 text-white"
            title="Edit Product"
          >
            <PencilIcon className="h-4 w-4" />
            <span>Edit</span>
          </button>

          <button
            onClick={() => onManageStock(product)}
            className="flex-1 flex items-center justify-center gap-1 px-3 py-2 rounded-lg font-medium transition-colors text-sm bg-purple-500 hover:bg-purple-600 text-white"
            title="Manage Stock"
          >
            <ChartBarIcon className="h-4 w-4" />
            <span>Stock</span>
          </button>

          <button
            onClick={() => onDelete(product)}
            className="flex-1 flex items-center justify-center gap-1 px-3 py-2 rounded-lg font-medium transition-colors text-sm bg-red-500 hover:bg-red-600 text-white"
            title="Delete Product"
          >
            <TrashIcon className="h-4 w-4" />
            <span>Delete</span>
          </button>
        </div>
      </div>
    </div>
  );
};

export default ProductCard;
