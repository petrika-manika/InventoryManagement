/**
 * ProductFilters Component
 * Provides filtering options for products including search, type filter, and view mode toggle
 */

import React from "react";
import ProductTypeSelect from "./ProductTypeSelect";
import {
  MagnifyingGlassIcon,
  Squares2X2Icon,
  TableCellsIcon,
} from "@heroicons/react/24/outline";

interface ProductFiltersProps {
  searchTerm: string;
  onSearchChange: (value: string) => void;
  filterType: number;
  onTypeChange: (typeId: number) => void;
  viewMode: "grid" | "table";
  onViewModeChange: (mode: "grid" | "table") => void;
}

const ProductFilters: React.FC<ProductFiltersProps> = ({
  searchTerm,
  onSearchChange,
  filterType,
  onTypeChange,
  viewMode,
  onViewModeChange,
}) => {
  return (
    <div className="flex flex-col sm:flex-row gap-4 items-start sm:items-end">
      {/* Search Input */}
      <div className="flex-1 relative">
        <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
          <MagnifyingGlassIcon className="h-5 w-5 text-gray-400" />
        </div>
        <input
          type="text"
          placeholder="Search products by name or description..."
          value={searchTerm}
          onChange={(e) => onSearchChange(e.target.value)}
          className="w-full pl-10 pr-3 py-2 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:border-primary-500 focus:ring-primary-500"
        />
      </div>

      {/* Product Type Filter */}
      <div className="w-full sm:w-64">
        <ProductTypeSelect
          value={filterType}
          onChange={onTypeChange}
          showAll={true}
          label=""
        />
      </div>

      {/* View Mode Toggle */}
      <div className="flex gap-2">
        <button
          onClick={() => onViewModeChange("grid")}
          className={`p-2 rounded-lg transition-colors ${
            viewMode === "grid"
              ? "bg-primary-100 text-primary-700"
              : "bg-gray-100 text-gray-600 hover:bg-gray-200"
          }`}
          title="Grid View"
        >
          <Squares2X2Icon className="h-6 w-6" />
        </button>
        <button
          onClick={() => onViewModeChange("table")}
          className={`p-2 rounded-lg transition-colors ${
            viewMode === "table"
              ? "bg-primary-100 text-primary-700"
              : "bg-gray-100 text-gray-600 hover:bg-gray-200"
          }`}
          title="Table View"
        >
          <TableCellsIcon className="h-6 w-6" />
        </button>
      </div>
    </div>
  );
};

export default ProductFilters;
