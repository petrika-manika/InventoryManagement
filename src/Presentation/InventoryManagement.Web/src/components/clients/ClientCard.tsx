/**
 * Client Card Component
 * Displays a single client in grid view with actions
 */

import {
  UserIcon,
  BuildingOfficeIcon,
  EnvelopeIcon,
  PhoneIcon,
  MapPinIcon,
  PencilIcon,
  TrashIcon,
} from "@heroicons/react/24/outline";
import type {
  ClientDto,
  IndividualClientDto,
  BusinessClientDto,
} from "../../types/client.types";
import { formatPhoneNumber, formatNIPT } from "../../utils/formatters";

interface ClientCardProps {
  client: ClientDto;
  onEdit: (client: ClientDto) => void;
  onDelete: (client: ClientDto) => void;
}

/**
 * Type guard to check if client is Individual
 */
function isIndividualClient(client: ClientDto): client is IndividualClientDto {
  return client.clientTypeId === 1;
}

/**
 * Type guard to check if client is Business
 */
function isBusinessClient(client: ClientDto): client is BusinessClientDto {
  return client.clientTypeId === 2;
}

export default function ClientCard({
  client,
  onEdit,
  onDelete,
}: ClientCardProps) {
  const isIndividual = isIndividualClient(client);
  const isBusiness = isBusinessClient(client);

  // Get display name with proper null checks
  let displayName = "Unknown";

  if (isIndividual) {
    const individualClient = client as IndividualClientDto;
    if (individualClient.fullName) {
      displayName = individualClient.fullName;
    } else if (individualClient.firstName && individualClient.lastName) {
      displayName = `${individualClient.firstName} ${individualClient.lastName}`;
    } else if (individualClient.firstName) {
      displayName = individualClient.firstName;
    } else if (individualClient.lastName) {
      displayName = individualClient.lastName;
    }
  } else if (isBusiness) {
    const businessClient = client as BusinessClientDto;
    displayName = businessClient.contactPersonFullName || "Unknown Business";
  }

  // Truncate text helper
  const truncate = (text: string | undefined, maxLength: number) => {
    if (!text) return "-";
    return text.length > maxLength
      ? `${text.substring(0, maxLength)}...`
      : text;
  };

  return (
    <div className="bg-white rounded-lg shadow hover:shadow-xl transition-shadow border border-gray-200 p-5">
      {/* Header Section */}
      <div className="flex justify-between items-start mb-3">
        {/* Client Type Badge */}
        <span
          className={`inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium ${
            isIndividual
              ? "bg-green-100 text-green-800"
              : "bg-blue-100 text-blue-800"
          }`}
        >
          {isIndividual ? (
            <>
              <UserIcon className="h-4 w-4" />
              Individual
            </>
          ) : (
            <>
              <BuildingOfficeIcon className="h-4 w-4" />
              Business
            </>
          )}
        </span>

        {/* Status indicator */}
        {!client.isActive && (
          <span className="text-xs text-gray-500 bg-gray-100 px-2 py-1 rounded">
            Inactive
          </span>
        )}
      </div>

      {/* Client Name */}
      <h3 className="text-lg font-bold text-gray-900 mt-2 mb-4">
        {displayName}
      </h3>

      {/* Info Section */}
      <div className="space-y-2 mb-4">
        {/* Individual specific info */}
        {isIndividual && (
          <>
            <div className="text-sm text-gray-600">
              <span className="font-medium">Name:</span> {client.firstName}{" "}
              {client.lastName}
            </div>
          </>
        )}

        {/* Business specific info */}
        {isBusiness && (
          <>
            <div className="text-sm text-gray-600">
              <span className="font-medium">NIPT:</span>{" "}
              {formatNIPT(client.nipt)}
            </div>
            <div className="text-sm text-gray-600">
              <span className="font-medium">Contact:</span>{" "}
              {client.contactPersonFullName}
            </div>
            {client.ownerFullName && (
              <div className="text-sm text-gray-600">
                <span className="font-medium">Owner:</span>{" "}
                {client.ownerFullName}
              </div>
            )}
          </>
        )}

        {/* Common info */}
        {client.email && (
          <div className="text-sm text-gray-600 flex items-center gap-2">
            <EnvelopeIcon className="h-4 w-4 text-gray-400" />
            <span className="truncate">{client.email}</span>
          </div>
        )}

        {client.phoneNumber && (
          <div className="text-sm text-gray-600 flex items-center gap-2">
            <PhoneIcon className="h-4 w-4 text-gray-400" />
            <span>{formatPhoneNumber(client.phoneNumber)}</span>
          </div>
        )}

        {client.address && (
          <div className="text-sm text-gray-600 flex items-center gap-2">
            <MapPinIcon className="h-4 w-4 text-gray-400" />
            <span className="truncate">{truncate(client.address, 100)}</span>
          </div>
        )}
      </div>

      {/* Notes Preview */}
      {client.notes && (
        <div className="mb-4 p-2 bg-gray-50 rounded text-xs text-gray-600 border border-gray-100">
          <span className="font-medium">Notes: </span>
          {truncate(client.notes, 50)}
        </div>
      )}

      {/* Action Buttons */}
      <div className="flex gap-2 mt-4 pt-4 border-t border-gray-100">
        <button
          onClick={() => onEdit(client)}
          className="flex-1 inline-flex items-center justify-center gap-2 px-3 py-2 text-sm font-medium text-blue-700 bg-blue-50 hover:bg-blue-100 rounded-lg transition-colors"
        >
          <PencilIcon className="h-4 w-4" />
          Edit
        </button>
        <button
          onClick={() => onDelete(client)}
          className="flex-1 inline-flex items-center justify-center gap-2 px-3 py-2 text-sm font-medium text-red-700 bg-red-50 hover:bg-red-100 rounded-lg transition-colors"
        >
          <TrashIcon className="h-4 w-4" />
          Delete
        </button>
      </div>
    </div>
  );
}
