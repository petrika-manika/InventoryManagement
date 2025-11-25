import apiClient from "./apiClient";
import { API_ENDPOINTS } from "../utils/apiConfig";
import type {
  LoginRequest,
  AuthenticationResult,
  UserDto,
} from "../types/user.types";
import { setToken, removeToken } from "../utils/tokenStorage";

/**
 * Authentication Service
 * Handles user authentication operations including login, logout, and fetching current user
 */
const authService = {
  /**
   * Authenticates a user with email and password
   * @param credentials - User login credentials (email and password)
   * @returns Promise resolving to authentication result with user data and token
   */
  async login(credentials: LoginRequest): Promise<AuthenticationResult> {
    const response = await apiClient.post<AuthenticationResult>(
      API_ENDPOINTS.AUTH.LOGIN,
      credentials
    );

    const data = response.data;

    // Store the authentication token
    setToken(data.token);

    return data;
  },

  /**
   * Logs out the current user
   * Clears authentication token and redirects to login page
   */
  logout(): void {
    removeToken();
    window.location.href = "/login";
  },

  /**
   * Fetches the currently authenticated user's information
   * @returns Promise resolving to the current user's data
   */
  async getCurrentUser(): Promise<UserDto> {
    const response = await apiClient.get<UserDto>(
      API_ENDPOINTS.USERS.GET_CURRENT
    );
    return response.data;
  },
};

export default authService;
