/**
 * Stock Service
 * Handles all stock management API operations including adding/removing stock
 * and retrieving stock history
 */

import apiClient from "./apiClient";
import { API_ENDPOINTS } from "../utils/apiConfig";
import type {
  AddStockRequest,
  RemoveStockRequest,
  StockHistoryDto,
} from "../types/product.types";

/**
 * Response type for stock operations
 */
interface StockOperationResponse {
  productId: string;
  newStockQuantity: number;
}

const stockService = {
  /**
   * Add stock to a product
   * @param data - Stock addition data (productId, quantity, reason)
   * @returns Promise resolving to the product ID and new stock quantity
   */
  async addStock(data: AddStockRequest): Promise<StockOperationResponse> {
    try {
      const response = await apiClient.post<StockOperationResponse>(
        API_ENDPOINTS.STOCK.ADD,
        data
      );
      return response.data;
    } catch (error) {
      console.error("Error adding stock:", error);
      throw error;
    }
  },

  /**
   * Remove stock from a product
   * @param data - Stock removal data (productId, quantity, reason)
   * @returns Promise resolving to the product ID and new stock quantity
   */
  async removeStock(data: RemoveStockRequest): Promise<StockOperationResponse> {
    try {
      const response = await apiClient.post<StockOperationResponse>(
        API_ENDPOINTS.STOCK.REMOVE,
        data
      );
      return response.data;
    } catch (error) {
      console.error("Error removing stock:", error);
      throw error;
    }
  },

  /**
   * Get stock history with optional filters
   * @param params - Query parameters for filtering history
   * @param params.productId - Filter by specific product ID (optional)
   * @param params.fromDate - Filter from date in ISO format (optional)
   * @param params.toDate - Filter to date in ISO format (optional)
   * @param params.take - Limit number of results (optional, default 50)
   * @returns Promise resolving to array of stock history entries
   */
  async getStockHistory(params: {
    productId?: string;
    fromDate?: string;
    toDate?: string;
    take?: number;
  }): Promise<StockHistoryDto[]> {
    try {
      const response = await apiClient.get<StockHistoryDto[]>(
        API_ENDPOINTS.STOCK.HISTORY,
        {
          params: {
            productId: params.productId,
            fromDate: params.fromDate,
            toDate: params.toDate,
            take: params.take || 50,
          },
        }
      );
      return response.data;
    } catch (error) {
      console.error("Error fetching stock history:", error);
      throw error;
    }
  },

  /**
   * Get stock history for a specific product
   * @param productId - The product ID to get history for
   * @param take - Number of history entries to return (default: 50)
   * @returns Promise resolving to array of stock history entries
   */
  async getProductStockHistory(
    productId: string,
    take: number = 50
  ): Promise<StockHistoryDto[]> {
    try {
      const response = await apiClient.get<StockHistoryDto[]>(
        API_ENDPOINTS.STOCK.HISTORY_BY_PRODUCT(productId),
        {
          params: { take },
        }
      );
      return response.data;
    } catch (error) {
      console.error(
        `Error fetching stock history for product ${productId}:`,
        error
      );
      throw error;
    }
  },
};

export default stockService;
