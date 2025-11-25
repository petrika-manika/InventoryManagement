import type { ReactNode } from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import LoadingSpinner from "../common/LoadingSpinner";

/**
 * ProtectedRoute component props
 */
interface ProtectedRouteProps {
  children: ReactNode;
}

/**
 * Protected Route Component
 * Wraps routes that require authentication
 * Redirects to login page if user is not authenticated
 * Shows loading spinner while checking authentication status
 *
 * @example
 * ```tsx
 * <Route
 *   path="/users"
 *   element={
 *     <ProtectedRoute>
 *       <UsersPage />
 *     </ProtectedRoute>
 *   }
 * />
 * ```
 */
const ProtectedRoute = ({ children }: ProtectedRouteProps) => {
  const { isAuthenticated, isLoading } = useAuth();

  // Show loading spinner while checking authentication
  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  // Redirect to login if not authenticated
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  // Render protected content
  return <>{children}</>;
};

export default ProtectedRoute;
