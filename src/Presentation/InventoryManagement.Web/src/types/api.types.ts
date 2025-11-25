/**
 * Represents an API error response
 */
export interface ApiError {
  statusCode: number;
  message: string;
  errors?: Array<{ property: string; message: string }>;
}

/**
 * Generic wrapper for API responses with loading and error states
 * Used for consistent API error handling across the application
 */
export interface ApiResponse<T> {
  data?: T;
  error?: ApiError;
  isLoading: boolean;
}
