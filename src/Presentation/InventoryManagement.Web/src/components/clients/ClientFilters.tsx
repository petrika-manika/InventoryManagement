/**
 * Client Filters Component
 * Provides search, filtering, and view mode controls for the clients list
 */

import {
  MagnifyingGlassIcon,
  Squares2X2Icon,
  TableCellsIcon,
} from "@heroicons/react/24/outline";
import Select from "../common/Select";

interface ClientFiltersProps {
  searchTerm: string;
  onSearchChange: (value: string) => void;
  filterType: number; // 0 = All, 1 = Individual, 2 = Business
  onTypeChange: (typeId: number) => void;
  viewMode: "grid" | "table";
  onViewModeChange: (mode: "grid" | "table") => void;
}

export default function ClientFilters({
  searchTerm,
  onSearchChange,
  filterType,
  onTypeChange,
  viewMode,
  onViewModeChange,
}: ClientFiltersProps) {
  const typeOptions = [
    { value: 0, label: "All Clients" },
    { value: 1, label: "Individual" },
    { value: 2, label: "Business" },
  ];

  return (
    <div className="flex flex-col sm:flex-row gap-4 items-start sm:items-end mb-6">
      {/* Search input - flex-1 */}
      <div className="flex-1 relative">
        <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
          <MagnifyingGlassIcon className="h-5 w-5 text-gray-400" />
        </div>
        <input
          type="text"
          placeholder="Search by name, email, phone, or NIPT..."
          value={searchTerm}
          onChange={(e) => onSearchChange(e.target.value)}
          className="w-full pl-10 pr-3 py-2 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:border-primary-500 focus:ring-primary-500"
        />
      </div>

      {/* Type filter */}
      <div className="w-full sm:w-64">
        <Select
          value={filterType}
          onChange={(value) => onTypeChange(value as number)}
          options={typeOptions}
          placeholder="Select client type"
        />
      </div>

      {/* View mode toggle */}
      <div className="flex gap-2">
        <button
          onClick={() => onViewModeChange("grid")}
          className={`p-2 rounded-lg transition-colors ${
            viewMode === "grid"
              ? "bg-primary-100 text-primary-700"
              : "bg-gray-100 text-gray-600 hover:bg-gray-200"
          }`}
          title="Grid view"
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
          title="Table view"
        >
          <TableCellsIcon className="h-6 w-6" />
        </button>
      </div>
    </div>
  );
}
