/**
 * Client Type Selection Modal
 * Allows users to select between Individual and Business client types before creation
 */

import { UserIcon, BuildingOfficeIcon } from "@heroicons/react/24/outline";
import Modal from "../common/Modal";

interface ClientTypeModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSelectType: (typeId: number) => void;
}

const clientTypes = [
  {
    id: 1,
    name: "Individual",
    description: "Personal client with basic contact information",
    icon: UserIcon,
    color: "bg-green-500",
  },
  {
    id: 2,
    name: "Business",
    description: "Company client with NIPT, owner, and contact person",
    icon: BuildingOfficeIcon,
    color: "bg-blue-500",
  },
];

export default function ClientTypeModal({
  isOpen,
  onClose,
  onSelectType,
}: ClientTypeModalProps) {
  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title="Select Client Type"
      maxWidth="lg"
    >
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-4">
        {clientTypes.map((type) => (
          <button
            key={type.id}
            onClick={() => {
              onSelectType(type.id);
              onClose();
            }}
            className="border-2 border-gray-200 rounded-lg p-6 hover:border-primary-500 hover:bg-primary-50 cursor-pointer transition-all text-left"
          >
            <div
              className={`h-12 w-12 rounded-full flex items-center justify-center mb-4 ${type.color}`}
            >
              <type.icon className="h-6 w-6 text-white" />
            </div>
            <h4 className="text-lg font-bold text-gray-900 mb-2">
              {type.name}
            </h4>
            <p className="text-sm text-gray-600">{type.description}</p>
          </button>
        ))}
      </div>
    </Modal>
  );
}
