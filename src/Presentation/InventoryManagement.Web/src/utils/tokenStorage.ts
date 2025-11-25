import { jwtDecode } from "jwt-decode";

/**
 * Token Storage Utilities
 * Manages JWT token persistence in localStorage
 * Uses jwt-decode library for robust JWT parsing and validation
 */

const TOKEN_KEY = "auth_token";

/**
 * JWT payload interface with expiration claim
 */
interface JwtPayload {
  exp?: number;
  [key: string]: unknown;
}

/**
 * Retrieves the authentication token from localStorage
 * @returns The stored token or null if not found
 */
export const getToken = (): string | null => {
  return localStorage.getItem(TOKEN_KEY);
};

/**
 * Saves the authentication token to localStorage
 * @param token - The JWT token to store
 */
export const setToken = (token: string): void => {
  localStorage.setItem(TOKEN_KEY, token);
};

/**
 * Removes the authentication token from localStorage
 */
export const removeToken = (): void => {
  localStorage.removeItem(TOKEN_KEY);
};

/**
 * Validates if the stored JWT token is still valid (not expired)
 * @returns true if token exists and is not expired, false otherwise
 */
export const isTokenValid = (): boolean => {
  try {
    const token = getToken();

    if (!token) {
      return false;
    }

    // Decode JWT payload using jwt-decode library
    const payload = jwtDecode<JwtPayload>(token);

    // Check if token has expiration and if it's still valid
    if (!payload.exp) {
      return false;
    }

    // Compare expiration time (in seconds) with current time
    const currentTimestamp = Math.floor(Date.now() / 1000);
    return payload.exp > currentTimestamp;
  } catch (error) {
    // If any error occurs during decode, consider token invalid
    console.error("Error validating token:", error);
    return false;
  }
};
