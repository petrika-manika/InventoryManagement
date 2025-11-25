/**
 * Product Service
 * Handles all product-related API operations including CRUD operations
 * for different product types and stock management
 */

import apiClient from "./apiClient";
import { API_ENDPOINTS } from "../utils/apiConfig";
import type {
  ProductDto,
  CreateAromaBombelRequest,
  CreateAromaBottleRequest,
  CreateAromaDeviceRequest,
  CreateSanitizingDeviceRequest,
  CreateBatteryRequest,
  UpdateAromaBombelRequest,
  UpdateAromaBottleRequest,
  UpdateAromaDeviceRequest,
  UpdateSanitizingDeviceRequest,
  UpdateBatteryRequest,
} from "../types/product.types";

// Union types for helper methods
type CreateProductRequest =
  | CreateAromaBombelRequest
  | CreateAromaBottleRequest
  | CreateAromaDeviceRequest
  | CreateSanitizingDeviceRequest
  | CreateBatteryRequest;

type UpdateProductRequest =
  | UpdateAromaBombelRequest
  | UpdateAromaBottleRequest
  | UpdateAromaDeviceRequest
  | UpdateSanitizingDeviceRequest
  | UpdateBatteryRequest;

const productService = {
  /**
   * Get all products regardless of type
   * @returns Promise resolving to array of all products
   */
  async getAllProducts(): Promise<ProductDto[]> {
    try {
      const response = await apiClient.get<ProductDto[]>(
        API_ENDPOINTS.PRODUCTS.GET_ALL
      );
      return response.data;
    } catch (error) {
      console.error("Error fetching all products:", error);
      throw error;
    }
  },

  /**
   * Get products filtered by product type
   * @param typeId - The product type ID to filter by
   * @returns Promise resolving to array of products of the specified type
   */
  async getProductsByType(typeId: number): Promise<ProductDto[]> {
    try {
      const response = await apiClient.get<ProductDto[]>(
        API_ENDPOINTS.PRODUCTS.GET_BY_TYPE(typeId)
      );
      return response.data;
    } catch (error) {
      console.error(`Error fetching products by type ${typeId}:`, error);
      throw error;
    }
  },

  /**
   * Get a single product by ID with all details
   * @param id - The product ID
   * @returns Promise resolving to the product details
   */
  async getProductById(id: string): Promise<ProductDto> {
    try {
      const response = await apiClient.get<ProductDto>(
        API_ENDPOINTS.PRODUCTS.GET_BY_ID(id)
      );
      return response.data;
    } catch (error) {
      console.error(`Error fetching product ${id}:`, error);
      throw error;
    }
  },

  /**
   * Get products with low stock levels
   * @param threshold - Stock quantity threshold (default: 10)
   * @returns Promise resolving to array of low stock products
   */
  async getLowStockProducts(threshold: number = 10): Promise<ProductDto[]> {
    try {
      const response = await apiClient.get<ProductDto[]>(
        API_ENDPOINTS.PRODUCTS.GET_LOW_STOCK,
        {
          params: { threshold },
        }
      );
      return response.data;
    } catch (error) {
      console.error("Error fetching low stock products:", error);
      throw error;
    }
  },

  // ==================== CREATE METHODS ====================

  /**
   * Create a new Aroma Bombel product
   * @param data - Aroma Bombel product data
   * @returns Promise resolving to the new product ID
   */
  async createAromaBombel(data: CreateAromaBombelRequest): Promise<string> {
    try {
      const response = await apiClient.post<string>(
        API_ENDPOINTS.PRODUCTS.CREATE_AROMA_BOMBEL,
        data
      );
      return response.data;
    } catch (error) {
      console.error("Error creating Aroma Bombel:", error);
      throw error;
    }
  },

  /**
   * Create a new Aroma Bottle product
   * @param data - Aroma Bottle product data
   * @returns Promise resolving to the new product ID
   */
  async createAromaBottle(data: CreateAromaBottleRequest): Promise<string> {
    try {
      const response = await apiClient.post<string>(
        API_ENDPOINTS.PRODUCTS.CREATE_AROMA_BOTTLE,
        data
      );
      return response.data;
    } catch (error) {
      console.error("Error creating Aroma Bottle:", error);
      throw error;
    }
  },

  /**
   * Create a new Aroma Device product
   * @param data - Aroma Device product data
   * @returns Promise resolving to the new product ID
   */
  async createAromaDevice(data: CreateAromaDeviceRequest): Promise<string> {
    try {
      const response = await apiClient.post<string>(
        API_ENDPOINTS.PRODUCTS.CREATE_AROMA_DEVICE,
        data
      );
      return response.data;
    } catch (error) {
      console.error("Error creating Aroma Device:", error);
      throw error;
    }
  },

  /**
   * Create a new Sanitizing Device product
   * @param data - Sanitizing Device product data
   * @returns Promise resolving to the new product ID
   */
  async createSanitizingDevice(
    data: CreateSanitizingDeviceRequest
  ): Promise<string> {
    try {
      const response = await apiClient.post<string>(
        API_ENDPOINTS.PRODUCTS.CREATE_SANITIZING_DEVICE,
        data
      );
      return response.data;
    } catch (error) {
      console.error("Error creating Sanitizing Device:", error);
      throw error;
    }
  },

  /**
   * Create a new Battery product
   * @param data - Battery product data
   * @returns Promise resolving to the new product ID
   */
  async createBattery(data: CreateBatteryRequest): Promise<string> {
    try {
      const response = await apiClient.post<string>(
        API_ENDPOINTS.PRODUCTS.CREATE_BATTERY,
        data
      );
      return response.data;
    } catch (error) {
      console.error("Error creating Battery:", error);
      throw error;
    }
  },

  // ==================== UPDATE METHODS ====================

  /**
   * Update an existing Aroma Bombel product
   * @param id - Product ID
   * @param data - Updated Aroma Bombel data
   * @returns Promise resolving when update is complete
   */
  async updateAromaBombel(
    id: string,
    data: UpdateAromaBombelRequest
  ): Promise<void> {
    try {
      await apiClient.put(API_ENDPOINTS.PRODUCTS.UPDATE_AROMA_BOMBEL(id), data);
    } catch (error) {
      console.error(`Error updating Aroma Bombel ${id}:`, error);
      throw error;
    }
  },

  /**
   * Update an existing Aroma Bottle product
   * @param id - Product ID
   * @param data - Updated Aroma Bottle data
   * @returns Promise resolving when update is complete
   */
  async updateAromaBottle(
    id: string,
    data: UpdateAromaBottleRequest
  ): Promise<void> {
    try {
      await apiClient.put(API_ENDPOINTS.PRODUCTS.UPDATE_AROMA_BOTTLE(id), data);
    } catch (error) {
      console.error(`Error updating Aroma Bottle ${id}:`, error);
      throw error;
    }
  },

  /**
   * Update an existing Aroma Device product
   * @param id - Product ID
   * @param data - Updated Aroma Device data
   * @returns Promise resolving when update is complete
   */
  async updateAromaDevice(
    id: string,
    data: UpdateAromaDeviceRequest
  ): Promise<void> {
    try {
      await apiClient.put(API_ENDPOINTS.PRODUCTS.UPDATE_AROMA_DEVICE(id), data);
    } catch (error) {
      console.error(`Error updating Aroma Device ${id}:`, error);
      throw error;
    }
  },

  /**
   * Update an existing Sanitizing Device product
   * @param id - Product ID
   * @param data - Updated Sanitizing Device data
   * @returns Promise resolving when update is complete
   */
  async updateSanitizingDevice(
    id: string,
    data: UpdateSanitizingDeviceRequest
  ): Promise<void> {
    try {
      await apiClient.put(
        API_ENDPOINTS.PRODUCTS.UPDATE_SANITIZING_DEVICE(id),
        data
      );
    } catch (error) {
      console.error(`Error updating Sanitizing Device ${id}:`, error);
      throw error;
    }
  },

  /**
   * Update an existing Battery product
   * @param id - Product ID
   * @param data - Updated Battery data
   * @returns Promise resolving when update is complete
   */
  async updateBattery(id: string, data: UpdateBatteryRequest): Promise<void> {
    try {
      await apiClient.put(API_ENDPOINTS.PRODUCTS.UPDATE_BATTERY(id), data);
    } catch (error) {
      console.error(`Error updating Battery ${id}:`, error);
      throw error;
    }
  },

  // ==================== DELETE METHOD ====================

  /**
   * Delete a product (works for all product types)
   * @param id - Product ID to delete
   * @returns Promise resolving when deletion is complete
   */
  async deleteProduct(id: string): Promise<void> {
    try {
      await apiClient.delete(API_ENDPOINTS.PRODUCTS.DELETE(id));
    } catch (error) {
      console.error(`Error deleting product ${id}:`, error);
      throw error;
    }
  },

  // ==================== HELPER METHODS ====================

  /**
   * Create a product of any type based on typeId
   * Simplifies form submission by routing to the correct create method
   * @param typeId - Product type ID (1-5)
   * @param data - Product data (type-specific)
   * @returns Promise resolving to the new product ID
   * @throws Error if typeId is invalid
   */
  async createProduct(
    typeId: number,
    data: CreateProductRequest
  ): Promise<string> {
    switch (typeId) {
      case 1: // AromaBombel
        return await this.createAromaBombel(data as CreateAromaBombelRequest);
      case 2: // AromaBottle
        return await this.createAromaBottle(data as CreateAromaBottleRequest);
      case 3: // AromaDevice
        return await this.createAromaDevice(data as CreateAromaDeviceRequest);
      case 4: // SanitizingDevice
        return await this.createSanitizingDevice(
          data as CreateSanitizingDeviceRequest
        );
      case 5: // Battery
        return await this.createBattery(data as CreateBatteryRequest);
      default:
        throw new Error(`Invalid product type ID: ${typeId}`);
    }
  },

  /**
   * Update a product of any type based on typeId
   * Simplifies form submission by routing to the correct update method
   * @param typeId - Product type ID (1-5)
   * @param id - Product ID to update
   * @param data - Updated product data (type-specific)
   * @returns Promise resolving when update is complete
   * @throws Error if typeId is invalid
   */
  async updateProduct(
    typeId: number,
    id: string,
    data: UpdateProductRequest
  ): Promise<void> {
    switch (typeId) {
      case 1: // AromaBombel
        return await this.updateAromaBombel(
          id,
          data as UpdateAromaBombelRequest
        );
      case 2: // AromaBottle
        return await this.updateAromaBottle(
          id,
          data as UpdateAromaBottleRequest
        );
      case 3: // AromaDevice
        return await this.updateAromaDevice(
          id,
          data as UpdateAromaDeviceRequest
        );
      case 4: // SanitizingDevice
        return await this.updateSanitizingDevice(
          id,
          data as UpdateSanitizingDeviceRequest
        );
      case 5: // Battery
        return await this.updateBattery(id, data as UpdateBatteryRequest);
      default:
        throw new Error(`Invalid product type ID: ${typeId}`);
    }
  },
};

export default productService;
