/**
 * ProductTypeModal Component
 * Modal for selecting product type before creating a new product
 */

import React from "react";
import Modal from "../common/Modal";
import {
  BeakerIcon,
  CpuChipIcon,
  SparklesIcon,
  BoltIcon,
} from "@heroicons/react/24/outline";

interface ProductTypeModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSelectType: (typeId: number) => void;
}

const ProductTypeModal: React.FC<ProductTypeModalProps> = ({
  isOpen,
  onClose,
  onSelectType,
}) => {
  const productTypes = [
    {
      id: 1,
      name: "Aroma Bombel",
      description: "Aroma bombs with various tastes for small spaces",
      icon: BeakerIcon,
      color: "bg-green-500",
    },
    {
      id: 2,
      name: "Aroma Bottle",
      description: "Aroma bottles with liquid refills",
      icon: BeakerIcon,
      color: "bg-cyan-500",
    },
    {
      id: 3,
      name: "Aroma Device",
      description: "Electronic aroma devices for medium to large spaces",
      icon: CpuChipIcon,
      color: "bg-purple-500",
    },
    {
      id: 4,
      name: "Sanitizing Device",
      description: "Hygienic sanitizing devices for cleaning",
      icon: SparklesIcon,
      color: "bg-orange-500",
    },
    {
      id: 5,
      name: "Battery",
      description: "Replacement batteries (LR6/AA and LR9/AAA)",
      icon: BoltIcon,
      color: "bg-yellow-500",
    },
  ];

  const handleSelectType = (typeId: number) => {
    onSelectType(typeId);
    onClose();
  };

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title="Select Product Type"
      maxWidth="xl"
    >
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-4">
        {productTypes.map((type) => {
          const Icon = type.icon;
          return (
            <button
              key={type.id}
              onClick={() => handleSelectType(type.id)}
              className="border-2 border-gray-200 rounded-lg p-6 hover:border-primary-500 hover:bg-primary-50 cursor-pointer transition-all text-left"
            >
              <div
                className={`h-12 w-12 rounded-full flex items-center justify-center mb-4 ${type.color}`}
              >
                <Icon className="h-6 w-6 text-white" />
              </div>
              <h3 className="text-lg font-bold text-gray-900 mb-2">
                {type.name}
              </h3>
              <p className="text-sm text-gray-600">{type.description}</p>
            </button>
          );
        })}
      </div>
    </Modal>
  );
};

export default ProductTypeModal;
