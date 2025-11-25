/**
 * API Configuration
 * Centralized configuration for all API endpoints
 */

/**
 * Base URL for API requests
 * Reads from environment variable or falls back to default localhost
 */
export const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL || "https://localhost:7173";

/**
 * API endpoint definitions organized by resource
 * Use these constants throughout the app for consistent API calls
 */
export const API_ENDPOINTS = {
  /**
   * Authentication endpoints
   */
  AUTH: {
    LOGIN: "/api/auth/login",
  },

  /**
   * User management endpoints
   */
  USERS: {
    BASE: "/api/users",
    GET_ALL: "/api/users",
    GET_BY_ID: (id: string) => `/api/users/${id}`,
    GET_CURRENT: "/api/users/me",
    CREATE: "/api/users",
    UPDATE: (id: string) => `/api/users/${id}`,
    ACTIVATE: (id: string) => `/api/users/${id}/activate`,
    DEACTIVATE: (id: string) => `/api/users/${id}/deactivate`,
  },

  /**
   * Product management endpoints
   */
  PRODUCTS: {
    BASE: "/api/products",
    GET_ALL: "/api/products",
    GET_BY_TYPE: (typeId: number) => `/api/products/type/${typeId}`,
    GET_BY_ID: (id: string) => `/api/products/${id}`,
    GET_LOW_STOCK: "/api/products/low-stock",

    // Create endpoints for each product type
    CREATE_AROMA_BOMBEL: "/api/products/aroma-bombel",
    CREATE_AROMA_BOTTLE: "/api/products/aroma-bottle",
    CREATE_AROMA_DEVICE: "/api/products/aroma-device",
    CREATE_SANITIZING_DEVICE: "/api/products/sanitizing-device",
    CREATE_BATTERY: "/api/products/battery",

    // Update endpoints for each product type
    UPDATE_AROMA_BOMBEL: (id: string) => `/api/products/aroma-bombel/${id}`,
    UPDATE_AROMA_BOTTLE: (id: string) => `/api/products/aroma-bottle/${id}`,
    UPDATE_AROMA_DEVICE: (id: string) => `/api/products/aroma-device/${id}`,
    UPDATE_SANITIZING_DEVICE: (id: string) =>
      `/api/products/sanitizing-device/${id}`,
    UPDATE_BATTERY: (id: string) => `/api/products/battery/${id}`,

    // Delete endpoint (common for all types)
    DELETE: (id: string) => `/api/products/${id}`,
  },

  /**
   * Stock management endpoints
   */
  STOCK: {
    ADD: "/api/stock/add",
    REMOVE: "/api/stock/remove",
    HISTORY: "/api/stock/history",
    HISTORY_BY_PRODUCT: (productId: string) =>
      `/api/stock/history/${productId}`,
  },
};
