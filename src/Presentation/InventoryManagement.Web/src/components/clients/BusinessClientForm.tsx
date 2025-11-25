/**
 * Business Client Form Component
 * Form for creating or editing business clients
 */

import React, { useState, useEffect } from "react";
import toast from "react-hot-toast";
import Modal from "../common/Modal";
import Input from "../common/Input";
import Textarea from "../common/Textarea";
import Button from "../common/Button";
import type {
  BusinessClientDto,
  CreateBusinessClientRequest,
  UpdateBusinessClientRequest,
} from "../../types/client.types";

interface BusinessClientFormProps {
  isOpen: boolean;
  onClose: () => void;
  client?: BusinessClientDto;
  onSubmit: (
    data: CreateBusinessClientRequest | UpdateBusinessClientRequest
  ) => Promise<void>;
}

const BusinessClientForm: React.FC<BusinessClientFormProps> = ({
  isOpen,
  onClose,
  client,
  onSubmit,
}) => {
  const [formData, setFormData] = useState({
    nipt: client?.nipt || "",
    ownerFirstName: client?.ownerFirstName || "",
    ownerLastName: client?.ownerLastName || "",
    ownerPhoneNumber: client?.ownerPhoneNumber || "",
    contactPersonFirstName: client?.contactPersonFirstName || "",
    contactPersonLastName: client?.contactPersonLastName || "",
    contactPersonPhoneNumber: client?.contactPersonPhoneNumber || "",
    address: client?.address || "",
    email: client?.email || "",
    phoneNumber: client?.phoneNumber || "",
    notes: client?.notes || "",
  });

  const [errors, setErrors] = useState<Record<string, string>>({});
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setFormData({
      nipt: client?.nipt || "",
      ownerFirstName: client?.ownerFirstName || "",
      ownerLastName: client?.ownerLastName || "",
      ownerPhoneNumber: client?.ownerPhoneNumber || "",
      contactPersonFirstName: client?.contactPersonFirstName || "",
      contactPersonLastName: client?.contactPersonLastName || "",
      contactPersonPhoneNumber: client?.contactPersonPhoneNumber || "",
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

    // NIPT validation
    if (!formData.nipt || formData.nipt.trim().length === 0) {
      newErrors.nipt = "NIPT is required";
    } else if (!/^[A-Za-z0-9]{10}$/.test(formData.nipt)) {
      newErrors.nipt = "NIPT must be exactly 10 alphanumeric characters";
    }

    // Contact Person validation (required)
    if (
      !formData.contactPersonFirstName ||
      formData.contactPersonFirstName.trim().length === 0
    ) {
      newErrors.contactPersonFirstName =
        "Contact person first name is required";
    } else if (formData.contactPersonFirstName.length > 50) {
      newErrors.contactPersonFirstName =
        "Contact person first name must be 50 characters or less";
    }

    if (
      !formData.contactPersonLastName ||
      formData.contactPersonLastName.trim().length === 0
    ) {
      newErrors.contactPersonLastName = "Contact person last name is required";
    } else if (formData.contactPersonLastName.length > 50) {
      newErrors.contactPersonLastName =
        "Contact person last name must be 50 characters or less";
    }

    // Owner validation (optional, but if provided, validate)
    if (formData.ownerFirstName && formData.ownerFirstName.length > 50) {
      newErrors.ownerFirstName =
        "Owner first name must be 50 characters or less";
    }

    if (formData.ownerLastName && formData.ownerLastName.length > 50) {
      newErrors.ownerLastName = "Owner last name must be 50 characters or less";
    }

    // Phone number validation
    const phoneRegex = /^[\d\s+\-()]+$/;
    if (formData.phoneNumber && !phoneRegex.test(formData.phoneNumber)) {
      newErrors.phoneNumber = "Invalid phone number format";
    }

    if (
      formData.ownerPhoneNumber &&
      !phoneRegex.test(formData.ownerPhoneNumber)
    ) {
      newErrors.ownerPhoneNumber = "Invalid phone number format";
    }

    if (
      formData.contactPersonPhoneNumber &&
      !phoneRegex.test(formData.contactPersonPhoneNumber)
    ) {
      newErrors.contactPersonPhoneNumber = "Invalid phone number format";
    }

    // Email validation
    if (formData.email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = "Invalid email format";
    }

    // Address validation
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
        nipt: formData.nipt.trim(),
        ownerFirstName: formData.ownerFirstName.trim() || undefined,
        ownerLastName: formData.ownerLastName.trim() || undefined,
        ownerPhoneNumber: formData.ownerPhoneNumber.trim() || undefined,
        contactPersonFirstName: formData.contactPersonFirstName.trim(),
        contactPersonLastName: formData.contactPersonLastName.trim(),
        contactPersonPhoneNumber:
          formData.contactPersonPhoneNumber.trim() || undefined,
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
      title={client ? "Edit Business Client" : "Create Business Client"}
      maxWidth="lg"
    >
      <form onSubmit={handleSubmit}>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          {/* Business Information Section */}
          <h3 className="md:col-span-2 text-sm font-semibold text-gray-700 mt-2 mb-2 border-b pb-2">
            Business Information
          </h3>

          {/* NIPT */}
          <div className="md:col-span-2">
            <Input
              label="NIPT"
              name="nipt"
              type="text"
              value={formData.nipt}
              onChange={handleChange}
              error={errors.nipt}
              required
              placeholder="Enter 10-character NIPT"
              maxLength={10}
            />
          </div>

          {/* Email */}
          <Input
            label="Email"
            name="email"
            type="email"
            value={formData.email}
            onChange={handleChange}
            error={errors.email}
            placeholder="Enter business email"
          />

          {/* Phone Number */}
          <Input
            label="Phone Number"
            name="phoneNumber"
            type="text"
            value={formData.phoneNumber}
            onChange={handleChange}
            error={errors.phoneNumber}
            placeholder="Enter business phone"
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
              placeholder="Enter business address"
            />
          </div>

          {/* Owner Information Section */}
          <h3 className="md:col-span-2 text-sm font-semibold text-gray-700 mt-4 mb-2 border-b pb-2">
            Owner Information (Optional)
          </h3>

          {/* Owner First Name */}
          <Input
            label="Owner First Name"
            name="ownerFirstName"
            type="text"
            value={formData.ownerFirstName}
            onChange={handleChange}
            error={errors.ownerFirstName}
            placeholder="Enter owner first name"
          />

          {/* Owner Last Name */}
          <Input
            label="Owner Last Name"
            name="ownerLastName"
            type="text"
            value={formData.ownerLastName}
            onChange={handleChange}
            error={errors.ownerLastName}
            placeholder="Enter owner last name"
          />

          {/* Owner Phone Number */}
          <div className="md:col-span-2">
            <Input
              label="Owner Phone Number"
              name="ownerPhoneNumber"
              type="text"
              value={formData.ownerPhoneNumber}
              onChange={handleChange}
              error={errors.ownerPhoneNumber}
              placeholder="Enter owner phone number"
            />
          </div>

          {/* Contact Person Section */}
          <h3 className="md:col-span-2 text-sm font-semibold text-gray-700 mt-4 mb-2 border-b pb-2">
            Contact Person
          </h3>

          {/* Contact Person First Name */}
          <Input
            label="Contact First Name"
            name="contactPersonFirstName"
            type="text"
            value={formData.contactPersonFirstName}
            onChange={handleChange}
            error={errors.contactPersonFirstName}
            required
            placeholder="Enter contact first name"
          />

          {/* Contact Person Last Name */}
          <Input
            label="Contact Last Name"
            name="contactPersonLastName"
            type="text"
            value={formData.contactPersonLastName}
            onChange={handleChange}
            error={errors.contactPersonLastName}
            required
            placeholder="Enter contact last name"
          />

          {/* Contact Person Phone Number */}
          <div className="md:col-span-2">
            <Input
              label="Contact Phone Number"
              name="contactPersonPhoneNumber"
              type="text"
              value={formData.contactPersonPhoneNumber}
              onChange={handleChange}
              error={errors.contactPersonPhoneNumber}
              placeholder="Enter contact phone number"
            />
          </div>

          {/* Notes Section */}
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

export default BusinessClientForm;
