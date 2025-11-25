import { useState } from "react";
import type { FormEvent } from "react";
import Input from "../common/Input";
import Button from "../common/Button";
import type {
  CreateUserRequest,
  UpdateUserRequest,
} from "../../types/user.types";

/**
 * UserForm component props
 */
export interface UserFormProps {
  /** Initial form data for edit mode */
  initialData?: {
    id?: string;
    firstName: string;
    lastName: string;
    email: string;
    password?: string;
  };
  /** Form submission handler */
  onSubmit: (data: CreateUserRequest | UpdateUserRequest) => Promise<void>;
  /** Cancel button handler */
  onCancel: () => void;
  /** Whether the form is in edit mode */
  isEditMode?: boolean;
  /** Whether the form is submitting */
  isLoading?: boolean;
}

/**
 * User Form Component
 * Handles both creating new users and editing existing users
 *
 * @example
 * ```tsx
 * // Create mode
 * <UserForm
 *   onSubmit={handleCreateUser}
 *   onCancel={() => setShowForm(false)}
 * />
 *
 * // Edit mode
 * <UserForm
 *   initialData={user}
 *   isEditMode
 *   onSubmit={handleUpdateUser}
 *   onCancel={() => setShowForm(false)}
 * />
 * ```
 */
const UserForm = ({
  initialData,
  onSubmit,
  onCancel,
  isEditMode = false,
  isLoading = false,
}: UserFormProps) => {
  const [firstName, setFirstName] = useState(initialData?.firstName || "");
  const [lastName, setLastName] = useState(initialData?.lastName || "");
  const [email, setEmail] = useState(initialData?.email || "");
  const [password, setPassword] = useState("");
  const [errors, setErrors] = useState<{
    firstName?: string;
    lastName?: string;
    email?: string;
    password?: string;
  }>({});

  /**
   * Validates all form fields
   * @returns true if form is valid, false otherwise
   */
  const validateForm = (): boolean => {
    const newErrors: typeof errors = {};

    // First Name validation
    if (!firstName.trim()) {
      newErrors.firstName = "First name is required";
    } else if (firstName.length > 100) {
      newErrors.firstName = "First name must be less than 100 characters";
    }

    // Last Name validation
    if (!lastName.trim()) {
      newErrors.lastName = "Last name is required";
    } else if (lastName.length > 100) {
      newErrors.lastName = "Last name must be less than 100 characters";
    }

    // Email validation
    if (!email.trim()) {
      newErrors.email = "Email is required";
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      newErrors.email = "Invalid email format";
    }

    // Password validation (only for create mode)
    if (!isEditMode) {
      if (!password) {
        newErrors.password = "Password is required";
      } else if (password.length < 6) {
        newErrors.password = "Password must be at least 6 characters";
      }
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  /**
   * Handles form submission
   * @param e - Form event
   */
  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setErrors({});

    if (!validateForm()) {
      return;
    }

    // Prepare data based on mode
    if (isEditMode && initialData?.id) {
      const updateData: UpdateUserRequest = {
        userId: initialData.id,
        firstName: firstName.trim(),
        lastName: lastName.trim(),
        email: email.trim(),
      };
      await onSubmit(updateData);
    } else {
      const createData: CreateUserRequest = {
        firstName: firstName.trim(),
        lastName: lastName.trim(),
        email: email.trim(),
        password,
      };
      await onSubmit(createData);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <Input
        label="First Name"
        type="text"
        value={firstName}
        onChange={(e) => setFirstName(e.target.value)}
        error={errors.firstName}
        placeholder="John"
        required
      />

      <Input
        label="Last Name"
        type="text"
        value={lastName}
        onChange={(e) => setLastName(e.target.value)}
        error={errors.lastName}
        placeholder="Doe"
        required
      />

      <Input
        label="Email"
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        error={errors.email}
        placeholder="john.doe@example.com"
        required
      />

      {!isEditMode && (
        <Input
          label="Password"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          error={errors.password}
          placeholder="••••••••"
          helperText="Must be at least 6 characters"
          required
        />
      )}

      <div className="flex justify-end space-x-3 pt-4">
        <Button
          type="button"
          variant="secondary"
          onClick={onCancel}
          disabled={isLoading}
        >
          Cancel
        </Button>
        <Button type="submit" variant="primary" isLoading={isLoading}>
          {isEditMode ? "Update User" : "Create User"}
        </Button>
      </div>
    </form>
  );
};

export default UserForm;
