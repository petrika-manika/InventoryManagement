import { useState, useEffect } from "react";
import { format } from "date-fns";
import toast from "react-hot-toast";
import {
  PlusIcon,
  PencilIcon,
  CheckIcon,
  XMarkIcon,
  MagnifyingGlassIcon,
} from "@heroicons/react/24/outline";
import userService from "../services/userService";
import type {
  UserDto,
  CreateUserRequest,
  UpdateUserRequest,
} from "../types/user.types";
import Button from "../components/common/Button";
import Modal from "../components/common/Modal";
import ConfirmationDialog from "../components/common/ConfirmationDialog";
import {
  Table,
  TableHeader,
  TableBody,
  TableRow,
  TableHead,
  TableCell,
} from "../components/common/Table";
import Badge from "../components/common/Badge";
import LoadingSpinner from "../components/common/LoadingSpinner";
import UserForm from "../components/users/UserForm";

/**
 * Formats a date string to a readable format
 * @param dateString - ISO date string
 * @returns Formatted date string
 */
const formatDate = (dateString: string): string => {
  try {
    return format(new Date(dateString), "MMM dd, yyyy");
  } catch {
    return "Invalid date";
  }
};

/**
 * Users Page Component
 * Main page for user management with CRUD operations
 *
 * Features:
 * - List all users with search
 * - Create new users
 * - Edit existing users
 * - Activate/Deactivate users
 * - Real-time status updates
 */
