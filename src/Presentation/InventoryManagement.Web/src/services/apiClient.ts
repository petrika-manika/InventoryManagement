import axios from "axios";
import { getToken, removeToken } from "../utils/tokenStorage";
import { API_BASE_URL } from "../utils/apiConfig";
import type { ApiError } from "../types/api.types";

/**
 * Axios client instance configured for API communication
 * Includes automatic token handling and error formatting
 */
const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
  timeout: 10000, // 10 seconds
});

/**
 * Request Interceptor
 * Automatically adds authentication token to all requests
 */
apiClient.interceptors.request.use(
  (config) => {
    const token = getToken();

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

/**
 * Response Interceptor
 * Handles authentication errors and formats error responses
 */
apiClient.interceptors.response.use(
  (response) => {
    // Pass through successful responses (2xx status codes)
    return response;
  },
  (error) => {
    // Handle 401 Unauthorized - user session expired or invalid token
    // Skip redirect if we're already on the login page or making a login request
    const isLoginRequest = error.config?.url?.includes("/auth/login");
    const isOnLoginPage = window.location.pathname === "/login";

    if (error.response?.status === 401 && !isLoginRequest && !isOnLoginPage) {
      removeToken();
      window.location.href = "/login";
    }

    // Format error for consistent error handling across the app
    const statusCode = error.response?.status || 500;
    const message =
      error.response?.data?.message || "An unexpected error occurred";
    const errors = error.response?.data?.errors || undefined;

    const formattedError: ApiError = {
      statusCode,
      message,
      errors,
    };

    return Promise.reject(formattedError);
  }
);

export default apiClient;
