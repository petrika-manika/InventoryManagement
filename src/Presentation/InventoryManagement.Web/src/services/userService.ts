import apiClient from "./apiClient";
import { API_ENDPOINTS } from "../utils/apiConfig";
import type {
  UserDto,
  CreateUserRequest,
  UpdateUserRequest,
} from "../types/user.types";

/**
 * User Service
 * Handles all user-related API operations
 */
const userService = {
  /**
   * Retrieves all users from the system
   * @returns Promise resolving to array of users
   */
  async getAllUsers(): Promise<UserDto[]> {
    const response = await apiClient.get<UserDto[]>(
      API_ENDPOINTS.USERS.GET_ALL
    );
    return response.data;
  },

  /**
   * Retrieves a specific user by ID
   * @param id - User ID
   * @returns Promise resolving to user data
   */
  async getUserById(id: string): Promise<UserDto> {
    const response = await apiClient.get<UserDto>(
      API_ENDPOINTS.USERS.GET_BY_ID(id)
    );
    return response.data;
  },

  /**
   * Retrieves the currently authenticated user
   * @returns Promise resolving to current user data
   */
  async getCurrentUser(): Promise<UserDto> {
    const response = await apiClient.get<UserDto>(
      API_ENDPOINTS.USERS.GET_CURRENT
    );
    return response.data;
  },

  /**
   * Creates a new user in the system
   * @param data - User creation data
   * @returns Promise resolving to the created user's ID
   */
  async createUser(data: CreateUserRequest): Promise<string> {
    const response = await apiClient.post<string>(
      API_ENDPOINTS.USERS.CREATE,
      data
    );
    return response.data;
  },

  /**
   * Updates an existing user's information
   * @param id - User ID to update
   * @param data - Updated user data
   * @returns Promise that resolves when update is complete
   */
  async updateUser(id: string, data: UpdateUserRequest): Promise<void> {
    await apiClient.put(API_ENDPOINTS.USERS.UPDATE(id), data);
  },

  /**
   * Activates a user account
   * @param id - User ID to activate
   * @returns Promise that resolves when activation is complete
   */
  async activateUser(id: string): Promise<void> {
    await apiClient.patch(API_ENDPOINTS.USERS.ACTIVATE(id));
  },

  /**
   * Deactivates a user account
   * @param id - User ID to deactivate
   * @returns Promise that resolves when deactivation is complete
   */
  async deactivateUser(id: string): Promise<void> {
    await apiClient.patch(API_ENDPOINTS.USERS.DEACTIVATE(id));
  },
};

export default userService;
