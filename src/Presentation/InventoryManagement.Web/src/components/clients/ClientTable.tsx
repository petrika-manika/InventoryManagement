/**
 * Client Table Component
 * Displays clients in table view with sorting and actions
 */

import { PencilIcon, TrashIcon } from "@heroicons/react/24/outline";
import type {
  ClientDto,
  IndividualClientDto,
  BusinessClientDto,
} from "../../types/client.types";
import { formatPhoneNumber, formatNIPT } from "../../utils/formatters";
import {
  Table,
  TableHeader,
  TableBody,
  TableRow,
  TableHead,
  TableCell,
} from "../common/Table";
import LoadingSpinner from "../common/LoadingSpinner";
import Badge from "../common/Badge";

interface ClientTableProps {
  clients: ClientDto[];
  onEdit: (client: ClientDto) => void;
  onDelete: (client: ClientDto) => void;
  loading?: boolean;
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

export default function ClientTable({
  clients,
  onEdit,
  onDelete,
  loading = false,
}: ClientTableProps) {
  // Truncate text helper
  const truncate = (text: string | undefined, maxLength: number) => {
    if (!text) return "-";
    return text.length > maxLength
      ? `${text.substring(0, maxLength)}...`
      : text;
  };

  return (
    <div className="bg-white rounded-lg shadow overflow-hidden">
      <div className="overflow-x-auto">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Type</TableHead>
              <TableHead>Name</TableHead>
              <TableHead>Contact Info</TableHead>
              <TableHead>NIPT</TableHead>
              <TableHead>Address</TableHead>
              <TableHead>Status</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={7}>
                  <div className="flex justify-center py-8">
                    <LoadingSpinner />
                  </div>
                </TableCell>
              </TableRow>
            ) : clients.length === 0 ? (
              <TableRow>
                <TableCell colSpan={7}>
                  <div className="text-center py-8 text-gray-500">
                    No clients found
                  </div>
                </TableCell>
              </TableRow>
            ) : (
              clients.map((client) => {
                const isIndividual = isIndividualClient(client);
                const isBusiness = isBusinessClient(client);

                const displayName = isIndividual
                  ? client.fullName
                  : isBusiness
                  ? client.contactPersonFullName
                  : "Unknown";

                return (
                  <TableRow key={client.id}>
                    {/* Type Badge */}
                    <TableCell>
                      <Badge
                        variant={isIndividual ? "success" : "info"}
                        size="sm"
                      >
                        {isIndividual ? "Individual" : "Business"}
                      </Badge>
                    </TableCell>

                    {/* Name */}
                    <TableCell>
                      <div className="font-medium text-gray-900">
                        {displayName}
                      </div>
                    </TableCell>

                    {/* Contact Info */}
                    <TableCell>
                      <div className="text-sm space-y-1">
                        {client.email && (
                          <div className="text-gray-600 truncate max-w-xs">
                            {client.email}
                          </div>
                        )}
                        {client.phoneNumber && (
                          <div className="text-gray-500">
                            {formatPhoneNumber(client.phoneNumber)}
                          </div>
                        )}
                        {!client.email && !client.phoneNumber && (
                          <span className="text-gray-400">-</span>
                        )}
                      </div>
                    </TableCell>

                    {/* NIPT (Business only) */}
                    <TableCell>
                      {isBusiness ? (
                        <span className="text-sm font-mono text-gray-700">
                          {formatNIPT(client.nipt)}
                        </span>
                      ) : (
                        <span className="text-gray-400">-</span>
                      )}
                    </TableCell>

                    {/* Address */}
                    <TableCell>
                      <span className="text-sm text-gray-600">
                        {truncate(client.address, 50)}
                      </span>
                    </TableCell>

                    {/* Status */}
                    <TableCell>
                      <Badge
                        variant={client.isActive ? "success" : "danger"}
                        size="sm"
                      >
                        {client.isActive ? "Active" : "Inactive"}
                      </Badge>
                    </TableCell>

                    {/* Actions */}
                    <TableCell>
                      <div className="flex justify-end gap-2">
                        <button
                          onClick={() => onEdit(client)}
                          className="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                          title="Edit client"
                        >
                          <PencilIcon className="h-5 w-5" />
                        </button>
                        <button
                          onClick={() => onDelete(client)}
                          className="p-2 text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                          title="Delete client"
                        >
                          <TrashIcon className="h-5 w-5" />
                        </button>
                      </div>
                    </TableCell>
                  </TableRow>
                );
              })
            )}
          </TableBody>
        </Table>
      </div>
    </div>
  );
}