const UsersPage = () => {
  // User data state
  const [users, setUsers] = useState<UserDto[]>([]);
  const [filteredUsers, setFilteredUsers] = useState<UserDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  // Search state
  const [searchTerm, setSearchTerm] = useState("");

  // Modal and form state
  const [selectedUser, setSelectedUser] = useState<UserDto | null>(null);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showConfirmDialog, setShowConfirmDialog] = useState(false);
  const [confirmAction, setConfirmAction] = useState<{
    type: "activate" | "deactivate";
    userId: string;
  } | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  /**
   * Loads all users from the API
   */
  const loadUsers = async () => {
    setIsLoading(true);
    try {
      const data = await userService.getAllUsers();
      setUsers(data);
      setFilteredUsers(data);
    } catch (error) {
      toast.error("Failed to load users");
      console.error("Error loading users:", error);
    } finally {
      setIsLoading(false);
    }
  };

  // Load users on component mount
  useEffect(() => {
    loadUsers();
  }, []);

  // Filter users based on search term
  useEffect(() => {
    if (!searchTerm.trim()) {
      setFilteredUsers(users);
    } else {
      const term = searchTerm.toLowerCase();
      const filtered = users.filter(
        (user) =>
          user.firstName.toLowerCase().includes(term) ||
          user.lastName.toLowerCase().includes(term) ||
          user.email.toLowerCase().includes(term)
      );
      setFilteredUsers(filtered);
    }
  }, [searchTerm, users]);

  /**
   * Handles creating a new user
   */
  const handleCreateUser = async (
    data: CreateUserRequest | UpdateUserRequest
  ) => {
    setIsSubmitting(true);
    try {
      await userService.createUser(data as CreateUserRequest);
      toast.success("User created successfully");
      setShowCreateModal(false);
      await loadUsers();
    } catch (error) {
      const errorMessage =
        (error as { message?: string })?.message || "Failed to create user";
      toast.error(errorMessage);
    } finally {
      setIsSubmitting(false);
    }
  };

  /**
   * Handles updating an existing user
   */
  const handleUpdateUser = async (
    data: CreateUserRequest | UpdateUserRequest
  ) => {
    setIsSubmitting(true);
    try {
      const updateData = data as UpdateUserRequest;
      await userService.updateUser(updateData.userId, updateData);
      toast.success("User updated successfully");
      setShowEditModal(false);
      setSelectedUser(null);
      await loadUsers();
    } catch (error) {
      const errorMessage =
        (error as { message?: string })?.message || "Failed to update user";
      toast.error(errorMessage);
    } finally {
      setIsSubmitting(false);
    }
  };

  /**
   * Opens confirmation dialog for activating a user
   */
  const handleActivate = (userId: string) => {
    setConfirmAction({ type: "activate", userId });
    setShowConfirmDialog(true);
  };

  /**
   * Opens confirmation dialog for deactivating a user
   */
  const handleDeactivate = (userId: string) => {
    setConfirmAction({ type: "deactivate", userId });
    setShowConfirmDialog(true);
  };

  /**
   * Executes the confirmed action (activate/deactivate)
   */
  const handleConfirmAction = async () => {
    if (!confirmAction) return;

    setIsSubmitting(true);
    try {
      if (confirmAction.type === "activate") {
        await userService.activateUser(confirmAction.userId);
        toast.success("User activated successfully");
      } else {
        await userService.deactivateUser(confirmAction.userId);
        toast.success("User deactivated successfully");
      }
      setShowConfirmDialog(false);
      setConfirmAction(null);
      await loadUsers();
    } catch (error) {
      const errorMessage =
        (error as { message?: string })?.message ||
        `Failed to ${confirmAction.type} user`;
      toast.error(errorMessage);
    } finally {
      setIsSubmitting(false);
    }
  };

  /**
   * Opens the edit modal with selected user data
   */
  const openEditModal = (user: UserDto) => {
    setSelectedUser(user);
    setShowEditModal(true);
  };

  /**
   * Closes the edit modal and clears selected user
   */
  const closeEditModal = () => {
    setShowEditModal(false);
    setSelectedUser(null);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Header Section */}
        <div className="mb-8">
          <div className="flex justify-between items-center mb-2">
            <div>
              <h1 className="text-3xl font-bold text-gray-900">
                User Management
              </h1>
              <p className="mt-1 text-sm text-gray-600">
                Manage system users and their access
              </p>
            </div>
            <Button
              variant="primary"
              onClick={() => setShowCreateModal(true)}
              className="flex items-center"
            >
              <PlusIcon className="h-5 w-5 mr-2" />
              Create User
            </Button>
          </div>
        </div>

        {/* Search Bar */}
        <div className="mb-6 bg-white rounded-lg shadow p-4">
          <div className="relative">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <MagnifyingGlassIcon className="h-5 w-5 text-gray-400" />
            </div>
            <input
              type="text"
              className="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
              placeholder="Search users by name or email..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>

        {/* Loading State */}
        {isLoading && (
          <div className="flex flex-col items-center justify-center py-12">
            <LoadingSpinner size="lg" />
            <p className="mt-4 text-gray-600">Loading users...</p>
          </div>
        )}

        {/* Empty State */}
        {!isLoading && filteredUsers.length === 0 && (
          <div className="bg-white rounded-lg shadow p-12 text-center">
            <p className="text-gray-600 text-lg">
              {searchTerm
                ? "No users found matching your search."
                : "No users found."}
            </p>
            {searchTerm && (
              <button
                onClick={() => setSearchTerm("")}
                className="mt-4 text-primary-600 hover:text-primary-700 font-medium"
              >
                Clear search
              </button>
            )}
          </div>
        )}

        {/* Users Table */}
        {!isLoading && filteredUsers.length > 0 && (
          <div className="bg-white rounded-lg shadow overflow-hidden">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Name</TableHead>
                  <TableHead>Email</TableHead>
                  <TableHead>Status</TableHead>
                  <TableHead>Created At</TableHead>
                  <TableHead className="text-right">Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredUsers.map((user) => (
                  <TableRow key={user.id}>
                    <TableCell>
                      <div className="font-medium text-gray-900">
                        {user.fullName}
                      </div>
                    </TableCell>
                    <TableCell>
                      <div className="text-gray-600">{user.email}</div>
                    </TableCell>
                    <TableCell>
                      <Badge variant={user.isActive ? "success" : "danger"}>
                        {user.isActive ? "Active" : "Inactive"}
                      </Badge>
                    </TableCell>
                    <TableCell>
                      <div className="text-gray-600">
                        {formatDate(user.createdAt)}
                      </div>
                    </TableCell>
                    <TableCell>
                      <div className="flex justify-end space-x-2">
                        <button
                          onClick={() => openEditModal(user)}
                          className="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                          title="Edit user"
                        >
                          <PencilIcon className="h-5 w-5" />
                        </button>
                        {user.isActive ? (
                          <button
                            onClick={() => handleDeactivate(user.id)}
                            className="p-2 text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                            title="Deactivate user"
                          >
                            <XMarkIcon className="h-5 w-5" />
                          </button>
                        ) : (
                          <button
                            onClick={() => handleActivate(user.id)}
                            className="p-2 text-green-600 hover:bg-green-50 rounded-lg transition-colors"
                            title="Activate user"
                          >
                            <CheckIcon className="h-5 w-5" />
                          </button>
                        )}
                      </div>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
        )}

        {/* Create User Modal */}
        <Modal
          isOpen={showCreateModal}
          onClose={() => setShowCreateModal(false)}
          title="Create New User"
          maxWidth="lg"
        >
          <UserForm
            onSubmit={handleCreateUser}
            onCancel={() => setShowCreateModal(false)}
            isLoading={isSubmitting}
          />
        </Modal>

        {/* Edit User Modal */}
        <Modal
          isOpen={showEditModal}
          onClose={closeEditModal}
          title="Edit User"
          maxWidth="lg"
        >
          {selectedUser && (
            <UserForm
              initialData={selectedUser}
              isEditMode
              onSubmit={handleUpdateUser}
              onCancel={closeEditModal}
              isLoading={isSubmitting}
            />
          )}
        </Modal>

        {/* Confirm Action Dialog */}
        <ConfirmationDialog
          isOpen={showConfirmDialog}
          onClose={() => {
            setShowConfirmDialog(false);
            setConfirmAction(null);
          }}
          onConfirm={handleConfirmAction}
          title="Confirm Action"
          message={
            confirmAction?.type === "activate"
              ? "Are you sure you want to activate this user? They will be able to access the system."
              : "Are you sure you want to deactivate this user? They will lose access to the system."
          }
          confirmLabel={
            confirmAction?.type === "activate" ? "Activate" : "Deactivate"
          }
          confirmVariant="danger"
          isLoading={isSubmitting}
        />
      </div>
    </div>
  );
};

export default UsersPage;
