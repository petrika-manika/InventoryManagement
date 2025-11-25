/**
 * ProductTable Component
 * Displays products in a table layout with sortable columns and action buttons
 */

import React from "react";
import type {
  ProductDto,
  AromaBombelProductDto,
  AromaBottleProductDto,
  AromaDeviceProductDto,
  SanitizingDeviceProductDto,
} from "../../types/product.types";
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
import LoadingSpinner from "../common/LoadingSpinner";
import StockBadge from "./StockBadge";
import {
  Table,
  TableHeader,
  TableBody,
  TableRow,
  TableHead,
  TableCell,
} from "../common/Table";

interface ProductTableProps {
  products: ProductDto[];
  onEdit: (product: ProductDto) => void;
  onDelete: (product: ProductDto) => void;
  onManageStock: (product: ProductDto) => void;
  loading?: boolean;
}

const ProductTable: React.FC<ProductTableProps> = ({
  products,
  onEdit,
  onDelete,
  onManageStock,
  loading = false,
}) => {
  /**
   * Get taste or color badge if applicable
   */
  const getAttributeBadge = (product: ProductDto) => {
    // Check product type and cast accordingly
    // AromaBombel (1) and AromaBottle (2) have taste
    if (product.productTypeId === 1 || product.productTypeId === 2) {
      const productWithTaste = product as
        | AromaBombelProductDto
        | AromaBottleProductDto;
      if (productWithTaste.tasteId) {
        return {
          label: getTasteTypeLabel(productWithTaste.tasteId),
          color: "bg-green-100 text-green-800",
        };
      }
    }

    // AromaDevice (3) and SanitizingDevice (4) have color
    if (product.productTypeId === 3 || product.productTypeId === 4) {
      const productWithColor = product as
        | AromaDeviceProductDto
        | SanitizingDeviceProductDto;
      if (productWithColor.colorId) {
        return {
          label: getColorTypeLabel(productWithColor.colorId),
          color: "bg-purple-100 text-purple-800",
        };
      }
    }

    return null;
  };

  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Image</TableHead>
          <TableHead>Name</TableHead>
          <TableHead>Type</TableHead>
          <TableHead>Attribute</TableHead>
          <TableHead>Price</TableHead>
          <TableHead>Stock</TableHead>
          <TableHead>Actions</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {/* Loading State */}
        {loading && (
          <TableRow>
            <TableCell className="text-center" colSpan={7}>
              <div className="py-8">
                <LoadingSpinner size="lg" />
              </div>
            </TableCell>
          </TableRow>
        )}

        {/* Empty State */}
        {products.length === 0 && !loading && (
          <TableRow>
            <TableCell className="text-center" colSpan={7}>
              <div className="py-8">
                <CubeIcon className="mx-auto h-12 w-12 text-gray-400" />
                <p className="mt-2 text-sm text-gray-500">No products found</p>
              </div>
            </TableCell>
          </TableRow>
        )}

        {/* Product Rows */}
        {!loading &&
          products.map((product) => {
            const attributeBadge = getAttributeBadge(product);

            return (
              <TableRow key={product.id}>
                {/* Image */}
                <TableCell>
                  {product.photoUrl ? (
                    <img
                      src={product.photoUrl}
                      alt={product.name}
                      className="h-12 w-12 rounded-lg object-cover"
                    />
                  ) : (
                    <div className="h-12 w-12 rounded-lg bg-gray-200 flex items-center justify-center">
                      <CubeIcon className="h-6 w-6 text-gray-400" />
                    </div>
                  )}
                </TableCell>

                {/* Name */}
                <TableCell>
                  <div className="text-sm font-medium text-gray-900">
                    {product.name}
                  </div>
                  {product.description && (
                    <div className="text-sm text-gray-500 max-w-xs truncate">
                      {product.description}
                    </div>
                  )}
                </TableCell>

                {/* Type Badge */}
                <TableCell>
                  <span className="px-2 py-1 inline-flex text-xs leading-5 font-semibold rounded-full bg-blue-100 text-blue-800">
                    {getProductTypeLabel(product.productTypeId)}
                  </span>
                </TableCell>

                {/* Taste/Color Badge */}
                <TableCell>
                  {attributeBadge && (
                    <span
                      className={`px-2 py-1 inline-flex text-xs leading-5 font-semibold rounded-full ${attributeBadge.color}`}
                    >
                      {attributeBadge.label}
                    </span>
                  )}
                  {!attributeBadge && (
                    <span className="text-sm text-gray-400">—</span>
                  )}
                </TableCell>

                {/* Price */}
                <TableCell>
                  <div className="text-sm font-medium text-gray-900">
                    {formatCurrency(product.price, product.currency)}
                  </div>
                </TableCell>

                {/* Stock Badge */}
                <TableCell>
                  <StockBadge
                    quantity={product.stockQuantity}
                    isLowStock={product.isLowStock}
                    size="sm"
                  />
                </TableCell>

                {/* Action Buttons */}
                <TableCell>
                  <div className="flex items-center gap-3">
                    <button
                      onClick={() => onEdit(product)}
                      className="text-blue-600 hover:text-blue-900 transition-colors"
                      title="Edit Product"
                    >
                      <PencilIcon className="h-5 w-5" />
                    </button>
                    <button
                      onClick={() => onManageStock(product)}
                      className="text-purple-600 hover:text-purple-900 transition-colors"
                      title="Manage Stock"
                    >
                      <ChartBarIcon className="h-5 w-5" />
                    </button>
                    <button
                      onClick={() => onDelete(product)}
                      className="text-red-600 hover:text-red-900 transition-colors"
                      title="Delete Product"
                    >
                      <TrashIcon className="h-5 w-5" />
                    </button>
                  </div>
                </TableCell>
              </TableRow>
            );
          })}
      </TableBody>
    </Table>
  );
};

export default ProductTable;
