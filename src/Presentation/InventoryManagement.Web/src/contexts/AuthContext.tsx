import { createContext, useState, useEffect, useContext } from "react";
import type { ReactNode } from "react";
import type { UserDto } from "../types/user.types";
import authService from "../services/authService";
import { getToken, isTokenValid } from "../utils/tokenStorage";

/**
 * Authentication context type definition
 */
interface AuthContextType {
  user: UserDto | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
}

/**
 * Authentication Context
 * Provides authentication state and methods throughout the application
 */
const AuthContext = createContext<AuthContextType | undefined>(undefined);

/**
 * AuthProvider Props
 */
interface AuthProviderProps {
  children: ReactNode;
}

/**
 * Authentication Provider Component
 * Wraps the application to provide authentication state and methods
 *
 * @example
 * ```tsx
 * <AuthProvider>
 *   <App />
 * </AuthProvider>
 * ```
 */
export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [user, setUser] = useState<UserDto | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  // Computed value - user is authenticated if user object exists
  const isAuthenticated = user !== null;

  /**
   * Initialize authentication state on mount
   * Checks for existing valid token and fetches current user
   */
  useEffect(() => {
    const initializeAuth = async () => {
      const token = getToken();

      // Check if token exists and is still valid
      if (token && isTokenValid()) {
        try {
          // Fetch current user data
          const userData = await authService.getCurrentUser();
          setUser(userData);
        } catch (error) {
          // Token is invalid or user fetch failed, clear authentication
          console.error("Failed to fetch current user:", error);
          authService.logout();
        }
      }

      setIsLoading(false);
    };

    initializeAuth();
  }, []);

  /**
   * Authenticates user with email and password
   * @param email - User's email address
   * @param password - User's password
   * @throws Error if login fails
   */
  const login = async (email: string, password: string): Promise<void> => {
    const result = await authService.login({ email, password });
    setUser(result.user);
  };

  /**
   * Logs out the current user
   * Clears user state and removes authentication token
   */
  const logout = (): void => {
    setUser(null);
    authService.logout();
  };

  const value: AuthContextType = {
    user,
    isAuthenticated,
    isLoading,
    login,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

/**
 * Custom hook to access authentication context
 * Must be used within AuthProvider
 *
 * @returns Authentication context value
 * @throws Error if used outside AuthProvider
 *
 * @example
 * ```tsx
 * const { user, isAuthenticated, login, logout } = useAuth();
 * ```
 */
export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);

  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }

  return context;
};
