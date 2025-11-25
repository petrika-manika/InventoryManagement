import type { ReactNode } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import {
  ArrowRightOnRectangleIcon,
  UserCircleIcon,
  CubeIcon,
  ClockIcon,
  UserGroupIcon,
  ChartBarIcon,
  UsersIcon,
} from "@heroicons/react/24/outline";

/**
 * Layout component props
 */
interface LayoutProps {
  children: ReactNode;
}

/**
 * Layout Component
 * Provides consistent page structure with header, navigation, and content area
 *
 * @example
 * ```tsx
 * <Layout>
 *   <UsersPage />
 * </Layout>
 * ```
 */
const Layout = ({ children }: LayoutProps) => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  /**
   * Handles user logout
   */
  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header/Navbar */}
      <header className="bg-primary-600 shadow-lg fixed top-0 left-0 right-0 z-40">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center h-16">
            {/* Logo/Title */}
            <div className="flex items-center">
              <NavLink
                to="/dashboard"
                className="flex items-center space-x-3 text-white hover:text-primary-100 transition-colors"
              >
                <div className="h-8 w-8 bg-white rounded-lg flex items-center justify-center">
                  <svg
                    className="h-6 w-6 text-primary-600"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth={2}
                      d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4"
                    />
                  </svg>
                </div>
                <span className="text-xl font-bold">Inventory Management</span>
              </NavLink>
            </div>

            {/* Navigation Links */}
            <nav className="hidden md:flex items-center space-x-2">
              <NavLink
                to="/dashboard"
                className={({ isActive }) =>
                  `flex items-center space-x-2 px-4 py-2 rounded-md transition-colors ${
                    isActive
                      ? "bg-primary-700 text-white"
                      : "text-primary-100 hover:bg-primary-600 hover:text-white"
                  }`
                }
              >
                <ChartBarIcon className="h-5 w-5" />
                <span className="text-sm font-medium">Dashboard</span>
              </NavLink>
              <NavLink
                to="/products"
                className={({ isActive }) =>
                  `flex items-center space-x-2 px-4 py-2 rounded-md transition-colors ${
                    isActive
                      ? "bg-primary-700 text-white"
                      : "text-primary-100 hover:bg-primary-600 hover:text-white"
                  }`
                }
              >
                <CubeIcon className="h-5 w-5" />
                <span className="text-sm font-medium">Products</span>
              </NavLink>
              <NavLink
                to="/clients"
                className={({ isActive }) =>
                  `flex items-center space-x-2 px-4 py-2 rounded-md transition-colors ${
                    isActive
                      ? "bg-primary-700 text-white"
                      : "text-primary-100 hover:bg-primary-600 hover:text-white"
                  }`
                }
              >
                <UsersIcon className="h-5 w-5" />
                <span className="text-sm font-medium">Clients</span>
              </NavLink>
              <NavLink
                to="/stock-history"
                className={({ isActive }) =>
                  `flex items-center space-x-2 px-4 py-2 rounded-md transition-colors ${
                    isActive
                      ? "bg-primary-700 text-white"
                      : "text-primary-100 hover:bg-primary-600 hover:text-white"
                  }`
                }
              >
                <ClockIcon className="h-5 w-5" />
                <span className="text-sm font-medium">Stock History</span>
              </NavLink>
              <NavLink
                to="/users"
                className={({ isActive }) =>
                  `flex items-center space-x-2 px-4 py-2 rounded-md transition-colors ${
                    isActive
                      ? "bg-primary-700 text-white"
                      : "text-primary-100 hover:bg-primary-600 hover:text-white"
                  }`
                }
              >
                <UserGroupIcon className="h-5 w-5" />
                <span className="text-sm font-medium">Users</span>
              </NavLink>
            </nav>

            {/* User Info & Logout */}
            <div className="flex items-center space-x-4">
              {user && (
                <div className="hidden sm:flex items-center space-x-2 text-white">
                  <UserCircleIcon className="h-8 w-8" />
                  <div className="flex flex-col">
                    <span className="text-sm font-medium">{user.fullName}</span>
                    <span className="text-xs text-primary-100">
                      {user.email}
                    </span>
                  </div>
                </div>
              )}
              <button
                onClick={handleLogout}
                className="flex items-center space-x-2 bg-primary-700 hover:bg-primary-800 text-white px-4 py-2 rounded-lg text-sm font-medium transition-colors"
                title="Logout"
              >
                <ArrowRightOnRectangleIcon className="h-5 w-5" />
                <span className="hidden sm:inline">Logout</span>
              </button>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content Area */}
      <main className="pt-16">{children}</main>
    </div>
  );
};

export default Layout;
