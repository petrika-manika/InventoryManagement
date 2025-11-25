/**
 * Client Service
 * Handles all client-related API operations including CRUD operations
 * for both Individual and Business clients
 */

import apiClient from "./apiClient";
import type {
  ClientDto,
  CreateIndividualClientRequest,
  CreateBusinessClientRequest,
  UpdateIndividualClientRequest,
  UpdateBusinessClientRequest,
} from "../types/client.types";

// Union types for helper methods
type CreateClientRequest =
  | CreateIndividualClientRequest
  | CreateBusinessClientRequest;

type UpdateClientRequest =
  | UpdateIndividualClientRequest
  | UpdateBusinessClientRequest;

const clientService = {
  /**
   * Get all clients regardless of type
   * @param includeInactive - Whether to include inactive clients (default: false)
   * @returns Promise resolving to array of all clients
   */
  async getAllClients(includeInactive = false): Promise<ClientDto[]> {
    try {
      const response = await apiClient.get<ClientDto[]>("/api/clients", {
        params: { includeInactive },
      });
      return response.data;
    } catch (error) {
      console.error("Error fetching all clients:", error);
      throw error;
    }
  },

  /**
   * Get clients filtered by client type
   * @param clientTypeId - The client type ID to filter by (1=Individual, 2=Business)
   * @param includeInactive - Whether to include inactive clients (default: false)
   * @returns Promise resolving to array of clients of the specified type
   */
  async getClientsByType(
    clientTypeId: number,
    includeInactive = false
  ): Promise<ClientDto[]> {
    try {
      const response = await apiClient.get<ClientDto[]>(
        `/api/clients/type/${clientTypeId}`,
        {
          params: { includeInactive },
        }
      );
      return response.data;
    } catch (error) {
      console.error(`Error fetching clients by type ${clientTypeId}:`, error);
      throw error;
    }
  },

  /**
   * Get a single client by ID with all details
   * @param id - The client ID
   * @returns Promise resolving to the client details
   */
  async getClientById(id: string): Promise<ClientDto> {
    try {
      const response = await apiClient.get<ClientDto>(`/api/clients/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching client ${id}:`, error);
      throw error;
    }
  },

  /**
   * Search clients with optional filters
   * @param searchTerm - Search term to filter by name, NIPT, email, etc.
   * @param clientTypeId - Optional client type ID to filter by
   * @param includeInactive - Whether to include inactive clients (default: false)
   * @returns Promise resolving to array of matching clients
   */
  async searchClients(
    searchTerm?: string,
    clientTypeId?: number,
    includeInactive = false
  ): Promise<ClientDto[]> {
    try {
      const response = await apiClient.get<ClientDto[]>("/api/clients/search", {
        params: { searchTerm, clientTypeId, includeInactive },
      });
      return response.data;
    } catch (error) {
      console.error("Error searching clients:", error);
      throw error;
    }
  },

  // ==================== CREATE METHODS ====================

  /**
   * Create a new Individual client
   * @param data - Individual client data
   * @returns Promise resolving to the new client ID
   */
  async createIndividualClient(
    data: CreateIndividualClientRequest
  ): Promise<string> {
    try {
      const response = await apiClient.post<string>(
        "/api/clients/individual",
        data
      );
      return response.data;
    } catch (error) {
      console.error("Error creating Individual client:", error);
      throw error;
    }
  },

  /**
   * Create a new Business client
   * @param data - Business client data
   * @returns Promise resolving to the new client ID
   */
  async createBusinessClient(
    data: CreateBusinessClientRequest
  ): Promise<string> {
    try {
      const response = await apiClient.post<string>(
        "/api/clients/business",
        data
      );
      return response.data;
    } catch (error) {
      console.error("Error creating Business client:", error);
      throw error;
    }
  },

  // ==================== UPDATE METHODS ====================

  /**
   * Update an existing Individual client
   * @param id - Client ID
   * @param data - Updated Individual client data
   * @returns Promise resolving when update is complete
   */
  async updateIndividualClient(
    id: string,
    data: UpdateIndividualClientRequest
  ): Promise<void> {
    try {
      await apiClient.put(`/api/clients/individual/${id}`, data);
    } catch (error) {
      console.error(`Error updating Individual client ${id}:`, error);
      throw error;
    }
  },

  /**
   * Update an existing Business client
   * @param id - Client ID
   * @param data - Updated Business client data
   * @returns Promise resolving when update is complete
   */
  async updateBusinessClient(
    id: string,
    data: UpdateBusinessClientRequest
  ): Promise<void> {
    try {
      await apiClient.put(`/api/clients/business/${id}`, data);
    } catch (error) {
      console.error(`Error updating Business client ${id}:`, error);
      throw error;
    }
  },

  // ==================== DELETE METHOD ====================

  /**
   * Delete a client (soft delete - marks as inactive)
   * Works for both Individual and Business clients
   * @param id - Client ID to delete
   * @returns Promise resolving when deletion is complete
   */
  async deleteClient(id: string): Promise<void> {
    try {
      await apiClient.delete(`/api/clients/${id}`);
    } catch (error) {
      console.error(`Error deleting client ${id}:`, error);
      throw error;
    }
  },

  // ==================== HELPER METHODS ====================

  /**
   * Create a client of any type based on clientTypeId
   * Simplifies form submission by routing to the correct create method
   * @param clientTypeId - Client type ID (1=Individual, 2=Business)
   * @param data - Client data (type-specific)
   * @returns Promise resolving to the new client ID
   * @throws Error if clientTypeId is invalid
   */
  async createClient(
    clientTypeId: number,
    data: CreateClientRequest
  ): Promise<string> {
    switch (clientTypeId) {
      case 1: // Individual
        return await this.createIndividualClient(
          data as CreateIndividualClientRequest
        );
      case 2: // Business
        return await this.createBusinessClient(
          data as CreateBusinessClientRequest
        );
      default:
        throw new Error(`Invalid client type ID: ${clientTypeId}`);
    }
  },

  /**
   * Update a client of any type based on clientTypeId
   * Simplifies form submission by routing to the correct update method
   * @param clientTypeId - Client type ID (1=Individual, 2=Business)
   * @param id - Client ID to update
   * @param data - Updated client data (type-specific)
   * @returns Promise resolving when update is complete
   * @throws Error if clientTypeId is invalid
   */
  async updateClient(
    clientTypeId: number,
    id: string,
    data: UpdateClientRequest
  ): Promise<void> {
    switch (clientTypeId) {
      case 1: // Individual
        return await this.updateIndividualClient(
          id,
          data as UpdateIndividualClientRequest
        );
      case 2: // Business
        return await this.updateBusinessClient(
          id,
          data as UpdateBusinessClientRequest
        );
      default:
        throw new Error(`Invalid client type ID: ${clientTypeId}`);
    }
  },
};

export default clientService;
