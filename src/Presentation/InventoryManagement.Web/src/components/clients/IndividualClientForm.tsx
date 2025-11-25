/**
 * Individual Client Form Component
 * Form for creating or editing individual clients
 */

import React, { useState, useEffect } from "react";
import toast from "react-hot-toast";
import Modal from "../common/Modal";
import Input from "../common/Input";
import Textarea from "../common/Textarea";
import Button from "../common/Button";
import type {
  IndividualClientDto,
  CreateIndividualClientRequest,
  UpdateIndividualClientRequest,
} from "../../types/client.types";

interface IndividualClientFormProps {
  isOpen: boolean;
  onClose: () => void;
  client?: IndividualClientDto;
  onSubmit: (
    data: CreateIndividualClientRequest | UpdateIndividualClientRequest
  ) => Promise<void>;
}

const IndividualClientForm: React.FC<IndividualClientFormProps> = ({
  isOpen,
  onClose,
  client,
  onSubmit,
}) => {
  const [formData, setFormData] = useState({
    firstName: client?.firstName || "",
    lastName: client?.lastName || "",
    address: client?.address || "",
    email: client?.email || "",
    phoneNumber: client?.phoneNumber || "",
    notes: client?.notes || "",
  });

  const [errors, setErrors] = useState<Record<string, string>>({});
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setFormData({
      firstName: client?.firstName || "",
      lastName: client?.lastName || "",
      address: client?.address || "",
      email: client?.email || "",
      phoneNumber: client?.phoneNumber || "",
      notes: client?.notes || "",
    });
    setErrors({});
  }, [client, isOpen]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    // Validate
    const newErrors: Record<string, string> = {};

    if (!formData.firstName || formData.firstName.trim().length === 0) {
      newErrors.firstName = "First name is required";
    } else if (formData.firstName.length > 50) {
      newErrors.firstName = "First name must be 50 characters or less";
    }

    if (!formData.lastName || formData.lastName.trim().length === 0) {
      newErrors.lastName = "Last name is required";
    } else if (formData.lastName.length > 50) {
      newErrors.lastName = "Last name must be 50 characters or less";
    }

    if (formData.email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = "Invalid email format";
    }

    if (formData.phoneNumber && !/^[\d\s+\-()]+$/.test(formData.phoneNumber)) {
      newErrors.phoneNumber = "Invalid phone number format";
    }

    if (formData.address && formData.address.length > 500) {
      newErrors.address = "Address must be 500 characters or less";
    }

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    setLoading(true);
    try {
      // Clean data - convert empty strings to undefined
      const cleanedData = {
        firstName: formData.firstName.trim(),
        lastName: formData.lastName.trim(),
        address: formData.address.trim() || undefined,
        email: formData.email.trim() || undefined,
        phoneNumber: formData.phoneNumber.trim() || undefined,
        notes: formData.notes.trim() || undefined,
      };

      await onSubmit(cleanedData);
      toast.success(
        client ? "Client updated successfully" : "Client created successfully"
      );
      onClose();
    } catch (error: unknown) {
      const errorMessage =
        error && typeof error === "object" && "message" in error
          ? String(error.message)
          : "Failed to save client";
      toast.error(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    // Clear error for this field when user starts typing
    if (errors[name]) {
      setErrors((prev) => {
        const newErrors = { ...prev };
        delete newErrors[name];
        return newErrors;
      });
    }
  };

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title={client ? "Edit Individual Client" : "Create Individual Client"}
      maxWidth="lg"
    >
      <form onSubmit={handleSubmit}>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          {/* First Name */}
          <Input
            label="First Name"
            name="firstName"
            type="text"
            value={formData.firstName}
            onChange={handleChange}
            error={errors.firstName}
            required
            placeholder="Enter first name"
          />

          {/* Last Name */}
          <Input
            label="Last Name"
            name="lastName"
            type="text"
            value={formData.lastName}
            onChange={handleChange}
            error={errors.lastName}
            required
            placeholder="Enter last name"
          />

          {/* Email */}
          <Input
            label="Email"
            name="email"
            type="email"
            value={formData.email}
            onChange={handleChange}
            error={errors.email}
            placeholder="Enter email address"
          />

          {/* Phone Number */}
          <Input
            label="Phone Number"
            name="phoneNumber"
            type="text"
            value={formData.phoneNumber}
            onChange={handleChange}
            error={errors.phoneNumber}
            placeholder="Enter phone number"
          />

          {/* Address */}
          <div className="md:col-span-2">
            <Textarea
              label="Address"
              name="address"
              value={formData.address}
              onChange={handleChange}
              error={errors.address}
              rows={2}
              placeholder="Enter address"
            />
          </div>

          {/* Notes */}
          <div className="md:col-span-2">
            <Textarea
              label="Notes"
              name="notes"
              value={formData.notes}
              onChange={handleChange}
              rows={3}
              placeholder="Add any additional notes"
            />
          </div>
        </div>

        {/* Form Actions */}
        <div className="flex justify-end gap-3 mt-6">
          <Button variant="secondary" onClick={onClose} type="button">
            Cancel
          </Button>
          <Button type="submit" isLoading={loading}>
            {client ? "Update Client" : "Create Client"}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default IndividualClientForm;
