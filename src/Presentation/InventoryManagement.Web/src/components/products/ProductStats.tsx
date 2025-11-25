/**
 * ProductStats Component
 * Displays product statistics including counts by type, total value, and low stock alerts
 */

import React from "react";
import type { ProductDto } from "../../types/product.types";
import { formatCurrency } from "../../utils/formatters";
import {
  CubeIcon,
  BeakerIcon,
  CpuChipIcon,
  SparklesIcon,
  BoltIcon,
  CurrencyDollarIcon,
  ExclamationTriangleIcon,
} from "@heroicons/react/24/outline";

interface ProductStatsProps {
  products: ProductDto[];
}

const ProductStats: React.FC<ProductStatsProps> = ({ products }) => {
  // Calculate statistics
  const totalCount = products.length;

  const aromaBombelCount = products.filter((p) => p.productTypeId === 1).length;

  const aromaBottleCount = products.filter((p) => p.productTypeId === 2).length;

  const aromaDeviceCount = products.filter((p) => p.productTypeId === 3).length;

  const sanitizingDeviceCount = products.filter(
    (p) => p.productTypeId === 4
  ).length;

  const batteryCount = products.filter((p) => p.productTypeId === 5).length;

  const lowStockCount = products.filter((p) => p.isLowStock).length;

  const totalValue = products.reduce(
    (sum, product) => sum + product.price * product.stockQuantity,
    0
  );

  const stats = [
    {
      label: "Total Products",
      value: totalCount.toString(),
      icon: CubeIcon,
      bgColor: "bg-blue-500",
    },
    {
      label: "Aroma Bombels",
      value: aromaBombelCount.toString(),
      icon: BeakerIcon,
      bgColor: "bg-green-500",
    },
    {
      label: "Aroma Bottles",
      value: aromaBottleCount.toString(),
      icon: BeakerIcon,
      bgColor: "bg-cyan-500",
    },
    {
      label: "Aroma Devices",
      value: aromaDeviceCount.toString(),
      icon: CpuChipIcon,
      bgColor: "bg-purple-500",
    },
    {
      label: "Sanitizing Devices",
      value: sanitizingDeviceCount.toString(),
      icon: SparklesIcon,
      bgColor: "bg-orange-500",
    },
    {
      label: "Batteries",
      value: batteryCount.toString(),
      icon: BoltIcon,
      bgColor: "bg-yellow-500",
    },
    {
      label: "Total Inventory Value",
      value: formatCurrency(totalValue, "ALL"),
      icon: CurrencyDollarIcon,
      bgColor: "bg-indigo-500",
      subtext: "Based on current stock",
    },
    {
      label: "Low Stock Items",
      value: lowStockCount.toString(),
      icon: ExclamationTriangleIcon,
      bgColor: "bg-red-500",
      subtext: lowStockCount > 0 ? "Needs attention" : "All good",
    },
  ];

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
      {stats.map((stat, index) => {
        const Icon = stat.icon;
        return (
          <div
            key={index}
            className="bg-white rounded-lg shadow p-6 flex items-center gap-4"
          >
            <div className={`p-3 rounded-full ${stat.bgColor}`}>
              <Icon className="h-6 w-6 text-white" />
            </div>
            <div className="flex-1">
              <p className="text-sm font-medium text-gray-600">{stat.label}</p>
              <p className="text-2xl font-bold text-gray-900">{stat.value}</p>
              {stat.subtext && (
                <p className="text-xs text-gray-500 mt-1">{stat.subtext}</p>
              )}
            </div>
          </div>
        );
      })}
    </div>
  );
};

export default ProductStats;
