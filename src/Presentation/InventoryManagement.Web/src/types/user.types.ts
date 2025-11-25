/**
 * Represents a user in the system
 */
export interface UserDto {
  id: string;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

/**
 * Result returned after successful authentication
 */
export interface AuthenticationResult {
  user: UserDto;
  token: string;
}

/**
 * Request payload for user login
 */
export interface LoginRequest {
  email: string;
  password: string;
}

/**
 * Request payload for creating a new user
 */
export interface CreateUserRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

/**
 * Request payload for updating an existing user
 */
export interface UpdateUserRequest {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
}
