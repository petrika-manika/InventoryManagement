/**
 * Client Type Definitions
 * Type definitions for the Clients module including DTOs and request types
 */

// ==================== ENUMS ====================

/**
 * Client Type enum
 * Represents the two types of clients supported by the system
 */
export const ClientType = {
  Individual: 1,
  Business: 2,
} as const;

export type ClientTypeValue = (typeof ClientType)[keyof typeof ClientType];

/**
 * Client Type Labels
 * Human-readable labels for each client type
 */
export const ClientTypeLabels: Record<number, string> = {
  1: "Individual",
  2: "Business",
};

// ==================== BASE DTO ====================

/**
 * Base Client DTO
 * Contains common properties shared by all client types
 */
export interface ClientDto {
  id: string;
  clientType: string;
  clientTypeId: number;
  address?: string;
  email?: string;
  phoneNumber?: string;
  notes?: string;
  createdAt: string;
  updatedAt: string;
  createdBy: string;
  updatedBy?: string;
  isActive: boolean;
}

// ==================== TYPE-SPECIFIC DTOS ====================

/**
 * Individual Client DTO
 * Represents a client who is an individual person
 */
export interface IndividualClientDto extends ClientDto {
  firstName: string;
  lastName: string;
  fullName: string;
}

/**
 * Business Client DTO
 * Represents a client who is a business entity
 */
export interface BusinessClientDto extends ClientDto {
  nipt: string;
  ownerFirstName?: string;
  ownerLastName?: string;
  ownerPhoneNumber?: string;
  ownerFullName?: string;
  contactPersonFirstName: string;
  contactPersonLastName: string;
  contactPersonPhoneNumber?: string;
  contactPersonFullName: string;
}

// ==================== CREATE REQUEST TYPES ====================

/**
 * Create Individual Client Request
 * Data required to create a new individual client
 */
export interface CreateIndividualClientRequest {
  firstName: string;
  lastName: string;
  address?: string;
  email?: string;
  phoneNumber?: string;
  notes?: string;
}

/**
 * Create Business Client Request
 * Data required to create a new business client
 */
export interface CreateBusinessClientRequest {
  nipt: string;
  ownerFirstName?: string;
  ownerLastName?: string;
  ownerPhoneNumber?: string;
  contactPersonFirstName: string;
  contactPersonLastName: string;
  contactPersonPhoneNumber?: string;
  address?: string;
  email?: string;
  phoneNumber?: string;
  notes?: string;
}

// ==================== UPDATE REQUEST TYPES ====================

/**
 * Update Individual Client Request
 * Data required to update an existing individual client
 */
export interface UpdateIndividualClientRequest {
  firstName: string;
  lastName: string;
  address?: string;
  email?: string;
  phoneNumber?: string;
  notes?: string;
}

/**
 * Update Business Client Request
 * Data required to update an existing business client
 */
export interface UpdateBusinessClientRequest {
  nipt: string;
  ownerFirstName?: string;
  ownerLastName?: string;
  ownerPhoneNumber?: string;
  contactPersonFirstName: string;
  contactPersonLastName: string;
  contactPersonPhoneNumber?: string;
  address?: string;
  email?: string;
  phoneNumber?: string;
  notes?: string;
}
