/**
 * Clients Page
 * Main page for managing individual and business clients
 */

import { useState, useEffect } from "react";
import { toast } from "react-hot-toast";
import { PlusIcon, UsersIcon } from "@heroicons/react/24/outline";
import type {
  ClientDto,
  IndividualClientDto,
  BusinessClientDto,
} from "../types/client.types";
import clientService from "../services/clientService";
import Button from "../components/common/Button";
import ConfirmationDialog from "../components/common/ConfirmationDialog";
import ClientStats from "../components/clients/ClientStats";
import ClientFilters from "../components/clients/ClientFilters";
import ClientCard from "../components/clients/ClientCard";
import ClientTable from "../components/clients/ClientTable";
import ClientTypeModal from "../components/clients/ClientTypeModal";
import IndividualClientForm from "../components/clients/IndividualClientForm";
import BusinessClientForm from "../components/clients/BusinessClientForm";

export default function ClientsPage() {
  const [clients, setClients] = useState<ClientDto[]>([]);
  const [filteredClients, setFilteredClients] = useState<ClientDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState("");
  const [filterType, setFilterType] = useState(0); // 0 = All
  const [viewMode, setViewMode] = useState<"grid" | "table">("grid");

  // Modal states
  const [showTypeModal, setShowTypeModal] = useState(false);
  const [showFormModal, setShowFormModal] = useState(false);
  const [showDeleteDialog, setShowDeleteDialog] = useState(false);

  // Selected items
  const [selectedClient, setSelectedClient] = useState<ClientDto | null>(null);
  const [selectedClientType, setSelectedClientType] = useState<number | null>(
    null
  );

  // Load clients on mount
  useEffect(() => {
    loadClients();
  }, []);

  // Filter clients when search/filter changes
  useEffect(() => {
    let filtered = clients;

    // Filter by type
    if (filterType > 0) {
      filtered = filtered.filter((c) => c.clientTypeId === filterType);
    }

    // Filter by search term
    if (searchTerm) {
      const term = searchTerm.toLowerCase();
      filtered = filtered.filter((c) => {
        // Search in common fields
        if (c.email?.toLowerCase().includes(term)) return true;
        if (c.phoneNumber?.toLowerCase().includes(term)) return true;

        // Type-specific search
        if (c.clientTypeId === 1) {
          const individual = c as IndividualClientDto;
          const fullName =
            individual.fullName ||
            `${individual.firstName} ${individual.lastName}`;
          return fullName.toLowerCase().includes(term);
        } else {
          const business = c as BusinessClientDto;
          return (
            business.nipt.toLowerCase().includes(term) ||
            business.contactPersonFullName.toLowerCase().includes(term) ||
            (business.ownerFullName &&
              business.ownerFullName.toLowerCase().includes(term))
          );
        }
      });
    }

    setFilteredClients(filtered);
  }, [clients, filterType, searchTerm]);

  const loadClients = async () => {
    setLoading(true);
    try {
      const data = await clientService.getAllClients();
      setClients(data);
    } catch {
      toast.error("Failed to load clients");
    } finally {
      setLoading(false);
    }
  };

  const handleAddClick = () => {
    setSelectedClient(null);
    setSelectedClientType(null);
    setShowTypeModal(true);
  };

  const handleSelectType = (typeId: number) => {
    setSelectedClientType(typeId);
    setShowTypeModal(false);
    setShowFormModal(true);
  };

  const handleEdit = (client: ClientDto) => {
    setSelectedClient(client);
    setSelectedClientType(client.clientTypeId);
    setShowFormModal(true);
  };

  const handleDelete = (client: ClientDto) => {
    setSelectedClient(client);
    setShowDeleteDialog(true);
  };

  const confirmDelete = async () => {
    if (!selectedClient) return;

    try {
      await clientService.deleteClient(selectedClient.id);
      toast.success("Client deleted successfully");
      loadClients();
      setShowDeleteDialog(false);
    } catch (error: unknown) {
      const errorMessage =
        error && typeof error === "object" && "message" in error
          ? String(error.message)
          : "Failed to delete client";
      toast.error(errorMessage);
    }
  };

  const handleFormSubmit = async (data: unknown) => {
    if (!selectedClientType) return;

    if (selectedClient) {
      // Update
      await clientService.updateClient(
        selectedClientType,
        selectedClient.id,
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        data as any
      );
    } else {
      // Create
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      await clientService.createClient(selectedClientType, data as any);
    }
    loadClients();
    setShowFormModal(false);
  };

  return (
    <div className="p-6">
      {/* Header */}
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Clients</h1>
          <p className="text-gray-600 mt-1">
            Manage your individual and business clients
          </p>
        </div>
        <Button onClick={handleAddClick} className="flex items-center">
          <PlusIcon className="h-5 w-5 mr-2" />
          Add Client
        </Button>
      </div>

      {/* Stats */}
      <ClientStats clients={clients} />

      {/* Filters */}
      <ClientFilters
        searchTerm={searchTerm}
        onSearchChange={setSearchTerm}
        filterType={filterType}
        onTypeChange={setFilterType}
        viewMode={viewMode}
        onViewModeChange={setViewMode}
      />

      {/* Loading */}
      {loading && (
        <div className="flex justify-center py-12">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
        </div>
      )}

      {/* Empty State */}
      {!loading && filteredClients.length === 0 && (
        <div className="text-center py-12">
          <UsersIcon className="mx-auto h-12 w-12 text-gray-400" />
          <h3 className="mt-2 text-sm font-medium text-gray-900">
            No clients found
          </h3>
          <p className="mt-1 text-sm text-gray-500">
            Get started by creating a new client.
          </p>
          <div className="mt-6">
            <Button
              onClick={handleAddClick}
              className="inline-flex items-center"
            >
              <PlusIcon className="h-5 w-5 mr-2" />
              Add Client
            </Button>
          </div>
        </div>
      )}

      {/* Grid View */}
      {!loading && viewMode === "grid" && filteredClients.length > 0 && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mt-6">
          {filteredClients.map((client) => (
            <ClientCard
              key={client.id}
              client={client}
              onEdit={handleEdit}
              onDelete={handleDelete}
            />
          ))}
        </div>
      )}

      {/* Table View */}
      {!loading && viewMode === "table" && filteredClients.length > 0 && (
        <div className="mt-6">
          <ClientTable
            clients={filteredClients}
            onEdit={handleEdit}
            onDelete={handleDelete}
          />
        </div>
      )}

      {/* Modals */}
      <ClientTypeModal
        isOpen={showTypeModal}
        onClose={() => setShowTypeModal(false)}
        onSelectType={handleSelectType}
      />

      {/* Type-specific form modals */}
      {selectedClientType === 1 && (
        <IndividualClientForm
          isOpen={showFormModal}
          onClose={() => setShowFormModal(false)}
          client={selectedClient as IndividualClientDto}
          onSubmit={handleFormSubmit}
        />
      )}
      {selectedClientType === 2 && (
        <BusinessClientForm
          isOpen={showFormModal}
          onClose={() => setShowFormModal(false)}
          client={selectedClient as BusinessClientDto}
          onSubmit={handleFormSubmit}
        />
      )}

      {/* Delete Confirmation */}
      <ConfirmationDialog
        isOpen={showDeleteDialog}
        onClose={() => setShowDeleteDialog(false)}
        onConfirm={confirmDelete}
        title="Delete Client"
        message="Are you sure you want to delete this client? This action cannot be undone."
        confirmLabel="Delete"
        confirmVariant="danger"
      />
    </div>
  );
}
